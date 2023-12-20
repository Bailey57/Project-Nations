using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SteelPlant : Building
{


    public SteelPlant(string Name, bool IsActive, float HoursToBuild, float MaintenanceCostPerHour, float MaxProductionInKGPerHour)
    {
        
        this.Name = Name;
        this.IsActive = IsActive;
        this.HoursToBuild = HoursToBuild;
        this.MaxProductionInKGPerHour = MaxProductionInKGPerHour;
        this.MaintenanceCostPerHour = MaintenanceCostPerHour;

        this.IronNeededPerHour = MaxProductionInKGPerHour * 1.6f;
    }

    public string Name { get; set; }
    public bool IsActive { get; set; }

    public float HoursToBuild { get; set; }
    public float ProductionInKGPerHour { get; set; }

    public float MaxProductionInKGPerHour { get; set; }

    public float MaintenanceCostPerHour { get; set; }

    public float Efficency { get; set; }

    public string FactionOwner { get; set; }

    public float IronNeededPerHour { get; set; }


    public void ConvertIronOreToSteel(GameObject map) 
    {
        float metricTonsOfIronOre = map.GetComponent<Map>().nations[FactionOwner].GetComponent<Nation>().metricTonsOfIronOre;
        ProductionInKGPerHour = MaxProductionInKGPerHour * Efficency * .01f;
        if (IsActive && FactionOwner != "" && metricTonsOfIronOre >= IronNeededPerHour * .001f)
        {
            //IsActive = true;
            
            map.GetComponent<Map>().nations[FactionOwner].GetComponent<Nation>().metricTonsOfSteel += ProductionInKGPerHour * .001f;
            map.GetComponent<Map>().nations[FactionOwner].GetComponent<Nation>().metricTonsOfIronOre -= IronNeededPerHour * .001f;



        }
        else
        {
            //IsActive = false;


        }
        
    
    }

    /*
    public string BuildingToString() 
    {
        string output = "";
        output += "\nBuilding: " + "Mine";
        output += "\nProductionInKGPerHour: " + Math.Round((double)(ProductionInKGPerHour));
        output += "\nMaintenanceCostPerHour: " + Math.Round((double)(MaintenanceCostPerHour));
        output += "\nEfficency: " + Math.Round((double)(Efficency)) + "%";

        output += "\nIsActive: " + IsActive;
        if (HoursToBuild > 0) 
        {
            output += "\nHoursTillBuilt: " + HoursToBuild;
        }


        output += "\n";
        return output;
    }
    */


}
