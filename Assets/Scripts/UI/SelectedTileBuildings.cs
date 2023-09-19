using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedTileBuildings : MonoBehaviour
{


    [SerializeField] public TMP_Text textMeshTxt;
    [SerializeField] public GameObject objectClick;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (objectClick != null && objectClick.selectedObject != null && objectClick.selectedObject.GetComponent<LandSquare>() != null)
        if (objectClick != null && objectClick.GetComponent<ObjectClick>() != null && objectClick.GetComponent<ObjectClick>().selectedObject != null && objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent<LandSquare>() != null)
        {

            //textMeshTxt.text = (objectClick.selectedObject.GetComponent("LandSquare") as LandSquare).LandSquareToString();

            textMeshTxt.text = (objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).LandSquareToString();
            string finalString = "";

            finalString = (objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).BuildingsToString();
            textMeshTxt.text = finalString;
            //landsquare buildings


        }
        else
        {
            textMeshTxt.text = "";
        }
    }






}
