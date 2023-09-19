using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory
{
    

    //"NewMine", 1, 


    public Mine BuildMine(string Name, bool IsActive, float HoursToBuild, float MaxProductionInKGPerHour, string factionOwner)
    {
        //mines make 0.523kg to 1.087kg of ore per $
        Mine newMine = new Mine(Name, IsActive, HoursToBuild, MaxProductionInKGPerHour * 1.087f, MaxProductionInKGPerHour);
        newMine.FactionOwner = factionOwner;

        return newMine;
    }


    public Mine BuildMine_lvl1(string factionOwner) 
    {
        return BuildMine("Mine",false, 24, 100, factionOwner);
    }

}
