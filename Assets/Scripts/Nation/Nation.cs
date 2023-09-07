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


    //have net gain/day for each resource 


    public List<GameObject> ownedLandSquares = new List<GameObject>();

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

    public float GetGoldIncomePerHour() 
    {
        
        //US makes $1.7235 per person per hour
        return population * 1.7235f;
    }

    public void IncreaseGoldFor1Hour() 
    {
        gold += GetGoldIncomePerHour();


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



        output += "\n";

        return output;
    }


   
}
