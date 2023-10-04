using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Entrench());

        StartCoroutine(UpdateUnit());
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    

    public string unitName;

    public GameObject nation;

    public float currentForce;
    public float maxForce;


    public float entrenchmentPercent = 0;

    public bool hasOrders;

    public GameObject map;

    public GameObject currentLandSquare;


    //keep track of counts of vehicles, rifles, vehicles, ect


    public IEnumerator UpdateUnit()
    {
        while (true) 
        {
            StartCoroutine(Entrench());
            Debug.Log("Updating Unit");
            yield return new WaitForSeconds(10);

        }
        
        
    }



    public IEnumerator MoveOrders(int goalX, int goalY)
    {
        if (hasOrders) 
        {
            yield break;//breaks out 
        }
        hasOrders = true;
        
        bool destinationReached = false;

        int currentX = currentLandSquare.GetComponent<LandSquare>().x;
        int currentY = currentLandSquare.GetComponent<LandSquare>().y;

        //int goalX;
        //int goalY;


        while (!destinationReached) 
        {
            entrenchmentPercent = 0;
            yield return new WaitForSeconds(10);
            
            if (currentX > goalX)
            {
                currentX -=1;
            }
            else if (currentX < goalX) 
            {
                currentX += 1;
            }

            if (currentY > goalY)
            {
                currentY -= 1;
            }
            else if (currentY < goalY)
            {
                currentY += 1;
            }

            //wait based on terrain

            //move off of landSquare
            if (map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().units.Count == 0)//TODO: make it to where a unit can move to another square with friendly units only
            {
                currentLandSquare.GetComponent<LandSquare>().units.Remove(gameObject);


                map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().units.Add(gameObject);
                currentLandSquare = map.GetComponent<Map>().worldLandSquares[currentX, currentY];

                gameObject.transform.position = map.GetComponent<Map>().worldLandSquares[currentX, currentY].transform.position;

                if (map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().factionOwner != "") 
                {
                    //if unit set to annex:
                    UnitAnnexLandSquareFromNation(this.nation, map.GetComponent<Map>().nations[map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().factionOwner], map.GetComponent<Map>().worldLandSquares[currentX, currentY]);
                }

                
            }
            else
            {
                destinationReached = true;//not reached, but needs to break out of method
            }


            if (goalX == currentX && goalY == currentY) 
            {
                destinationReached = true;

            }

            //how long it takes to move to a landSquare
            //TODO: Account for terrain and infrastructure
            //yield return new WaitForSeconds(10);
        }




        hasOrders = false;

    }



    public void UnitAnnexLandSquareFromNation(GameObject nation, GameObject nationLoosing, GameObject landsquare)
    {
        

        //take land square away
        nationLoosing.GetComponent<Nation>().ownedLandSquares.Remove(landsquare);

        //add land square to nation
        landsquare.GetComponent<LandSquare>().factionOwner = nation.GetComponent<Nation>().nationName;
        nation.GetComponent<Nation>().ownedLandSquares.Add(landsquare);
        map.GetComponent<Map>().UpdateBorders();

        //add dissaproval rating if land square likes nationLoosing more
        if (landsquare.GetComponent<LandSquare>().nationApprovalRatings[nationLoosing.GetComponent<Nation>().nationName].positiveApproval > landsquare.GetComponent<LandSquare>().nationApprovalRatings[nation.GetComponent<Nation>().nationName].positiveApproval) 
        {
            float annexMinNegativeApprovalPercent = .3f;
            float annexMaxNegativeApprovalPercent = 1;


            float dissaprovalRating = Random.Range(annexMinNegativeApprovalPercent, annexMaxNegativeApprovalPercent);
            landsquare.GetComponent<LandSquare>().nationApprovalRatings[nation.GetComponent<Nation>().nationName].DecreaseApproval(landsquare.GetComponent<LandSquare>().nationApprovalRatings[nation.GetComponent<Nation>().nationName].neutralApproval * dissaprovalRating);

        }


    }

    public void CancalAllOrders() 
    {
        StopAllCoroutines();
    }

    public IEnumerator Entrench()
    {
        if (!hasOrders && entrenchmentPercent < 100)
        {
            int hoursToEntrench = 10;//12 hrs

            
            for (int i = 0; i < hoursToEntrench; i++) 
            {
                if (hasOrders) 
                {
                    yield break;
                }

                yield return new WaitForSeconds(1);
                if (entrenchmentPercent < 100) 
                {
                    entrenchmentPercent += 10;
                }
            }
        }
        else 
        {
            yield break;

        }
    
    }


    









        public string UnitToString() 
    {
        string output = "";
        output += "Name: " + unitName;
        output += "\nCurrent Force: " + currentForce;
        output += "\nStrength: " + ((currentForce / maxForce) * 100 + "%");
        output += "\nEntrenchment: " + entrenchmentPercent + "%";
        output += "\nHasOrders: " + hasOrders;
        return output;
    }






}
