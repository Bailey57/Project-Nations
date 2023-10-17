using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LandSquare : MonoBehaviour
{

    void Start()
    {
        //TODO: update everything there needs to be updated ex: population increase, building production

    }

    public int x;
    public int y;

    public string factionOwner = "";
    public string type;//forest, grass_fields, wheat_fields, ocean, forest, lake

    //1 = max 
    public float ironAvalibility;
    public float waterAvalibility;
    public float oilAvalibility;
    public float lumberAvalibility;
    public float fertility;

    //public float infrastuctureLevel;

    public float population;

    //military
    public float populationPercentageInMilitary;//
    public float populationAmmountInMilitary;//

    //
    public float infrastructureLevel;

    public GameObject map;

    public Dictionary<string, NationApprovalRatings> nationApprovalRatings = new Dictionary<string, NationApprovalRatings>();
    //public List<NationApprovalRatings> nationApprovalRatings = new List<NationApprovalRatings>();

    public List<Building> buildings = new List<Building>();

    public List<GameObject> units = new List<GameObject>();

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
        float value = 1000000;//5000000, 5mil base value
        float resourceWorth = 300000;
        //add based on pop
        value += population * 10;
        value += ironAvalibility * resourceWorth;
        value += waterAvalibility * resourceWorth;
        value += oilAvalibility * resourceWorth;
        value += lumberAvalibility * resourceWorth;
        value += fertility * resourceWorth;
        //add based on buildings
        //add based on infastructure
        return value;
    }

    /**
     * Based on buildings, population, infrastructure ect...
     */
    public float CalculateAnnexCost(GameObject nation)
    {
        float priceIncrease = .4f;//1 = 100%

        //TODO: make more expensive the further away and if no infrastructure on the way from a capitol
        int capitalX = nation.GetComponent<Nation>().capitalLandSquare.GetComponent<LandSquare>().x;
        int capitalY = nation.GetComponent<Nation>().capitalLandSquare.GetComponent<LandSquare>().y;

        float distance = nation.GetComponent<Nation>().GetDistanceFromCapitol(this.gameObject);

        priceIncrease *= distance;


        float value = CalculateLandValue();
       

        value += value * priceIncrease * priceIncrease;
        return value;
    }



    public bool IsBorderWithNation(GameObject nation)
    {
       
        int xPosTmp;
        int yPosTmp;

        
        xPosTmp = this.x + 1;
        yPosTmp = this.y + 1;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }
        xPosTmp = this.x - 1;
        yPosTmp = this.y - 1;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }
        xPosTmp = this.x + 1;
        yPosTmp = this.y - 1;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }
        xPosTmp = this.x - 1;
        yPosTmp = this.y + 1;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }
        xPosTmp = this.x + 1;
        yPosTmp = this.y;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }
        xPosTmp = this.x - 1;
        yPosTmp = this.y;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }
        xPosTmp = this.x;
        yPosTmp = this.y + 1;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }
        xPosTmp = this.x;
        yPosTmp = this.y - 1;
        if (xPosTmp >= 0 && xPosTmp < map.GetComponent<Map>().worldSize && yPosTmp >= 0 && yPosTmp < map.GetComponent<Map>().worldSize)
        {

            if (map.GetComponent<Map>().worldLandSquares[xPosTmp, yPosTmp].GetComponent<LandSquare>().factionOwner == nation.GetComponent<Nation>().nationName)
            {
                return true;
            }
        }


        return false;
    }

    public string GetApprovalRatingsString(GameObject nation) 
    {
        return nationApprovalRatings[nation.GetComponent<Nation>().nationName].NationApprovalRatingsToString();
    }



    public void IncreasePopulationInLandSquare(double hoursPassed)
    {
        double e = 2.71828;//euler's number
        double r = 1.1; //rate, 1 = 100%
        population *= (float)Math.Pow(e, (r * hoursPassed / 8760));//8760 = hors in a year
        //(worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).population += (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).population * hourPerTick * 0.00000134077f;//old pop equation

    }

    public bool HasMajorCity() 
    {
        for (int i = 0; i < this.buildings.Count; i++) 
        {
            if (buildings[i] is MajorCity) 
            {
                return true;
            }
        
        }

        return false;
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






    public string LandSquareToString(GameObject nation)
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

        float landAnnexCost = CalculateAnnexCost(nation);
        if (landAnnexCost > 1000000)
        {
            output += "\nLand Annex Cost: " + Math.Round((double)(landAnnexCost / 1000000), 3) + " million";

        }
        else
        {
            output += "\nLand Annex Cost: " + landAnnexCost;
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
