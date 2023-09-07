using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSquare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public int x;
    public int y;

    public string factionOwner;
    public string type;//forest, grass_fields, wheat_fields, ocean, forest, lake

    //1 = max 
    public float ironAvalibility;
    public float fertility;



    public float population;


    /**
     * Based on buildings, population, infastructure ect...
     */
    public float CalculateLandValue() 
    {
        float value = 5000000;//5mil base value

        //add based on pop
        value += population * 1000;
        //add based on buildings
        //add based on infastructure
        return value;
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

        return output;
    }


}
