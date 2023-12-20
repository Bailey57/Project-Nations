using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Mine : Building
{
   

    public Mine(string Name, bool IsActive, float HoursToBuild, float MaintenanceCostPerHour, float MaxProductionInKGPerHour) 
    {
        this.Name = Name;
        this.IsActive = IsActive;
        this.HoursToBuild = HoursToBuild;
        this.MaxProductionInKGPerHour = MaxProductionInKGPerHour;
        this.MaintenanceCostPerHour = MaintenanceCostPerHour;


    }

    public string Name { get; set; }
    public bool IsActive { get; set; }

    public float HoursToBuild { get; set; }
    public float ProductionInKGPerHour { get; set; }

    public float MaxProductionInKGPerHour { get; set; }

    public float MaintenanceCostPerHour { get; set; }

    public float Efficency { get; set; }

    public string FactionOwner { get; set; }

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
