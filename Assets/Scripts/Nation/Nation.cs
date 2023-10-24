using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class Nation : MonoBehaviour
{

    void LateUpdate()
    {
        if (this.capitalLandSquare != null && this.capitalLandSquare.GetComponent<LandSquare>().factionOwner != this.nationName) 
        {
            CapitalLost();
        }

    }


    //public float metricTonOf = 0;
    public string nationName;

    public float metricTonsOfIronOre = 0;

    public float metricTonsOfIron = 0;

    public float metricTonsOfFood = 0;

    public float metricTonsOfWater = 0;

    public float metricTonsOfSteel = 0;

    public float population = 0;

    public float gold = 0;

    public float goldIncomePerHour = 0;

    public float goldExpencesPerHour = 0;





    //military supplies
    public float rifles;

    public float rifleBullets;

    public float artillary;

    public float artillaryShells;



    //have net gain/day for each resource 

    public bool botControlled;

    public Color nationMainColor;

    public Military military = new Military();


    public GameObject nationActions;

    /**
     * NationNameString, relationship)
     * 
     * Relationship types: friendly, enemy, neutral
     */
    public Dictionary<string, string> nationRelations = new Dictionary<string, string>();


    public List<GameObject> ownedLandSquares = new List<GameObject>();

    /**
     * Ordered list of border land squares based on annex cost from high to low
     */
    public List<GameObject> borderLandSquares = new List<GameObject>();

    public GameObject capitalLandSquare;

    public Dictionary<string, (int x, int y)> majorCities = new Dictionary<string, (int x, int y)>();

    
    //public Dictionary<(int, int), GameObject> borderLandSquares = new Dictionary<(int, int), GameObject>();

    //public bool 

    public Nation() 
    {
        this.military = new Military();
    }


    /**
     * If Capitol is lost, nation looses all military and territory
     * 
     */
    public void CapitalLost() 
    {

        for (int i = 0; i < this.military.units.Count; i++) 
        {
            this.military.units[i].GetComponent<Unit>().DestroyUnit();
            //Destroy(this.military.units[i]);
        }
        for (int i = 0; i < this.ownedLandSquares.Count; i++) 
        {
            //this.ownedLandSquares[i].GetComponent<LandSquare>().factionOwner = "";
            //this.ownedLandSquares.Remove(this.ownedLandSquares[i]);

        }
    
    
    }

    public void AddToBorderList(GameObject landSquare) 
    {

        if (borderLandSquares.Count == 0)
        {
            borderLandSquares.Add(landSquare);
        }
        else 
        {
            for (int i = 0; i < this.borderLandSquares.Count; i++)
            {
                if (borderLandSquares[i].GetComponent<LandSquare>().CalculateAnnexCost(this.gameObject) >= landSquare.GetComponent<LandSquare>().CalculateAnnexCost(this.gameObject))
                {
                    borderLandSquares.Insert(i, landSquare);
                    break;
                }

            }

        }

        

    
    
    }

    /**
     * Gets Nation Relationship string, if does not exist, returns null
     * 
     */
    public string GetNationRelationship(string otherNationName) 
    {
        if (nationRelations.ContainsKey(otherNationName))
        {
            return nationRelations[otherNationName];
        }
        else 
        {
            return null;
        }
        
    
    }

    public bool IsSameNation(string nationName) 
    {
        if (this.nationName == nationName)
        {
            return true;
        }
        else 
        {
            return false;
        } 
    
    }

    public bool IsEnemyNation(string nationName) 
    {

        if (nationRelations.ContainsKey(nationName) && nationRelations[nationName] == "enemy")
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public float GetAndSetGoldExpencesPerHour() 
    {
        float goldExpencesPerHour = 0;
        for (int i = 0; i < ownedLandSquares.Count; i++) 
        {
            if (ownedLandSquares[i].GetComponent<LandSquare>().buildings.Count > 0) 
            {
                for (int k = 0; k < ownedLandSquares[i].GetComponent<LandSquare>().buildings.Count; k++) 
                {
                    if (ownedLandSquares[i].GetComponent<LandSquare>().buildings[k].IsActive) 
                    {
                        goldExpencesPerHour += ownedLandSquares[i].GetComponent<LandSquare>().buildings[k].MaintenanceCostPerHour;
                    }
                    
                }
                //goldExpencesPerHour 
            }
            
        }
        this.goldExpencesPerHour = goldExpencesPerHour;
        return goldExpencesPerHour;
    }

    public float GetAndSetPopulation() 
    {
        this.population = 0;
        for (int i = 0; i < ownedLandSquares.Count; i++) 
        {
            //Debug.Log("i: " + i + " max: " + ownedLandSquares.Count);
            this.population += (ownedLandSquares[i].GetComponent(typeof(LandSquare)) as LandSquare).population;
            
        }
        return this.population;
    }

    public float GetAndSetGoldIncomePerHour() 
    {

        //US makes $1.7235 per person per hour
        this.goldIncomePerHour = population * 1.7235f;
        return population * 1.7235f;
    }

    public void IncreaseGoldFor1Hour() 
    {
        gold += GetAndSetGoldIncomePerHour();


    }

    public float GetDistanceFromCapitol(GameObject landSquare) 
    {
        int capitalX = capitalLandSquare.GetComponent<LandSquare>().x;
        int capitalY = capitalLandSquare.GetComponent<LandSquare>().y;

        int landSquareX = landSquare.GetComponent<LandSquare>().x;
        int landSquareY = landSquare.GetComponent<LandSquare>().y;

        int distance = (int)Math.Sqrt(Math.Pow(landSquareX - capitalX, 2) + Math.Pow(landSquareY - capitalY, 2));
        return distance;
    
    }

   public string NationToString() 
    {
        string output = "";
        output += "Nation: " + nationName;
        output += "\nPopulation: " + Math.Round(population);

        output += "\n";


        if (gold > 1000000)
        {
            output += "\nGold: " + Math.Round((double)(gold / 1000000), 3) + " million";
            //Math.Round((double)(gold / 1000000), 2);
        }
        else 
        {
            output += "\nGold: " + Math.Round((double)(gold), 3);
        }

        
        if (goldIncomePerHour > 1000000)
        {
            output += "\nGoldIncomePerHour: " + Math.Round((double)(goldIncomePerHour / 1000000), 3) + " million";
        }
        else
        {
            output += "\nGoldIncomePerHour: " + Math.Round((double)(goldIncomePerHour), 3);
        }

        if (goldExpencesPerHour > 1000000)
        {
            output += "\nGoldExpencesPerHour: " + Math.Round((double)(goldExpencesPerHour / 1000000), 3) + " million";
        }
        else
        {
            output += "\nGoldExpencesPerHour: " + Math.Round((double)(goldExpencesPerHour), 3);
        }

        if (goldIncomePerHour - goldExpencesPerHour > 1000000)
        {
            output += "\nNetGoldPerHour: " + Math.Round((double)((goldIncomePerHour - goldExpencesPerHour) / 1000000), 3) + " million";
        }
        else
        {
            output += "\nNetGoldPerHour: " + Math.Round((double)((goldIncomePerHour - goldExpencesPerHour)), 3);
        }

        output += "\n";

        if (metricTonsOfIronOre > 1000000)
        {
            output += "\nMetricTonsOfIronOre: " + Math.Round((double)(metricTonsOfIronOre / 1000000), 3) + " million";
        }
        else
        {
            output += "\nMetricTonsOfIronOre: " + Math.Round((double)(metricTonsOfIronOre), 3);
        }
        if (metricTonsOfIronOre > 1000000)
        {
            output += "\nMetricTonsOfSteel: " + Math.Round((double)(metricTonsOfSteel / 1000000), 3) + " million";
        }
        else
        {
            output += "\nMetricTonsOfSteel: " + Math.Round((double)(metricTonsOfSteel), 3);
        }

        output += "\n";


        output += "\nMilitary Reserves: " + (int)military.totalForce;


        output += "\n";


        output += "\nFriends: ";
        foreach (var item in nationRelations)
        {
            if (item.Value == "friendly")
            {
                output += item.Key + ", ";
            }

        }


        output += "\nEnemies: ";
        foreach (var item in nationRelations)
        {
            if (item.Value == "enemy")
            {
                output += item.Key + ", ";
            }

        }

        output += "\n";

        output += "\nCasualtiesInflicted: " + Math.Round((double)(military.casualtiesInflicted));
        output += "\nCasualtiesSuffered: " + Math.Round((double)(military.casualtiesSuffered));


        output += "\n";
        return output;
    }


    public void GetBorderLandSquaresOneNation(GameObject nation) 
    {
        //look through all landsquares and see which ones are border squares
        //for () 
        //{
        
        
        //}
    
    }
   
}
