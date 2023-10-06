using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Entrench());

        //StartCoroutine(UpdateUnit());
        PatrollWithinBordersOrder();
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

    public bool patrolling;

    //roadMoveSpeed, offroadMoveSpeed

    public GameObject map;

    public GameObject currentLandSquare;


    //keep track of counts of vehicles, rifles, vehicles, ect


    public IEnumerator UpdateUnit()
    {
        while (true) 
        {
            StartCoroutine(Entrench());
            //Debug.Log("Updating Unit");
            yield return new WaitForSeconds(10);

        }
        
        
    }



    public IEnumerator MoveOrders(int goalX, int goalY)
    {
        int currentX = currentLandSquare.GetComponent<LandSquare>().x;
        int currentY = currentLandSquare.GetComponent<LandSquare>().y;

        if (hasOrders || (goalX == currentX && goalY == currentY)) 
        {
            yield break;//breaks out 
        }
        hasOrders = true;
        
        bool destinationReached = false;

        

        //int goalX;
        //int goalY;


        while (!destinationReached) 
        {
            entrenchmentPercent = 0;
            
            AddActionIndicator("", GetDirectionHeading(currentX, currentY, goalX, goalY));
            yield return new WaitForSeconds(10);//10
            

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



        RemoveActionIndicator();
        hasOrders = false;

    }


    
    public string GetDirectionHeading(int currX, int currY, int destinationX, int destinationY) 
    {
        if (destinationY - currY > 0 && destinationX == currX)
        {
            return "N";

        }
        else if (destinationY - currY < 0 && destinationX == currX)
        {
            return "S";

        }
        else if (destinationX - currX < 0 && destinationY == currY)
        {
            return "E";

        }
        else if (destinationX - currX > 0 && destinationY == currY)
        {
            return "W";

        }
        else 
        {
            return "FAIL";
        }

    }

    /**
     * 
     * 
     */
    public void AddActionIndicator(string action, string direction) 
    {
        //Actons: move, attack
        //directions: N, S, E, W, NE, NW, SE, SW


        GameObject newActionIndicator = (GameObject)Instantiate(Resources.Load("Prefabs/Military/ActionIndicator/WhiteArrow1"));
        newActionIndicator.transform.position = this.transform.position;

        float currX = newActionIndicator.transform.position.x;
        float currY = newActionIndicator.transform.position.y;

        float space = 2.5f;

        //set rotation and position based on what its doing
        if (direction == "N" || direction == "n")
        {
            newActionIndicator.transform.position = new Vector3(currX, currY + space);
            //newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == "W" || direction == "2")
        {
            newActionIndicator.transform.position = new Vector3(currX + space, currY);
            newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (direction == "S" || direction == "s")
        {
            newActionIndicator.transform.position = new Vector3(currX, currY - space);
            newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        else if (direction == "E" || direction == "e")
        {
            newActionIndicator.transform.position = new Vector3(currX - space, currY);
            newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, -270);
        }
        else 
        {
            Destroy(newActionIndicator);
        }



        if (newActionIndicator != null) 
        {
            newActionIndicator.transform.SetParent(transform);
        }
        
    }

    public void RemoveActionIndicator() 
    {
        if (this.transform.Find("WhiteArrow1(Clone)") != null) 
        {
            Destroy(this.transform.Find("WhiteArrow1(Clone)").gameObject);
        }
        
        /*
        foreach (var child in this.transform)
        {
            if (child is GameObject && ((GameObject)child).name.Contains("WhiteArrow1"))
            {
                //child.Get.name.Contains("WhiteArrow1")
                Destroy((GameObject)child);
            }
        }
        */

    }


    public void PatrollWithinBordersOrder() 
    {
        StartCoroutine(PatrollWithinBorders());
    
    }

    private IEnumerator PatrollWithinBorders() 
    {
        
        while (true)
        {
             yield return new WaitForSeconds(.1f);
            
            int randX = Random.Range(currentLandSquare.GetComponent<LandSquare>().x - 1, currentLandSquare.GetComponent<LandSquare>().x + 2);
            int randY = Random.Range(currentLandSquare.GetComponent<LandSquare>().y - 1, currentLandSquare.GetComponent<LandSquare>().y + 2);
            if (randX >= 0 && randY >= 0 && map.GetComponent<Map>().worldSize > randX && map.GetComponent<Map>().worldSize > randY  && map.GetComponent<Map>().worldLandSquares[randX, randY].GetComponent<LandSquare>().factionOwner == this.nation.GetComponent<Nation>().nationName && map.GetComponent<Map>().worldLandSquares[randX, randY].GetComponent<LandSquare>().units.Count == 0) 
            {
                StartCoroutine(MoveOrders(randX, randY));
            }
            

        }
       

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
        StartCoroutine(UpdateUnit());
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
