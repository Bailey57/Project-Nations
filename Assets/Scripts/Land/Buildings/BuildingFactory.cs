using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //"NewMine", 1, 


    public Mine BuildMine(string Name, int CooldownDays, float ProductionInKG)
    {
        Mine newMine = new Mine(Name, CooldownDays, ProductionInKG);

        return newMine;
    }


    public Mine BuildMine_lvl1() 
    {

        return BuildMine("NewMine", 1, 100);
    }

}
