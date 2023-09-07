using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory
{
    

    //"NewMine", 1, 


    public Mine BuildMine(string Name, bool IsActive, float HoursToBuild, float ProductionInKGPerHour)
    {
        //mines make 0.523kg to 1.087kg of ore per $
        Mine newMine = new Mine(Name, IsActive, HoursToBuild, ProductionInKGPerHour * 1.087f, ProductionInKGPerHour);

        return newMine;
    }


    public Mine BuildMine_lvl1() 
    {
        return BuildMine("NewMine",false, 24, 100);
    }

}
