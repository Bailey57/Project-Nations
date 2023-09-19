using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nation : MonoBehaviour
{

    

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

    public List<GameObject> ownedLandSquares = new List<GameObject>();


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

   public string NationToString() 
    {
        string output = "";
        output += "Nation: " + nationName;
        output += "\nPopulation: " + Math.Round(population);
        if (gold > 1000000)
        {
            output += "\nGold: " + Math.Round((double)(gold / 1000000), 3) + " million";
            //Math.Round((double)(gold / 1000000), 2);
        }
        else 
        {
            output += "\nGold: " + Math.Round((double)(gold), 3);
        }

        if (metricTonsOfIronOre > 1000000)
        {
            output += "\nMetricTonsOfIronOre: " + Math.Round((double)(metricTonsOfIronOre / 1000000), 3) + " million";
        }
        else
        {
            output += "\nMetricTonsOfIronOre: " + Math.Round((double)(metricTonsOfIronOre), 3);
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

        return output;
    }


   
}
