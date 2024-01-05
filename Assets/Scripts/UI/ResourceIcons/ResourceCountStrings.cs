using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceCountStrings : MonoBehaviour
{


    public GameObject nation;

    public TMP_Text goldText;
    public TMP_Text populationText;
    public TMP_Text metricTonsOfIronOreText;
    public TMP_Text militaryReservesText;
    
    //public TMText textField;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetStrings();
    }

    public void GetStrings() 
    {
        try 
        {
            goldText.text = nation.GetComponent<Nation>().NumberToResourceString("", nation.GetComponent<Nation>().gold);
            populationText.text = nation.gameObject.GetComponent<Nation>().NumberToResourceString("", nation.GetComponent<Nation>().population);
            metricTonsOfIronOreText.text = nation.GetComponent<Nation>().NumberToResourceString("", nation.GetComponent<Nation>().metricTonsOfIronOre);
            militaryReservesText.text = nation.GetComponent<Nation>().NumberToResourceString("", nation.GetComponent<Nation>().military.totalForce);

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    //output += "\nPopulation: " + Math.Round(population);

    //    output += "\n";
    //    output += NumberToResourceString("Gold", gold);

    //output += "\n";
    //    output += NumberToResourceString("GoldIncomePerHour", goldIncomePerHour);

    //output += "\n";
    //    output += NumberToResourceString("GoldExpencesPerHour", goldExpencesPerHour);

    //output += "\n";
    //    output += NumberToResourceString("NetGoldPerHour", goldIncomePerHour - goldExpencesPerHour);

    //output += "\n";
    //    output += NumberToResourceString("MetricTonsOfIronOre", metricTonsOfIronOre);

    //output += "\n";
    //    output += NumberToResourceString("MetricTonsOfSteel", metricTonsOfSteel);

    //output += "\n";
    //    output += NumberToResourceString("Military Reserves", (int) military.totalForce);

}
