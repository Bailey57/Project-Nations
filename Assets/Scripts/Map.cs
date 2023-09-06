using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Map : MonoBehaviour
{






    private int squareLength = 5;
    private int hourPerTick = 1;



    public List<GameObject> players = new List<GameObject>();

    public List<GameObject> nations = new List<GameObject>();

    //public List<GameObject> worldLandSquares = new List<GameObject>();

    //public List<List<GameObject>> worldLandSquares = new List<List<GameObject>>();
    public static int worldSize = 100;
    public GameObject[,] worldLandSquares = new GameObject[worldSize, worldSize];
    public string mapStr = "";


    // Start is called before the first frame update
    void Start()
    {
        players.Add(GameObject.Find("Player1"));
        MakeMap1();
        //GetMapStr();
        MakeNation1();
        StartCoroutine(PassTime());

        //DrawMap();


        //UpdateNationResources();
    }

    //

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetMapStr() 
    {
        string output = "";
        for (int x = 0; x < worldLandSquares.GetLength(0); x++)
        {
            for (int y = 0; y < worldLandSquares.GetLength(1); y++)
            {
                //Debug.Log("MapStr");
                output += "y";
            }
            output += "x \n";
        }    


        this.mapStr = output;
        return output;
    }

    public IEnumerator PassTime()
    {
        
        while (true)
        {

            yield return new WaitForSeconds(hourPerTick);
            UpdateTiles();
            UpdateNationResources();

            for (int i = 0; i < nations.Count; i++) 
            {
            }

            //(players[0].GetComponent(typeof(Nation)) as Nation).population += 55;
        }
    }


    public void UpdateTiles() 
    {
        for (int x = 0; x < worldLandSquares.GetLength(0); x++) 
        {
            for (int y = 0; y < worldLandSquares.GetLength(1); y++) 
            {
                if (worldLandSquares[x, y] != null) 
                {
                    (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).population += (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).population * hourPerTick * 0.00000134077f;

                }
                
            }
            


        }
    }

    public void UpdateNationResources() 
    {
       

        for (int i = 0; i < nations.Count; i++)
        {
            //float newPop = 0;
            //float newIron = 0;

            //float newGold = 0;
            //float newWood = 0;
            //float newWater = 0;
            //float newFuel = 0;

            for (int k = 0; k < (nations[i].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Count; k++) 
            {
                //(nations[i].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].gameObject.GetComponent<LandSquare>().population += (nations[i].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].gameObject.GetComponent<LandSquare>().population * hourPerTick * 0.00000134077f;

                //Debug.Log("owns " + k + " land squares" + (nations[i].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].name);
                (nations[i].GetComponent(typeof(Nation)) as Nation).GetAndSetPopulation();
            }

            //(nations[i].GetComponent(typeof(Nation)) as Nation).population += newPop;



            //(nations[i].GetComponent(typeof(Nation)) as Nation).population +=


        }

    }




    public void MakeMap1() 
    {
    
        for (int x = 0; x < worldSize; x++) 
        {
            for (int y = 0; y < worldSize; y++) 
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

                //rand pop count
                randInt = Random.Range(0, 4000);
                (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).population = randInt;

                // GetSpriteForLand()
                (newLandSquare.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).sprite = GetSpriteForLand((newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type);

                //newLandSquare.AddComponent<LandSquare>();
                (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).x = x;
                (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).y = y;
                /*
                if (x >= worldLandSquares.Count)
                {
                    List<GameObject> newXList = new List<GameObject>();
                    worldLandSquares.Add(newXList);
                }
                */
                //Debug.Log("Before: " + worldLandSquares[x]);
                //worldLandSquares[x].Append(newLandSquare);
                worldLandSquares[x, y] = newLandSquare;
                //worldLandSquares[x][y] = newLandSquare;
                //worldLandSquares.Add(newLandSquare);
                //Debug.Log("X: " + x + " Y: " + y + " Square: " + worldLandSquares[x]);

                int newLandSquarex = (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).x;
                int newLandSquarey = (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).y;
                //newObject.transform.position = new Vector3(x * squareLength, y * squareLength, 0);
                newLandSquare.transform.position = new Vector3(newLandSquarex * squareLength, newLandSquarey * squareLength, 0);
            }
        }
    }

    public void MakeNation1() 
    {

        //(players[0].GetComponent(typeof(Nation)) as Nation).population = 1000000;
        (players[0].GetComponent(typeof(Nation)) as Nation).nationName = "Nation1";
        Debug.Log("X count: " + worldLandSquares.GetLength(0) + "Y count: " + worldLandSquares.GetLength(1) + " ");
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[0,0]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[0].GetComponent<LandSquare>().population += 40000;
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[0].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[1,0]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[0,1]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[1,1]);

        nations.Add(players[0]);
    }


    public void MakeRandomStartingNation(int ownSquareCount) 
    {

    
    
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
        //Debug.Log(sprite);

        return sprite;

    }



    public void DrawMap()
    {

        for (int x = 0; x < worldLandSquares.GetLength(0); x++)
        {
            for (int y = 0; y < worldLandSquares.GetLength(1); y++) 
            {
                SpawnTile(worldLandSquares[x,y]);
            }
            
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
