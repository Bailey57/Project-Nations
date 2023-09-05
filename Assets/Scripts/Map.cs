using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        players.Add(GameObject.Find("Player1"));
        MakeMap1();
        MakeNation1();
        StartCoroutine(PassTime());
        //DrawMap();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator PassTime()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(1);


            (players[0].GetComponent(typeof(Nation)) as Nation).population += 55;
        }
    }




    private int squareLength = 5;

    public List<GameObject> players = new List<GameObject>();

    public List<GameObject> worldLandSquares = new List<GameObject>();


    public void MakeMap1() 
    {
    
        for (int x = 0; x < 30; x++) 
        {
            for (int y = 0; y < 30; y++) 
            {
                //LandSquare newLandSquare = new LandSquare();
  
                GameObject newLandSquare = (GameObject)Instantiate(Resources.Load("Prefabs/GrassTile_50x50"));

                int randInt = Random.Range(0, 100);
                if (randInt <= 1)
                {
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "lake";
                }
                else if (randInt >= 15 && randInt <= 30)
                {
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "forest";
                }
                else 
                {
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "grass_fields";
                }
                

                // GetSpriteForLand()
                (newLandSquare.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).sprite = GetSpriteForLand((newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type);

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

    public void MakeNation1() 
    {

        (players[0].GetComponent(typeof(Nation)) as Nation).population = 1000000;
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[0]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[1]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[30]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[31]);
    }


    public Sprite GetSpriteForLand(string type) 
    {
        Sprite sprite = null;
        if (type == "grass_fields") 
        {
            sprite = Resources.Load<Sprite>("Sprites/Land/GrassTile_50x50");
            
        }
        else if (type == "wheat_fields")
        {

        }
        else if (type == "forest")
        {
            sprite = Resources.Load<Sprite>("Sprites/Land/TreeTile_50x50");

        }
        else if (type == "ocean")
        {

        }
        else if (type == "lake")
        {
            sprite = Resources.Load<Sprite>("Sprites/Land/LakeTile_50x50");
        }
        Debug.Log(sprite);

        return sprite;

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
