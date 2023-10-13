using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedTileStats : MonoBehaviour
{


    [SerializeField] public TMP_Text textMeshTxt;
    [SerializeField] public GameObject objectClick;
    [SerializeField] public GameObject nation;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        //Debug.Log("objectClick null: " + objectClick);
        //Debug.Log("2: " + objectClick.selectedObject );
        //Debug.Log("3: " + objectClick == null);
        //Debug.Log("4: " + textMeshTxt == null);
        
        //if (objectClick != null && objectClick.selectedObject != null && objectClick.selectedObject.GetComponent<LandSquare>() != null)
        if (objectClick != null && objectClick.GetComponent<ObjectClick>() != null && objectClick.GetComponent<ObjectClick>().selectedObject != null && objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent<LandSquare>() != null && nation.GetComponent<Nation>().capitalLandSquare != null)
        {

            //textMeshTxt.text = (objectClick.selectedObject.GetComponent("LandSquare") as LandSquare).LandSquareToString();
            textMeshTxt.text = (objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).LandSquareToString(nation) + (objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).BuildingsToString();



        }
        else 
        {
            textMeshTxt.text = "";
        }
    }






}
