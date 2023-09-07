using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NationActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject map;


    //need enough money and influence in the area to annex
    public void AnnexLandSquare(GameObject nation, GameObject landsquare)
    {
        if (nation.GetComponent<Nation>().gold >= landsquare.GetComponent<LandSquare>().CalculateLandValue()) 
        {
            nation.GetComponent<Nation>().gold -= landsquare.GetComponent<LandSquare>().CalculateLandValue();
            landsquare.GetComponent<LandSquare>().factionOwner = nation.GetComponent<Nation>().nationName;
            nation.GetComponent<Nation>().ownedLandSquares.Add(landsquare);
            map.GetComponent<Map>().UpdateBorders();
        }
        
    }



    public void Player1AnnexLandSquare()
    {

        if (map.GetComponent<Map>().players[0].GetComponentInChildren<ObjectClick>().selectedObject != null)
        //GetComponent<Camera>().GetComponent<ObjectClick>().selectedObject != null) 
        {
            AnnexLandSquare(map.GetComponent<Map>().players[0], map.GetComponent<Map>().players[0].GetComponentInChildren<ObjectClick>().selectedObject);
        }
        
        
        //AnnexLandSquare();
    }
}
