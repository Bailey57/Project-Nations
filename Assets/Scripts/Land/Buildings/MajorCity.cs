using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorCity : Building
{
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public float HoursToBuild { get; set; }

    public float ProductionInKGPerHour { get; set; }

    public float MaxProductionInKGPerHour { get; set; }

    public float MaintenanceCostPerHour { get; set; }

    public float Efficency { get; set; }//based on land square stats and negative/positive approval %

    public string FactionOwner { get; set; }


    public MajorCity() { }

    public MajorCity(string name, string factionOwner) 
    {
        this.Name = name;
        this.FactionOwner = factionOwner;

        IsActive = true;
    }



}
