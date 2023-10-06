using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Military
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject nation;


    public float totalForce = 0;
    public float conscriptionPercentage = 1;//starts out at 1%

    public List<GameObject> units = new List<GameObject>();
    public float EstimateTotalForce() 
    {
        return nation.GetComponent<Nation>().population * conscriptionPercentage;
    }

    /**
     * When drafting troops, takes out conscriptionPercentage of the population of each landSquare
     */
    public float DraftTroops() 
    {
        totalForce = conscriptionPercentage;

        return totalForce;
    }




    
}
