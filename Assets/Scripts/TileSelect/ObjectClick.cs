using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClick : MonoBehaviour
{
    [SerializeField] public GameObject selectedObject;
    [SerializeField] private Camera cam;

    public GameObject nation;



    public GameObject selectionIndicator;

    //[SerializeField] public LayerMask clickable;
    //[SerializeField] public LayerMask ground;


    public void SelectObject()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));


        if (Input.GetMouseButtonDown(0) && selectedObject != null)
        {

            //if hit landSquare after targeting unit, add it to move order
            if (rayHit.transform != null && selectedObject.GetComponent<Unit>() && rayHit.transform.gameObject.GetComponent<LandSquare>() && selectedObject.GetComponent<Unit>().nation == nation)
            {
                Debug.Log(selectedObject.GetComponent<Unit>().unitName);

                StartCoroutine(selectedObject.GetComponent<Unit>().MoveOrders(rayHit.transform.gameObject.GetComponent<LandSquare>().x, rayHit.transform.gameObject.GetComponent<LandSquare>().y));


            }
        }

            



        if (Input.GetMouseButtonDown(0) && selectedObject == null)
        {
            if (rayHit.transform != null && rayHit.transform.gameObject != null && rayHit.transform.gameObject.layer == 5)
            {

                return;

            }


            
         



            //if (rayHit.transform != null && rayHit.transform.gameObject != null && (((rayHit.transform.gameObject.GetComponent("LandSquare") as LandSquare) && !(rayHit.collider is BoxCollider2D))))
            if (rayHit.transform != null && rayHit.transform.gameObject != null && ((((rayHit.transform.gameObject.GetComponent("LandSquare") as LandSquare))) || rayHit.transform.gameObject.GetComponent<Unit>()))
            {


                



                Debug.Log("Hit " + rayHit.transform.gameObject.name);
                selectedObject = rayHit.transform.gameObject;
                selectionIndicator = (GameObject)Instantiate(Resources.Load("Prefabs/UI/DownArrowGreen"));
                selectionIndicator.transform.position = selectedObject.transform.position;
            }
            else
            {
                Debug.Log("Hit Nothing");
                selectedObject = null;
                Destroy(selectionIndicator);
            }


        }
        else if (Input.GetMouseButtonDown(1))
        {
            selectedObject = null;
            Destroy(selectionIndicator);
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