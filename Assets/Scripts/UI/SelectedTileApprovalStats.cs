using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedTileApprovalStats : MonoBehaviour
{


    [SerializeField] public TMP_Text textMeshTxt;
    [SerializeField] public GameObject objectClick;
    public GameObject nation;


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
        if (objectClick != null && objectClick.GetComponent<ObjectClick>() != null && objectClick.GetComponent<ObjectClick>().selectedObject != null && objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent<LandSquare>() != null)
        {

            Debug.Log("SelectionNotNull!");
            textMeshTxt.text = (objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).GetApprovalRatingsString(nation);

            /*
            string outputStr = "Count: " + (objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).nationApprovalRatings.Count;
            foreach (var nation in (objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).nationApprovalRatings) 
            {
                Debug.Log("Iterated: " + nation.Key.name);
                outputStr += nation.Value.NationApprovalRatingsToString();
                //(objectClick.GetComponent<ObjectClick>().selectedObject.GetComponent("LandSquare") as LandSquare).nationApprovalRatings[nation].NationApprovalRatingsToString();
            }

            textMeshTxt.text = outputStr;
            */
        }
        else
        {
            textMeshTxt.text = "";
        }
    }






}
