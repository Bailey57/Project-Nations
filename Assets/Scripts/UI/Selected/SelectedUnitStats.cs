using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedUnitStats : MonoBehaviour
{


    [SerializeField] public TMP_Text textMeshTxt;
    [SerializeField] public GameObject objectClick;
    //[SerializeField] public GameObject nation;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (objectClick != null && objectClick.selectedObject != null && objectClick.selectedObject.GetComponent<LandSquare>() != null)
        if (objectClick != null && objectClick.GetComponent<ObjectClick>() != null && objectClick.GetComponent<ObjectClick>().selectedObject != null && objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent<Unit>() != null)
        {

            //textMeshTxt.text = (objectClick.selectedObject.GetComponent("LandSquare") as LandSquare).LandSquareToString();
            textMeshTxt.text = objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent<Unit>().UnitToString();



        }
        else
        {
            textMeshTxt.text = "";
        }
    }






}
