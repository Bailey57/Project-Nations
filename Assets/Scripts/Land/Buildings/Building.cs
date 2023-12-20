using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Building
{
    string Name { get; set; }

    bool IsActive { get; set; }

    float HoursToBuild { get; set; }

    float ProductionInKGPerHour { get; set; }

    float MaxProductionInKGPerHour { get; set; }

    float MaintenanceCostPerHour { get; set; }

    float Efficency { get; set; }//based on land square stats and negative/positive approval %

    string FactionOwner { get; set; }

    string BuildingToString()
    {
        string output = "";
        output += "\nBuilding: " + Name;
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

    /**
     * positiveApprovalRating can add 10%, negative can take away 100% 
     */
    public void SetEfficencyAndProductionRate(float negativeApprovalRatig, float positiveApprovalRating, float resourceAvalibility) 
    {
        Efficency = (negativeApprovalRatig * -1 * resourceAvalibility) + (positiveApprovalRating * .1f * resourceAvalibility) + (resourceAvalibility * 100);
        ProductionInKGPerHour = MaxProductionInKGPerHour * Efficency;
    }
}
