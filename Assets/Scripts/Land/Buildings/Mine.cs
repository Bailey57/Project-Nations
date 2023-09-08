using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
   

    public Mine(string Name, bool IsActive, float HoursToBuild, float MaintenanceCostPerHour, float ProductionInKGPerHour) 
    {
        this.Name = Name;
        this.IsActive = IsActive;
        this.HoursToBuild = HoursToBuild;
        this.ProductionInKGPerHour = ProductionInKGPerHour;
        this.MaintenanceCostPerHour = MaintenanceCostPerHour;


    }

    public string Name { get; set; }
    public bool IsActive { get; set; }

    public float HoursToBuild { get; set; }
    public float ProductionInKGPerHour { get; set; }

    public float MaintenanceCostPerHour { get; set; }


    public string BuildingToString() 
    {
        string output = "";
        output += "\nBuilding: " + "Mine";
        output += "\nProductionInKGPerHour: " + ProductionInKGPerHour;
        output += "\nMaintenanceCostPerHour: " + MaintenanceCostPerHour;

        output += "\nIsActive: " + IsActive;
        if (HoursToBuild > 0) 
        {
            output += "\nHoursTillBuilt: " + HoursToBuild;
        }


        output += "\n";
        return output;
    }


}
