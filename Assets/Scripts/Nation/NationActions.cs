using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NationActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null) 
        {
            mainCamera = new GameObject();
            mainCamera.transform.SetParent(gameObject.transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    BuildingFactory buildingFactory = new BuildingFactory();
    public GameObject mainCamera;//null if npc 
    public GameObject map;


    //need enough money and influence in the area to annex
    public void AnnexLandSquare(GameObject nation, GameObject landsquare)
    {
        if (nation.GetComponent<Nation>().gold >= landsquare.GetComponent<LandSquare>().CalculateLandValue() && nation.GetComponent<Nation>().nationName != landsquare.GetComponent<LandSquare>().factionOwner) 
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

    public void PlayerAnnexLandSquare()
    {
        //gameObject.transform.gets

        if (mainCamera.GetComponent<ObjectClick>().selectedObject != null)
        {
            
            AnnexLandSquare(gameObject.transform.parent.GetComponent<Nation>().gameObject, mainCamera.GetComponent<ObjectClick>().selectedObject);
        }

    }

    public void BuildMine(GameObject landSquare) 
    {
        if (gameObject.transform.parent.GetComponent<Nation>().nationName == landSquare.GetComponent<LandSquare>().factionOwner) 
        {
            float buildCost = landSquare.GetComponent<LandSquare>().ironAvalibility * 5000000;
            if (gameObject.transform.parent.GetComponent<Nation>().gold >= buildCost)
            {
                gameObject.transform.parent.GetComponent<Nation>().gold -= buildCost;
                landSquare.GetComponent<LandSquare>().buildings.Add(buildingFactory.BuildMine_lvl1());
            }
        }

        
        


    }

    public void PlayerBuildMine() 
    {
        //this.transform.parent
        Debug.Log("This: " + gameObject.name);
        Debug.Log("The Parent: " + gameObject.transform.parent.GetComponent<Nation>().nationName);
        if (mainCamera.GetComponent<ObjectClick>().selectedObject != null)
        {

            BuildMine(mainCamera.GetComponent<ObjectClick>().selectedObject);
        }
    }



    public void CalculateBestDecision() 
    {
    }

    public void SelectSqureToAnnex() 
    {
    
    }



}