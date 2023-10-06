using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nation : MonoBehaviour
{

    void LateUpdate()
    {
        if (this.capitalLandSquare.GetComponent<LandSquare>().factionOwner != this.nationName) 
        {
            CapitolLost();
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


    //have net gain/day for each resource 

    public bool botControlled;

    public Color nationMainColor;

    public Military military = new Military();

    public List<GameObject> ownedLandSquares = new List<GameObject>();

    public GameObject capitalLandSquare;

    public Dictionary<string, (int x, int y)> majorCities = new Dictionary<string, (int x, int y)>();

    public Dictionary<(int, int), GameObject> borderLandSquares = new Dictionary<(int, int), GameObject>();

    //public bool 

    public Nation() 
    {
        this.military = new Military();
    }


    /**
     * If Capitol is lost, nation looses all military and territory
     * 
     */
    public void CapitolLost() 
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

        output += "\n";


        output += "\nMilitary Reserves: " + (int)military.totalForce;


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
