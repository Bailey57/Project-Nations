using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        StartCoroutine(UpdateNation());
        //ConscriptMilitaryPersonnelOnePercent();
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


    //









    public IEnumerator UpdateNation()
    {

        while (true)
        {
            yield return new WaitForSeconds(1);
            this.ConscriptMilitaryPersonnelOnePercent();
        }
    }


    /**
     * Choose which landsquare to buy or build on based on what resources the natin needs. 
     * 
     */
    public IEnumerator ChooseNextAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);




            if (nation.GetComponent<Nation>().military.totalForce > 250)
            {
                //CreateCompanyUnit();
                CreatePlatoonUnit();



                for (int i = 1; i < nation.GetComponent<Nation>().military.units.Count; i++)
                {
                    if (!nation.GetComponent<Nation>().military.units[i].GetComponent<Unit>().hasOrders)
                    {
                        //nation.GetComponent<Nation>().military.units[i].GetComponent<Unit>().PatrollWithinBordersOrder();
                        nation.GetComponent<Nation>().military.units[i].GetComponent<Unit>().PatrollWithinBordersAndEnemyOrder();
                    }

                }
            }



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
                        //Debug.Log("Nation: " + nation.GetComponent<Nation>().name + " \nStarted Lobying in square: x" + selectedLandSquare.Item1 + " y" + selectedLandSquare.Item2);
                    }
                    else
                    {
                        AnnexLandSquare(nation, map.GetComponent<Map>().worldLandSquares[selectedLandSquare.Item1, selectedLandSquare.Item2]);
                        //Debug.Log("Nation: " + nation.GetComponent<Nation>().name + " \nAnnexed square: x" + selectedLandSquare.Item1 + " y" + selectedLandSquare.Item2);
                    }


                }


            }
        }

    }



    public void DeclareEnemyNation(string nationName) 
    {
        if (this.nation.GetComponent<Nation>().nationName == nationName) 
        {
            return;
        }
        if (nation.GetComponent<Nation>().nationRelations.ContainsKey(nationName))
        {
            nation.GetComponent<Nation>().nationRelations[nationName] = "enemy";

        }
        else 
        {
            nation.GetComponent<Nation>().nationRelations.Add(nationName, "enemy");
        }
    
    }

    public void DeclareEnemyNationBothSides(string nationName)
    {
        if (this.nation.GetComponent<Nation>().nationName == nationName)
        {
            return;
        }

        //map.GetComponent<Map>().nations[nationName].GetComponent<Nation>()
        string thisNationName = nation.GetComponent<Nation>().nationName;
        if (map.GetComponent<Map>().nations[nationName].GetComponent<Nation>().nationRelations.ContainsKey(thisNationName))
        {
            map.GetComponent<Map>().nations[nationName].GetComponent<Nation>().nationRelations[thisNationName] = "enemy";

        }
        else
        {
            map.GetComponent<Map>().nations[nationName].GetComponent<Nation>().nationRelations.Add(thisNationName, "enemy");
        }


        
        if (nation.GetComponent<Nation>().nationRelations.ContainsKey(nationName))
        {
            nation.GetComponent<Nation>().nationRelations[nationName] = "enemy";

        }
        else
        {
            nation.GetComponent<Nation>().nationRelations.Add(nationName, "enemy");
        }

    }

    /**
     * 
     * 
     */
    private void ConscriptMilitaryPersonnel(float populationMilitaryPercentage)
    {
        for (int i = 0; i < nation.GetComponent<Nation>().ownedLandSquares.Count; i++)
        {
            nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().populationPercentageInMilitary = populationMilitaryPercentage;

            float neededPersonell = populationMilitaryPercentage * (nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().population);
            neededPersonell -= nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().populationAmmountInMilitary;
            nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().populationAmmountInMilitary += neededPersonell;
            //nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().population -= neededPersonell;
            nation.GetComponent<Nation>().military.totalForce += neededPersonell;

            //Debug.Log("i: " + i + " \nneededPersonell: " + neededPersonell + " \npopulationAmmountInMilitary: " + nation.GetComponent<Nation>().ownedLandSquares[i].GetComponent<LandSquare>().populationAmmountInMilitary + " \nTotal force: " + nation.GetComponent<Nation>().military.totalForce);
        }


    }

    public void GiveUnitMoveOrders(GameObject unit, GameObject landSquare) 
    {
        if (unit.GetComponent<Unit>() && landSquare.GetComponent<LandSquare>()) 
        {
            unit.GetComponent<Unit>().MoveOrders(landSquare.GetComponent<LandSquare>().x, landSquare.GetComponent<LandSquare>().y);
        }
    }




    public void CreateUnit(int forceSize) 
    {

        GameObject newUnit;
        if (forceSize >= 100 && forceSize <= 250)
        {
            newUnit = (GameObject)Instantiate(Resources.Load("Prefabs/Military/Units/infantry1CompanyWhite"));
        }
        else if (forceSize >= 25 && forceSize <= 40) 
        {
            newUnit = (GameObject)Instantiate(Resources.Load("Prefabs/Military/Units/infantry1White"));
            newUnit.GetComponentInChildren<TextMeshPro>().text = "•••";
        }
        else
        {
            newUnit = (GameObject)Instantiate(Resources.Load("Prefabs/Military/Units/infantry1White"));
        }
        
        newUnit.GetComponent<SpriteRenderer>().color = nation.GetComponent<Nation>().nationMainColor;
        newUnit.transform.position = nation.GetComponent<Nation>().capitalLandSquare.transform.position;
        nation.GetComponent<Nation>().capitalLandSquare.GetComponent<LandSquare>().units.Add(newUnit);
        newUnit.GetComponent<Unit>().maxForce = forceSize;
        newUnit.GetComponent<Unit>().map = map;
        newUnit.GetComponent<Unit>().currentLandSquare = nation.GetComponent<Nation>().capitalLandSquare;
        newUnit.GetComponent<Unit>().currentForce = forceSize;
        newUnit.GetComponent<Unit>().nation = nation;
        newUnit.GetComponent<Unit>().unitName = "newUnit";
        nation.GetComponent<Nation>().military.units.Add(newUnit);
    }



    public void CreatePlatoonUnit()
    {
        float size = 25;


        if (this.nation.GetComponent<Nation>().military.totalForce >= size)
        {
            this.nation.GetComponent<Nation>().military.totalForce -= size;
            CreateUnit((int)size);

        }

    }

    /**
     * Unit Sizes: https://en.wikipedia.org/wiki/Military_organization
     * 
     */
    public void CreateCompanyUnit() 
    {
        float companySize = 250;
        

        if (this.nation.GetComponent<Nation>().military.totalForce >= companySize) 
        {
            this.nation.GetComponent<Nation>().military.totalForce -= companySize;
            CreateUnit((int)companySize);

        }
    
    }



    public void CreateDivisionUnit()
    {
        float companySize = 10000;


        if (this.nation.GetComponent<Nation>().military.totalForce >= companySize)
        {
            this.nation.GetComponent<Nation>().military.totalForce -= companySize;
            CreateUnit((int)companySize);

        }

    }



    public void ConscriptMilitaryPersonnelOnePercent()
    {
        ConscriptMilitaryPersonnel(.01f);


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
