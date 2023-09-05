using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MakeMap1();
        //DrawMap();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int squareLength = 5;



    public List<GameObject> worldLandSquares = new List<GameObject>();


    public void MakeMap1() 
    {
    
        for (int x = 0; x < 30; x++) 
        {
            for (int y = 0; y < 30; y++) 
            {
                //LandSquare newLandSquare = new LandSquare();
  
                GameObject newLandSquare = (GameObject)Instantiate(Resources.Load("Prefabs/GrassTile_50x50"));

                //newLandSquare.AddComponent<LandSquare>();
                (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).x = x;
                (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).y = y;
                worldLandSquares.Add(newLandSquare);

                int newLandSquarex = (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).x;
                int newLandSquarey = (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).y;
                //newObject.transform.position = new Vector3(x * squareLength, y * squareLength, 0);
                newLandSquare.transform.position = new Vector3(newLandSquarex * squareLength, newLandSquarey * squareLength, 0);
            }
        }
    }

    public void DrawMap()
    {

        for (int i = 0; i < worldLandSquares.Count; i++)
        {
            SpawnTile(worldLandSquares[i]);
        }


    }

    public void SpawnTile(GameObject landSquare) 
    {
        //GameObject newObject = (GameObject)Instantiate(Resources.Load("Prefabs/GrassTile_50x50"));
        //landSquare = (GameObject)Instantiate(Resources.Load("Prefabs/GrassTile_50x50"));

        //landSquare.AddComponent<SpriteRenderer>();
        //(landSquare.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).sprite = Resources.Load<Sprite>("Prefabs/GrassTile_50x50");

        int x = (landSquare.GetComponent(typeof(LandSquare)) as LandSquare).x;
        int y = (landSquare.GetComponent(typeof(LandSquare)) as LandSquare).y;
        //newObject.transform.position = new Vector3(x * squareLength, y * squareLength, 0);
        landSquare.transform.position = new Vector3(x * squareLength, y * squareLength, 0);
        //landSquare = newObject;
        //Destroy( newObject );
    }


}
