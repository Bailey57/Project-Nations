using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClick : MonoBehaviour
{
    [SerializeField] public GameObject selectedObject;
    [SerializeField] private Camera cam;

    //[SerializeField] public LayerMask clickable;
    //[SerializeField] public LayerMask ground;


    public void SelectObject()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (Input.GetMouseButtonDown(0) && selectedObject == null)
        {
            if (rayHit.transform != null && rayHit.transform.gameObject != null && rayHit.transform.gameObject.layer == 5)
            {

                return;

            }




            //if (rayHit.transform != null && rayHit.transform.gameObject != null && (((rayHit.transform.gameObject.GetComponent("LandSquare") as LandSquare) && !(rayHit.collider is BoxCollider2D))))
            if (rayHit.transform != null && rayHit.transform.gameObject != null && (((rayHit.transform.gameObject.GetComponent("LandSquare") as LandSquare))))
            {


                Debug.Log("Hit " + rayHit.transform.gameObject.name);
                selectedObject = rayHit.transform.gameObject;


            }
            else
            {
                Debug.Log("Hit Nothing");
                selectedObject = null;



            }


        }
        else if (Input.GetMouseButtonDown(1))
        {
            selectedObject = null;
            Debug.Log("unselected");

        }
    }




    public void FollowSelected()
    {
        if (selectedObject != null)
        {
            cam.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, cam.transform.position.z);


        }


    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }


    // Update is called once per frame
    void Update()
    {
        SelectObject();
        //FollowSelected();




    }

}