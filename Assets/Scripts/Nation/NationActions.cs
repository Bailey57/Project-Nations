using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NationActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null) 
        {
            mainCamera = new GameObject();
            mainCamera.transform.SetParent(gameObject.transform);
        }

        annexMinNegativeApprovalPercent = .2f;
        annexMaxNegativeApprovalPercent = .7f;
        minHourSupportIncrease = .05f;
        maxHourSupportIncrease = .35f;
        hoursToLobby = 168;


        if (nation.GetComponent<Nation>().botControlled) 
        {
            StartCoroutine(ChooseNextAction());
        }
    }


    public GameObject nation;

    public float annexMinNegativeApprovalPercent = .2f;
    public float annexMaxNegativeApprovalPercent = .7f;


    
    public float minHourSupportIncrease = .05f;
    public float maxHourSupportIncrease = .35f;
    public float hoursToLobby = 168;

    BuildingFactory buildingFactory = new BuildingFactory();
    public GameObject mainCamera;//null if npc 
    public GameObject map;

    




    /**
     * Choose which landsquare to buy or build on based on what resources the natin needs. 
     * 
     */
    public IEnumerator ChooseNextAction() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < nation.GetComponent<Nation>().ownedLandSquares.Count; i++) 
            {

                //if no mine and ironAvalibility >= .3, then build mine 
                if (nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().ironAvalibility >= .3 && nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().buildings.Count < 1) 
                {
                    BuildMine(nation.GetComponent<Nation>().ownedLandSquares[i]);
                }



                (int, int) selectedLandSquare;
                //choose weather to buy the most or least expensive
                int randInt = Random.Range(0, 1000);
                if (randInt < 0)//FindLeastValuableBorderLandSquare only for now
                {
                    selectedLandSquare = FindMostValuableBorderLandSquare(nation.GetComponent<Nation>().ownedLandSquares);
                }
                else 
                {
                    selectedLandSquare = FindLeastValuableBorderLandSquare(nation.GetComponent<Nation>().ownedLandSquares);
                }

                



                if (selectedLandSquare.Item1 >= 0 && selectedLandSquare.Item2 >= 0)
                {
                    if (map.GetComponent<Map>().worldLandSquares[selectedLandSquare.Item1, selectedLandSquare.Item2].GetComponent<LandSquare>().factionOwner == "" && map.GetComponent<Map>().worldLandSquares[selectedLandSquare.Item1, selectedLandSquare.Item2].GetComponent<LandSquare>().nationApprovalRatings[nation.GetComponent<Nation>().nationName].positiveApproval <= 0)// 0
                    {
                        StartCoroutine(LobbyInLandSquare(nation, map.GetComponent<Map>().worldLandSquares[selectedLandSquare.Item1, selectedLandSquare.Item2]));
                        Debug.Log("Nation: " + nation.GetComponent<Nation>().name + " \nStarted Lobying in square: x" + selectedLandSquare.Item1 + " y" + selectedLandSquare.Item2);
                    }
                    else
                    {
                        AnnexLandSquare(nation, map.GetComponent<Map>().worldLandSquares[selectedLandSquare.Item1, selectedLandSquare.Item2]);
                        Debug.Log("Nation: " + nation.GetComponent<Nation>().name + " \nAnnexed square: x" + selectedLandSquare.Item1 + " y" + selectedLandSquare.Item2);
                    }
                    

                }
                

            }
        }
        
    }

    public void SetCapital(GameObject landSquare) 
    {
        nation.GetComponent<Nation>().capitalLandSquare = landSquare;
    }



    /**
     * If Nation looses control of capital square, move to new landSquare if it owns another city
     */
    public void ChangeCapital() 
    {
        //TODO
    }

    /**
     * Owned landsquares gain negative approval on the denounced nation based on their positive approval on their owner.
     */
    public void DenounceNation(GameObject nation) 
    {
        //TODO
    
    }


    /**
     * Claim a landsquare 
     */
    public void ClaimLandSquare(GameObject landSquare) 
    {
        //TODO

    }


    /**
     * Improves approval, decreases negative approval, then starts  
     */
    public void AnnounceInfrastructureImprovement() 
    {
        //TODO
        //ImproveLandSquareApproval
        //takes some time
        ImproveInfrastructure();
    }

    /**
     * 
     */
    public void ImproveInfrastructure()
    {
        //TODO
        //build time
    }



    /**
     * Runs through the owned landSquare list and finds the most valuble that borders that is not owned 
     * Return x and y of the mostValuableBorderLandSquare
     */
    public (int, int) FindMostValuableBorderLandSquare(List<GameObject> landSquareList) 
    {
        //only used when refering to non owned tiles
        int xPos = -1;
        int yPos = -1;
        float landValue = -1;
        //float nationOpinionInSquare;

        int xPosTmp;
        int yPosTmp;

        //only used when refering to owned landSquareList squares
        int landSquareListX;
        int landSquareListY;

        for (int i = 0;i < landSquareList.Count;i++) 
        {
            landSquareListX = landSquareList[i].GetComponent<LandSquare>().x;
            landSquareListY = landSquareList[i].GetComponent<LandSquare>().y;


            /*
            //Corners Square Tiles
            xPosTmp = landSquareListX + 1;
            yPosTmp = landSquareListY + 1;
            //Debug.Log("xPosTmp: " + xPosTmp + " yPosTmp: " + yPosTmp + " ");
            //Debug.Log(" Map1: " + map);
            //Debug.Log(" Map2: " + map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp]);
            //Debug.Log("MaxSize: " + map.GetComponent<Map>().worldSize);
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "") 
            {
                
                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue) 
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX - 1;
            yPosTmp = landSquareListY - 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX + 1;
            yPosTmp = landSquareListY - 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX - 1;
            yPosTmp = landSquareListY + 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            */
            xPosTmp = landSquareListX + 1;
            yPosTmp = landSquareListY;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            
            xPosTmp = landSquareListX - 1;
            yPosTmp = landSquareListY;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX;
            yPosTmp = landSquareListY + 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX;
            yPosTmp = landSquareListY - 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() > landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }

        }

        return (xPos, yPos);
    }


    /**
     * Runs through the owned landSquare list and finds the most valuble that borders that is not owned 
     * Return x and y of the mostValuableBorderLandSquare
     */
    public (int, int) FindLeastValuableBorderLandSquare(List<GameObject> landSquareList)
    {
        //only used when refering to non owned tiles
        int xPos = -1;
        int yPos = -1;
        float landValue = 99999999999;
        //float nationOpinionInSquare;

        int xPosTmp;
        int yPosTmp;

        //only used when refering to owned landSquareList squares
        int landSquareListX;
        int landSquareListY;

        for (int i = 0; i < landSquareList.Count; i++)
        {
            landSquareListX = landSquareList[i].GetComponent<LandSquare>().x;
            landSquareListY = landSquareList[i].GetComponent<LandSquare>().y;



            /*
            //Corners Square Tiles
            xPosTmp = landSquareListX + 1;
            yPosTmp = landSquareListY + 1;
            //Debug.Log("xPosTmp: " + xPosTmp + " yPosTmp: " + yPosTmp + " ");
            //Debug.Log(" Map1: " + map);
            //Debug.Log(" Map2: " + map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp]);
            //Debug.Log("MaxSize: " + map.GetComponent<Map>().worldSize);
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX - 1;
            yPosTmp = landSquareListY - 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX + 1;
            yPosTmp = landSquareListY - 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX - 1;
            yPosTmp = landSquareListY + 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            */
            xPosTmp = landSquareListX + 1;
            yPosTmp = landSquareListY;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX - 1;
            yPosTmp = landSquareListY;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX;
            yPosTmp = landSquareListY + 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }
            xPosTmp = landSquareListX;
            yPosTmp = landSquareListY - 1;
            if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize && map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == "")
            {

                if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().CalculateLandValue() < landValue)
                {
                    xPos = xPosTmp;
                    yPos = yPosTmp;
                    landValue = map.GetComponent<Map>().worldLandSquares[xPos, yPos].GetComponent<LandSquare>().CalculateLandValue();
                }
            }

        }

        return (xPos, yPos);
    }


    public LandSquare ChooseLandSquareToAnnex() 
    {
    

        return new LandSquare();
    }



    public IEnumerator LobbyInLandSquare(GameObject nation, GameObject landsquare) 
    {

        if (nation.GetComponent<Nation>().gold >= landsquare.GetComponent<LandSquare>().CalculateLandValue() / 2 && (landsquare.GetComponent<LandSquare>().factionOwner == null || nation.GetComponent<Nation>().nationName != landsquare.GetComponent<LandSquare>().factionOwner))
        {
            nation.GetComponent<Nation>().gold -= landsquare.GetComponent<LandSquare>().CalculateLandValue() / 2; //lobbying costs half the value of the tile

            for (int i = 0; i < hoursToLobby; i++)
            {
                yield return new WaitForSeconds(1);

                float ratingIncrease = Random.Range(minHourSupportIncrease, maxHourSupportIncrease);
                landsquare.GetComponent<LandSquare>().nationApprovalRatings[nation.GetComponent<Nation>().nationName].IncreaseApproval(ratingIncrease);
                //Debug.Log("Lobby hours left: " + (hoursToLobby - i) + " ratingIncrease: " + ratingIncrease);
            }

        }

    }

    public void PlayerLobbyInLandSquare()
    {
        if (mainCamera.GetComponent<ObjectClick>().selectedObject != null)
        {
            StartCoroutine(LobbyInLandSquare(gameObject.transform.parent.GetComponent<Nation>().gameObject, mainCamera.GetComponent<ObjectClick>().selectedObject));
            
        }

    }




    public void MovePopulationToSquare(GameObject nation, GameObject landsquare)
    {



    }



    //need enough money and influence in the area to annex
    //TODO: Make farther landsquares cost more based on how far they are from capitol
    public void AnnexLandSquare(GameObject nation, GameObject landsquare)
    {
        if (nation.GetComponent<Nation>().gold >= landsquare.GetComponent<LandSquare>().CalculateAnnexCost(nation) && nation.GetComponent<Nation>().nationName != landsquare.GetComponent<LandSquare>().factionOwner && landsquare.GetComponent<LandSquare>().IsBorderWithNation(nation)) 
        {
            //nation.GetComponent<Nation>().gold -= landsquare.GetComponent<LandSquare>().CalculateLandValue();
            nation.GetComponent<Nation>().gold -= landsquare.GetComponent<LandSquare>().CalculateAnnexCost(nation);

            landsquare.GetComponent<LandSquare>().factionOwner = nation.GetComponent<Nation>().nationName;
            nation.GetComponent<Nation>().ownedLandSquares.Add(landsquare);
            map.GetComponent<Map>().UpdateBorders();

            //adds dissaproval rating 
            float dissaprovalRating = Random.Range(annexMinNegativeApprovalPercent, annexMaxNegativeApprovalPercent);
            landsquare.GetComponent<LandSquare>().nationApprovalRatings[nation.GetComponent<Nation>().nationName].DecreaseApproval(landsquare.GetComponent<LandSquare>().nationApprovalRatings[nation.GetComponent<Nation>().nationName].neutralApproval * dissaprovalRating);


        }
        
    }

    public void Player1AnnexLandSquare()
    {

        if (map.GetComponent<Map>().players[0].GetComponentInChildren<ObjectClick>().selectedObject != null)
        //GetComponent<Camera>().GetComponent<ObjectClick>().selectedObject != null) 
        {
            AnnexLandSquare(map.GetComponent<Map>().players[0], map.GetComponent<Map>().players[0].GetComponentInChildren<ObjectClick>().selectedObject);
        }
        
        
        //AnnexLandSquare();
    }

    public void PlayerAnnexLandSquare()
    {
        //gameObject.transform.gets

        if (mainCamera.GetComponent<ObjectClick>().selectedObject != null)
        {
            
            AnnexLandSquare(gameObject.transform.parent.GetComponent<Nation>().gameObject, mainCamera.GetComponent<ObjectClick>().selectedObject);
        }

    }

    public void BuildMine(GameObject landSquare) 
    {
        if (gameObject.transform.parent.GetComponent<Nation>().nationName == landSquare.GetComponent<LandSquare>().factionOwner) 
        {
            float buildCost = landSquare.GetComponent<LandSquare>().ironAvalibility * 5000000;
            if (gameObject.transform.parent.GetComponent<Nation>().gold >= buildCost)
            {
                gameObject.transform.parent.GetComponent<Nation>().gold -= buildCost;
                landSquare.GetComponent<LandSquare>().buildings.Add(buildingFactory.BuildMine_lvl1(gameObject.transform.parent.GetComponent<Nation>().nationName));

                
            }
        }
    }

    public void PlayerBuildMine() 
    {
        //this.transform.parent
        Debug.Log("This: " + gameObject.name);
        Debug.Log("The Parent: " + gameObject.transform.parent.GetComponent<Nation>().nationName);
        if (mainCamera.GetComponent<ObjectClick>().selectedObject != null)
        {

            BuildMine(mainCamera.GetComponent<ObjectClick>().selectedObject);
        }
    }


    public void LobbyInSquare(GameObject nation, GameObject landsquare) 
    {

        nation.GetComponent<Nation>().gold -= landsquare.GetComponent<LandSquare>().CalculateLandValue();
        landsquare.GetComponent<LandSquare>().factionOwner = nation.GetComponent<Nation>().nationName;
        nation.GetComponent<Nation>().ownedLandSquares.Add(landsquare);
        map.GetComponent<Map>().UpdateBorders();
    }


    public void CalculateBestDecision() 
    {
    }

    public void SelectSqureToAnnex() 
    {
    
    }



}
