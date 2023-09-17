using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSquare : MonoBehaviour
{

    void Start()
    {
        //TODO: update everything there needs to be updated ex: population increase, building production

    }

    public int x;
    public int y;

    public string factionOwner;
    public string type;//forest, grass_fields, wheat_fields, ocean, forest, lake

    //1 = max 
    public float ironAvalibility;
    public float waterAvalibility;
    public float oilAvalibility;
    public float lumberAvalibility;
    public float fertility;

    //public float infrastuctureLevel;

    public float population;

    public Dictionary<string, NationApprovalRatings> nationApprovalRatings = new Dictionary<string, NationApprovalRatings>();
    //public List<NationApprovalRatings> nationApprovalRatings = new List<NationApprovalRatings>();

    public List<Building> buildings = new List<Building>();

    public LandSquare() 
    {
    }


    public LandSquare(float ironAvalibility, float waterAvalibility, float oilAvalibility, float lumberAvalibility, float fertility) 
    {
        this.ironAvalibility = ironAvalibility;
        this.waterAvalibility = waterAvalibility;   
        this.oilAvalibility = oilAvalibility;
        this.lumberAvalibility = lumberAvalibility;
        this.fertility = fertility;
    }


    public void SetLandSquareResources(float ironAvalibility, float waterAvalibility, float oilAvalibility, float lumberAvalibility, float fertility) 
    {
        this.ironAvalibility = ironAvalibility;
        this.waterAvalibility = waterAvalibility;
        this.oilAvalibility = oilAvalibility;
        this.lumberAvalibility = lumberAvalibility;
        this.fertility = fertility;

    }



    


    /**
     * Based on buildings, population, infastructure ect...
     */
    public float CalculateLandValue() 
    {
        float value = 5000000;//5mil base value
        float resourceWorth = 1000000;
        //add based on pop
        value += population * 1000;
        value += ironAvalibility * resourceWorth;
        value += waterAvalibility * resourceWorth;
        value += oilAvalibility * resourceWorth;
        value += lumberAvalibility * resourceWorth;
        value += fertility * resourceWorth;
        //add based on buildings
        //add based on infastructure
        return value;
    }


    public string GetApprovalRatingsString(GameObject nation) 
    {
        return nationApprovalRatings[nation.GetComponent<Nation>().nationName].NationApprovalRatingsToString();
    }


    public string BuildingsToString() 
    {
        string output = "";
        for (int i = 0; i < this.buildings.Count; i++)
        {
            output += this.buildings[i].BuildingToString();
            output += "\n";
        }
        
        return output;
    }

    public string LandSquareToString() 
    {
        string output = "";
        output += "x: " + x + " y: " + y;
        if (factionOwner != "")
        {
            output += "\nFactionOwner: " + factionOwner;
        }
        else 
        {
            output += "\nFactionOwner: " + "none";
        }

        


        if (population > 1000000)
        {
            output += "\nPopulation: " + Math.Round((double)(CalculateLandValue() / 1000000), 3) + " million";
          
        }
        else
        {
            output += "\nPopulation: " + Math.Round(population);
        }


        if (CalculateLandValue() > 1000000)
        {
            output += "\nLand Value: " + Math.Round((double)(CalculateLandValue() / 1000000), 3) + " million";
         
        }
        else
        {
            output += "\nLand Value: " + CalculateLandValue();
        }


        if (CalculateLandValue() > 1000000)
        {
            output += "\nLand Annex Cost: " + Math.Round((double)(CalculateLandValue() / 1000000), 3) + " million";

        }
        else
        {
            output += "\nLand Annex Cost: " + CalculateLandValue();
        }
        //output += "\nValue: " ;

        output += "\n";
        output += "\nIronAvalibility: " + ironAvalibility;
        output += "\nWaterAvalibility: " + waterAvalibility;
        output += "\nOilAvalibility: " + oilAvalibility;
        output += "\nLumberAvalibility: " + lumberAvalibility;
        output += "\nFertility: " + fertility;

        output += "\n";
        output += "\nBuildings: ";
        for (int i = 0; i < buildings.Count; i++) 
        {
            output += buildings[i].GetType() + ", ";
        }

        output += "\n";
        return output;
    }


}
