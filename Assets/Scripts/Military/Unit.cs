using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Entrench());

        StartCoroutine(UpdateUnit());
        //PatrollWithinBordersOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentForce < 1) 
        {
            this.DestroyUnit();
        }
        
        
    }

    

    public string unitName;

    public GameObject nation;

    public float currentForce;
    public float maxForce;

    /**
     * TODO:
     * When maxed, unit is where it wants to be for defencive manuvers and is able to start entrenching
     */
    public float favorableTerrainPercent = 0;

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
            if (!hasOrders) 
            {
                RemoveActionIndicator();
            }

            if (patrolling) 
            {
                //StartCoroutine(PatrollWithinBordersAndEnemy());
            }
            
            StartCoroutine(Entrench());

            if (this.currentForce < this.maxForce) 
            {
                StartCoroutine(ResupplyUnit());
            }
            
            //Debug.Log("Updating Unit");
            yield return new WaitForSeconds(10);

        }
        
        
    }


    public IEnumerator ResupplyUnit() 
    {

        int distanceFromCapitol = (int)this.nation.GetComponent<Nation>().GetDistanceFromCapitol(this.currentLandSquare);

        int timeToTravelOneSquare = 10;

        yield return new WaitForSeconds(distanceFromCapitol * timeToTravelOneSquare * 2);

        float forceNeeded = maxForce - currentForce;
        if (this.nation.GetComponent<Nation>().military.totalForce >= forceNeeded) 
        {
            this.nation.GetComponent<Nation>().military.totalForce -= forceNeeded;
            currentForce += forceNeeded;
        }
        

    }

    private bool UnitWithinOneSquare(int goalX, int goalY) 
    {
        return this.currentLandSquare.GetComponent<LandSquare>().x - goalX <= 1 && this.currentLandSquare.GetComponent<LandSquare>().x - goalX >= -1 && this.currentLandSquare.GetComponent<LandSquare>().y - goalY <= 1 && this.currentLandSquare.GetComponent<LandSquare>().y - goalY >= -1;


    }

    public void AttackOrders(int goalX, int goalY) 
    {
        
        if (!hasOrders && this.currentLandSquare.GetComponent<LandSquare>().x - goalX <= 1 && this.currentLandSquare.GetComponent<LandSquare>().x - goalX >= -1 && this.currentLandSquare.GetComponent<LandSquare>().y - goalY <= 1 && this.currentLandSquare.GetComponent<LandSquare>().y - goalY >= -1)
        {
            if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units.Count == 0 || map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units[0].GetComponent<Unit>().nation.GetComponent<Nation>().nationName == this.nation.GetComponent<Nation>().nationName)
            {
                //this.hasOrders = false;

            }
            else 
            {
                hasOrders = true;
                StartCoroutine(InitializeAttack(goalX, goalY));
            }
            
        }
    }

    private IEnumerator InitializeAttack(int goalX, int goalY) 
    {
        string directionHeading = this.GetDirectionHeading(this.currentLandSquare.GetComponent<LandSquare>().x, this.currentLandSquare.GetComponent<LandSquare>().y, goalX, goalY);
        this.AddActionIndicator("attack", directionHeading);
        hasOrders = true;
        yield return new WaitForSeconds(8);

        StartCoroutine(Attack(goalX, goalY));
        //yield break;
    }

    /**
     * 
     */
    private IEnumerator Attack(int goalX, int goalY) 
    {
        bool finishedWithAttack = false;
        this.hasOrders = true;
        //TODO:
        //Make attacks affected by force, entrenchment, intel(affects readyness), intel
        //Make long range attacks by artillary and tanks(tank barage affected greatly by sight lines)
        //Attack affected by specifications of firearms and how much ammo unit has left
        //units not entrenched or in defence positions are at great risk of artillary damage

       
        
        while (!finishedWithAttack) 
        {

            yield return new WaitForSeconds(1);//
            float attackForce = this.currentForce;
            float defenceForce = 0;





            //if no units on square or unit is friendly, then cancal attack
            //else, make nations enemies if not already
            if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units.Count == 0 || map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units[0].GetComponent<Unit>().nation.GetComponent<Nation>().nationName == this.nation.GetComponent<Nation>().nationName)
            {
                finishedWithAttack = true;
                this.hasOrders = false;
                RemoveActionIndicator();
                yield break;
            }
            else 
            {
                //nation declares war
                
                nation.GetComponent<Nation>().nationActions.GetComponent<NationActions>().DeclareEnemyNationBothSides(map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units[0].GetComponent<Unit>().nation.GetComponent<Nation>().nationName);
            }

            //calculate damage done on each side based on force, entrenchment, intel(affects readyness), 
            //currentForce - combinedForce of units on landsquare
            for (int i = 0; i < map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units.Count; i++)
            {
                defenceForce += map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units[i].GetComponent<Unit>().currentForce;

            }

            //300 rounds per kill in close/civil war distance combat
            //7k to 50k rounds to kill someone 
            //bolt action: 1 to 3 rounds per second depending on experience 
            float roundsPerKill = 300;
            float fireRatePerHour = 10;


            //get base attackForce damage
            float attackForceDamage = fireRatePerHour * attackForce / roundsPerKill;
            float defenceForceDamage = fireRatePerHour * defenceForce / roundsPerKill;




            //entrecnhment changes
            float entrenchmentAttackChanges = -1 * attackForceDamage * (entrenchmentPercent / 100) * .8f;
            float entrenchmentDefenceChanges = defenceForceDamage * (entrenchmentPercent / 100) * 2f;
            //attackForceDamage -= attackForceDamage * (entrenchmentPercent / 100) * .8f;
            //defenceForceDamage += defenceForceDamage * (entrenchmentPercent / 100) * 2f;



            float terrainAttackChanges = 0;
            float terrainDefenceChanges = 0;



            //terrain changes
            //this unit terrain changes
            if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().HasMajorCity() == true)
            {
                terrainDefenceChanges += defenceForceDamage * 2f;
                terrainAttackChanges -= attackForceDamage * .5f;

            }
            else 
            {
                
                if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Mountain"))
                {
                    terrainAttackChanges += attackForceDamage * .4f;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Hill"))
                {
                    terrainAttackChanges += attackForceDamage * .2f;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Grass"))
                {
                    terrainAttackChanges -= attackForceDamage * .2f;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Lake"))
                {
                    terrainAttackChanges += attackForceDamage * 0f;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Tree"))
                {
                    terrainAttackChanges += attackForceDamage * .1f;
                }

                //defending unit terrain changes
                if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().gameObject.name.Contains("Mountain"))
                {
                    terrainDefenceChanges += defenceForceDamage * 1.2f;
                    terrainAttackChanges -= attackForceDamage * .6f;
                }
                else if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().gameObject.name.Contains("Hill"))
                {
                    terrainDefenceChanges += defenceForceDamage * .6f;
                    terrainAttackChanges -= attackForceDamage * .3f;
                }
                else if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().gameObject.name.Contains("Grass"))
                {
                    //defenceForceDamage -= defenceForceDamage * .1f;
                    //attackForceDamage -= attackForceDamage * .5f;
                }
                else if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().gameObject.name.Contains("lake"))
                {
                    terrainDefenceChanges += defenceForceDamage * 0f;
                    terrainAttackChanges -= attackForceDamage * 0f;
                }
                else if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().gameObject.name.Contains("Tree"))
                {
                    terrainDefenceChanges += defenceForceDamage * 1f;
                    terrainAttackChanges -= attackForceDamage * .4f;
                }

            }


            //do damage to both sides
            attackForceDamage += entrenchmentAttackChanges + terrainAttackChanges;
            defenceForceDamage += entrenchmentDefenceChanges + terrainDefenceChanges;

            float damageToEachUnit = attackForceDamage / map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units.Count;
            this.currentForce -= defenceForceDamage;

            //damage each unit
            for (int i = 0; i < map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units.Count; i++)
            {
                map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units[i].GetComponent<Unit>().currentForce -= damageToEachUnit;

            }


            if ((currentForce / maxForce) <= .2) 
            {
                hasOrders = false;
                finishedWithAttack = true;
            }


            //make two nations enemies



        }
        RemoveActionIndicator();
    }





    public IEnumerator MoveOrders(int goalX, int goalY)
    {
        //TODO: make hoursTillFinished affected by movespeed, terrain, and infrastructure
        int hoursTillFinished = 10;
        int currentX = currentLandSquare.GetComponent<LandSquare>().x;
        int currentY = currentLandSquare.GetComponent<LandSquare>().y;

        if (hasOrders || (goalX == currentX && goalY == currentY)) 
        {
            yield break;//breaks out 
        }
        //RemoveActionIndicator();
        hasOrders = true;
        
        bool destinationReached = false;

        

        //int goalX;
        //int goalY;


        while (!destinationReached) 
        {
            entrenchmentPercent = 0;
            
            AddActionIndicator("", GetDirectionHeading(currentX, currentY, goalX, goalY));

            //if unit there, add attack orders
            

            if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().units.Count > 0 && UnitWithinOneSquare(goalX, goalY))
            {
                RemoveActionIndicator();
                hasOrders = false;
                AttackOrders(goalX, goalY);
                destinationReached = true;
                yield break;
            }
            

            yield return new WaitForSeconds(hoursTillFinished);


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

                //gameObject.transform.position = map.GetComponent<Map>().worldLandSquares[currentX, currentY].transform.position;
                gameObject.transform.position = new Vector3(map.GetComponent<Map>().worldLandSquares[currentX, currentY].transform.position.x, map.GetComponent<Map>().worldLandSquares[currentX, currentY].transform.position.y, gameObject.transform.position.z);

                if (map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().factionOwner != "") 
                //if (nation.GetComponent<Nation>().GetNationRelationship(map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().factionOwner) != null && nation.GetComponent<Nation>().GetNationRelationship(map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().factionOwner) == "enemy")
                {
                    nation.GetComponent<Nation>().GetNationRelationship(map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().factionOwner);
                    //if unit set to annex:
                    UnitAnnexLandSquareFromNation(this.nation, map.GetComponent<Map>().nations[map.GetComponent<Map>().worldLandSquares[currentX, currentY].GetComponent<LandSquare>().factionOwner], map.GetComponent<Map>().worldLandSquares[currentX, currentY]);
                    //nation declares war if not on unowned land
                    if (map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().factionOwner != "") 
                    {
                        nation.GetComponent<Nation>().nationActions.GetComponent<NationActions>().DeclareEnemyNationBothSides(map.GetComponent<Map>().worldLandSquares[goalX, goalY].GetComponent<LandSquare>().factionOwner);
                    }
                    
                }

                
            }
            else
            {
                hasOrders = false;
                RemoveActionIndicator();
                AttackOrders(goalX, goalY);
                destinationReached = true;//not reached, but needs to break out of method
                yield break;
            }


            if (goalX == currentX && goalY == currentY) 
            {
                destinationReached = true;
                RemoveActionIndicator();
                hasOrders = false;
                yield break;
            }

            //how long it takes to move to a landSquare
            //TODO: Account for terrain and infrastructure
            //yield return new WaitForSeconds(10);

            //hasOrders = false;
            RemoveActionIndicator();
        }



        //RemoveActionIndicator();
        //hasOrders = false;
        //RemoveActionIndicator();
    }


    public void DestroyUnit() 
    {
        
        this.currentLandSquare.GetComponent<LandSquare>().units.Remove(gameObject);
        this.nation.GetComponent<Nation>().military.units.Remove(gameObject);
        Destroy(this.gameObject);
    
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
        else if (destinationX - currX > 0 && destinationY - currY > 0)
        {
            return "NE";
        }
        else if (destinationX - currX < 0 && destinationY - currY < 0)
        {
            return "NW";
        }
        else if (destinationX - currX < 0 && destinationY - currY > 0)
        {
            return "SW";
        }
        else if (destinationX - currX > 0 && destinationY - currY < 0)
        {
            return "SE";
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
        if (action == "attack") 
        {
            newActionIndicator.GetComponent<SpriteRenderer>().color = Color.red;
        }
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
        else if (direction == "NE" || direction == "ne")
        {
            newActionIndicator.transform.position = new Vector3(currX + space, currY + space);
            newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, 320);
        }
        else if (direction == "SW" || direction == "sw")
        {
            newActionIndicator.transform.position = new Vector3(currX - space, currY + space);
            newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        else if (direction == "SE" || direction == "se")
        {
            newActionIndicator.transform.position = new Vector3(currX + space, currY - space);
            newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, -135);
        }
        else if (direction == "NW" || direction == "nw")
        {
            newActionIndicator.transform.position = new Vector3(currX - space, currY - space);
            newActionIndicator.transform.rotation = Quaternion.Euler(0, 0, 135);
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
            float waitTime = 0;
            float minWaitTime = 0;
            float maxWaitTime = 0;

            //this.currentLandSquare.GetComponent<LandSquare>().
            if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Mountain"))
            {
                minWaitTime = 5;
                maxWaitTime = 20;
            }
            else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Hill"))
            {
                minWaitTime = 5;
                maxWaitTime = 15;
            }
            else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Grass"))
            {
                minWaitTime = 0;
                maxWaitTime = 3;
            }
            else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Lake"))
            {
                minWaitTime = 0;
                maxWaitTime = 3;
            }
            else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Tree"))
            {
                minWaitTime = 5;
                maxWaitTime = 15;
            }

            waitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            
            int randX = UnityEngine.Random.Range(currentLandSquare.GetComponent<LandSquare>().x - 1, currentLandSquare.GetComponent<LandSquare>().x + 2);
            int randY = UnityEngine.Random.Range(currentLandSquare.GetComponent<LandSquare>().y - 1, currentLandSquare.GetComponent<LandSquare>().y + 2);
            if (randX >= 0 && randY >= 0 && map.GetComponent<Map>().worldSize > randX && map.GetComponent<Map>().worldSize > randY  && map.GetComponent<Map>().worldLandSquares[randX, randY].GetComponent<LandSquare>().factionOwner == this.nation.GetComponent<Nation>().nationName && map.GetComponent<Map>().worldLandSquares[randX, randY].GetComponent<LandSquare>().units.Count == 0) 
            {
                //hasOrders = false;
                StartCoroutine(MoveOrders(randX, randY));
            }
            

        }
       

    }

    public void PatrollWithinBordersAndEnemyOrder()
    {
        if (!hasOrders) 
        {
            StartCoroutine(PatrollWithinBordersAndEnemy());
        }
        

    }

    private IEnumerator PatrollWithinBordersAndEnemy()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(1);
            /*
            if (hasOrders)
            {
                yield break;
            }
            */

            //hasOrders = true;

            int randX = UnityEngine.Random.Range(currentLandSquare.GetComponent<LandSquare>().x - 1, currentLandSquare.GetComponent<LandSquare>().x + 2);
            int randY = UnityEngine.Random.Range(currentLandSquare.GetComponent<LandSquare>().y - 1, currentLandSquare.GetComponent<LandSquare>().y + 2);

            if (!hasOrders && randX >= 0 && randY >= 0 && map.GetComponent<Map>().worldSize > randX && map.GetComponent<Map>().worldSize > randY) 
            {
               
                //StopAllCoroutines();
                //StartCoroutine(UpdateUnit());
                float waitTime = 0;
                float minWaitTime = 0;
                float maxWaitTime = 0;

                //this.currentLandSquare.GetComponent<LandSquare>().
                if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Mountain"))
                {
                    minWaitTime = 5;
                    maxWaitTime = 20;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Hill"))
                {
                    minWaitTime = 5;
                    maxWaitTime = 15;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Grass"))
                {
                    minWaitTime = 0;
                    maxWaitTime = 3;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Lake"))
                {
                    minWaitTime = 0;
                    maxWaitTime = 3;
                }
                else if (this.currentLandSquare.GetComponent<LandSquare>().gameObject.name.Contains("Tree"))
                {
                    minWaitTime = 5;
                    maxWaitTime = 15;
                }

                waitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);


                
                bool isOwner = map.GetComponent<Map>().worldLandSquares[randX, randY].GetComponent<LandSquare>().factionOwner == this.nation.GetComponent<Nation>().nationName;
                bool hasUnits = map.GetComponent<Map>().worldLandSquares[randX, randY].GetComponent<LandSquare>().units.Count > 0;

                bool isOwnedByEnemy = nation.GetComponent<Nation>().IsEnemyNation(map.GetComponent<Map>().worldLandSquares[randX, randY].GetComponent<LandSquare>().factionOwner);
                if ((isOwnedByEnemy || (isOwner && !hasUnits)))
                {
                    //hasOrders = false;
                    StartCoroutine(MoveOrders(randX, randY));
                }

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


            float dissaprovalRating = UnityEngine.Random.Range(annexMinNegativeApprovalPercent, annexMaxNegativeApprovalPercent);
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
            int hoursToEntrench = 20;//12 hrs

            
            for (int i = 0; i < hoursToEntrench; i++) 
            {
                if (hasOrders) 
                {
                    yield break;
                }

                yield return new WaitForSeconds(1);
                if (entrenchmentPercent < 100) 
                {
                    entrenchmentPercent += 5;
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
        output += "\nCurrent Force: " + Math.Round((double)(currentForce), 0);//Math.Round((double)(currentForce), 3)
        output += "\nStrength: " + (Math.Round((double)((currentForce / maxForce) * 100), 1) + "%"); 
        output += "\nEntrenchment: " + entrenchmentPercent + "%";
        output += "\nHasOrders: " + hasOrders;
        return output;
    }






}
