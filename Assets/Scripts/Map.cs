using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEditor.Progress;


public class Map : MonoBehaviour
{

    private int squareLength = 5;
    private int hourPerTick = 1;



    public List<GameObject> players = new List<GameObject>();

    //public List<GameObject> nations = new List<GameObject>();
    public Dictionary<string, GameObject> nations = new Dictionary<string, GameObject>();

    //public List<GameObject> worldLandSquares = new List<GameObject>();

    //public List<List<GameObject>> worldLandSquares = new List<List<GameObject>>();
    public int worldSize;
    public GameObject[,] worldLandSquares;
    public string mapStr = "";

    public bool allSquaresAnnexed;

    // Start is called before the first frame update
    void Start()
    {
        //set this worlds size
        worldSize = 30;//250 max for now 
        allSquaresAnnexed = false;

        worldLandSquares = new GameObject[worldSize, worldSize];
        players.Add(GameObject.Find("Player1"));
        MakeMap1();

        //GetMapStr();
        MakeNation1();


        StartCoroutine(PassTime());
        //StartCoroutine(UpdateTilesCR());
        //StartCoroutine(UpdateNationCR());
        //StartCoroutine(IncreaseNationGoldCRCR());
        //StartCoroutine(UpdateExpencesCR());
        

        for(int i = 0; i < 15; i ++)
        {
            GenerateRandNation();
        }


        UpdateBorders();
        //DrawMap();//old



        //UpdateNation();

        //AddNewSquareTst();
        SetBaseNationApprovalForTiles();
    }




    public void IncreaseNationGold() 
    {
        /*
        for (int i = 0; i < nations.Count; i++) 
        {
            nations[i].GetComponent<Nation>().IncreaseGoldFor1Hour();
        }
        */
        foreach (var item in nations)
        {
            item.Value.GetComponent<Nation>().IncreaseGoldFor1Hour();
            //foo(item.Key);
            //bar(item.Value);
        }
    }


    public void GenerateRandNation() 
    {
        //(players[0].GetComponent(typeof(Nation)) as Nation).population = 1000000;
        GameObject newNation = new GameObject();


        GameObject newNationActions = new GameObject();
        newNationActions.AddComponent<NationActions>();
        newNationActions.transform.parent = newNation.transform;
        newNationActions.GetComponent<NationActions>().nation = newNation;
        newNationActions.GetComponent<NationActions>().map = this.gameObject;

        newNation.AddComponent<Nation>();
        (newNation.GetComponent(typeof(Nation)) as Nation).nationName = "NewNation " + this.nations.Count;

        Debug.Log("X count: " + worldLandSquares.GetLength(0) + "Y count: " + worldLandSquares.GetLength(1) + " ");


        //(newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[0, 0]);
        int randX = Random.Range(0, worldSize - 1);
        int randY = Random.Range(0, worldSize - 1);

        (newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[randX, randY]);
        (newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares[0].GetComponent<LandSquare>().population += 40000;
        (newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares[0].GetComponent<LandSquare>().factionOwner = "NewNation " + this.nations.Count;
        

        /*
        //extra squares
        (newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[randX + 1, randY]);
        (newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares[1].GetComponent<LandSquare>().factionOwner = "NewNation " + this.nations.Count;
        (newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[randX, randY + 1]);
        (newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares[2].GetComponent<LandSquare>().factionOwner = "NewNation " + this.nations.Count;
        */
        //(newNation.GetComponent(typeof(Nation)) as Nation).ownedLandSquares[3].GetComponent<LandSquare>().factionOwner = "NewNation " + this.nations.Count;

        (newNation.GetComponent(typeof(Nation)) as Nation).botControlled = true;
        nations.Add(newNation.GetComponent<Nation>().nationName, newNation);

        newNation.name = (newNation.GetComponent(typeof(Nation)) as Nation).nationName;
        float million = 1000000;
        newNation.GetComponent<Nation>().gold += 9 * million;//start with 6 mil usually 

        //make nation have a rand color
        newNation.GetComponent<Nation>().nationMainColor = Random.ColorHSV();

    }



    public void AddNewSquareTst() 
    {
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[1, 2]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[4].GetComponent<LandSquare>().factionOwner = "Nation1";

        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[2, 3]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[5].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[0, 3]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[6].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[2, 2]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[7].GetComponent<LandSquare>().factionOwner = "Nation1";


        //edge case testing
        /**
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[98, 98]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[8].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[99, 99]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[9].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[0, 99]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[10].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[99, 0]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[11].GetComponent<LandSquare>().factionOwner = "Nation1";
        **/
        UpdateBorders();
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




    
    public IEnumerator UpdateTilesCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(hourPerTick);
            UpdateTiles();
        }
    }
    public IEnumerator UpdateNationCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(hourPerTick);
            UpdateNation();
        }
    }
    public IEnumerator IncreaseNationGoldCRCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(hourPerTick);
            IncreaseNationGold();
        }
    }
    public IEnumerator UpdateExpencesCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(hourPerTick);

            /*
            for (int i = 0; i < nations.Count; i++)
            {
                nations[i].GetComponent<Nation>().GetAndSetGoldExpencesPerHour();
            }
            */

            foreach (var item in nations)
            {
                item.Value.GetComponent<Nation>().GetAndSetGoldExpencesPerHour();
            }

        }
    }

    public IEnumerator PassTime()
    {
        
        while (true)
        {

            yield return new WaitForSeconds(hourPerTick);
            UpdateTiles();
            UpdateNation();
            IncreaseNationGold();
            UpdateExpences();
            //for (int i = 0; i < nations.Count; i++) 
            //{
            //}

            //(players[0].GetComponent(typeof(Nation)) as Nation).population += 55;
        }
    }


    public void UpdateExpences() 
    {
        /*
        for (int i = 0; i < nations.Count; i++)
        {
            nations[i].GetComponent<Nation>().GetAndSetGoldExpencesPerHour();
        }
        */
        foreach (var item in nations)
        {
            item.Value.GetComponent<Nation>().GetAndSetGoldExpencesPerHour();
        }

    }


    /*
    public int GetLandSquaresNationIndex(GameObject landSquare) 
    {
        foreach (var item in nations)
        {
            if (landSquare.GetComponent<LandSquare>().factionOwner == item.Value.GetComponent<Nation>().nationName)
            {
                return i;
            }
        }

        for (int i = 0; i < nations.Count; i ++) 
        {

            if (landSquare.GetComponent<LandSquare>().factionOwner == nations[i].GetComponent<Nation>().nationName) 
            {
                return i;
            }
        }

        return -1;
    }
    */



    public void SetBaseNationApprovalForTiles() 
    {
        for (int x = 0; x < worldLandSquares.GetLength(0); x++)
        {
            for (int y = 0; y < worldLandSquares.GetLength(1); y++)
            {
                if (worldLandSquares[x, y] != null)
                {



                    /*
                    //old nation List
                    for (int i = 0; i < this.nations.Count; i++)
                    {
                        NationApprovalRatings nar = new NationApprovalRatings();
                        (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).nationApprovalRatings.Add(nations[i].GetComponent<Nation>().nationName, nar);

                        //if tile is owned already, then set approval to 80%, else neutral
                        if ((worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).factionOwner != "")
                        {
                            nar.IncreaseApproval(80);

                        } 
                    }
                    */

                    foreach (var item in nations)
                    {

                        NationApprovalRatings nar = new NationApprovalRatings();
                        (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).nationApprovalRatings.Add(nations[item.Key].GetComponent<Nation>().nationName, nar);

                        //if tile is owned already, then set approval to 80%, else neutral
                        if ((worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).factionOwner != "")
                        {
                            nar.IncreaseApproval(80);

                        }
                    }

                }

            }
        }


    }


    


    public void UpdateTiles() 
    {
        bool tempAllSquaresAnnexed = true;
        for (int x = 0; x < worldLandSquares.GetLength(0); x++) 
        {
            for (int y = 0; y < worldLandSquares.GetLength(1); y++) 
            {
                
                if (worldLandSquares[x, y] != null) 
                {
                    
                    worldLandSquares[x, y].GetComponent<LandSquare>().IncreasePopulationInLandSquare(this.hourPerTick);

                    //int nationIdx = GetLandSquaresNationIndex(worldLandSquares[x, y]);

                    //check if annexed
                    if (worldLandSquares[x, y].GetComponent<LandSquare>().factionOwner == "") 
                    {
                        tempAllSquaresAnnexed = false;
                    }
                    for (int i = 0; i < (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings.Count; i++) 
                    {
                        //building production

                        //TODO: make buildings give resources to who owns them instead of who owns the land square
                        //mines
                        if ((worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i] is Mine)
                        {
                            if ((worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].IsActive != false && (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].HoursToBuild <= 0)
                            {
                                float oreCost = (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].MaintenanceCostPerHour;
                                if (nations[worldLandSquares[x, y].GetComponent<LandSquare>().factionOwner].GetComponent<Nation>().gold >= oreCost)
                                {
                                    //(worldLandSquares[x, y].GetComponent<LandSquare>().buildings[i].NationOwner
                                    //(worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].NationOwner

                                    float negativeApprovalInSquare = worldLandSquares[x, y].GetComponent<LandSquare>().nationApprovalRatings[(worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].FactionOwner].negativeApproval;
                                    float positiveApprovalInSquare = worldLandSquares[x, y].GetComponent<LandSquare>().nationApprovalRatings[(worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].FactionOwner].positiveApproval;
                                    float ironAvalability = (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).ironAvalibility;
                                    
                                    worldLandSquares[x, y].GetComponent<LandSquare>().buildings[i].SetEfficencyAndProductionRate(negativeApprovalInSquare, positiveApprovalInSquare, ironAvalability);
                                    //worldLandSquares[x, y].GetComponent<LandSquare>().ironAvalibility * 
                                    //Debug.Log("negativeApprovalInSquare: " + negativeApprovalInSquare + " positiveApprovalInSquare: " + positiveApprovalInSquare + " ironAvalability: " + ironAvalability);

                                    //return how much was gained
                                    nations[worldLandSquares[x, y].GetComponent<LandSquare>().factionOwner].GetComponent<Nation>().metricTonsOfIronOre += worldLandSquares[x, y].GetComponent<LandSquare>().buildings[i].ProductionInKGPerHour / 1000;//convert kg to metric tons
                                    
                                    nations[worldLandSquares[x, y].GetComponent<LandSquare>().factionOwner].GetComponent<Nation>().gold -= oreCost;

                                }
                            }
                            else if ((worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].HoursToBuild >= 0) 
                            {
                                (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].HoursToBuild -= hourPerTick;

                                if ((worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].HoursToBuild <= 0) 
                                {
                                    (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].IsActive = true;
                                }


                            }

                            //Debug.Log("hours left till mine built: " + (worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i].HoursToBuild);


                        }
                        //farms
                        if ((worldLandSquares[x, y].GetComponent(typeof(LandSquare)) as LandSquare).buildings[i] is Farm)
                        {

                        }
                    }


                    

                    
                }
                
            }
            


        }
        this.allSquaresAnnexed = tempAllSquaresAnnexed;
    }



    public void UpdateBorders() 
    {

        //for (int i = 0; i < nations.Count; i++)
        foreach (var item in nations)
        {


            for (int k = 0; k < (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Count; k++)
            {
                Debug.Log("landsquare: " + k);
                //(nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].gameObject.GetComponent<LandSquare>().
                int landSquareX = (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().x;
                int landSquareY = (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().y;
                string landSquareFaction = (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().factionOwner;


                //clear current borders
                foreach (Transform child in (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().transform)
                {
                    if (child.name.Contains("TileBorder"))
                    {
                        Destroy(child.gameObject);
                    }

                }

                //if border, then add border, else removes border drawing
                Debug.Log("+x: " + (landSquareX + 1 >= worldLandSquares.GetLength(0)));
                //+x
                if ((landSquareX + 1 >= worldLandSquares.GetLength(0)) || (this.worldLandSquares[landSquareX + 1, landSquareY] == null) || this.worldLandSquares[landSquareX + 1, landSquareY].GetComponent<LandSquare>().factionOwner != landSquareFaction)
                {
                    if ((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>())
                    {
                        GameObject newLandSquareBorderR = (GameObject)Instantiate(Resources.Load("Prefabs/Borders/TileBorderWhite_Right_50x50"));
                        newLandSquareBorderR.transform.position = new Vector3(landSquareX * squareLength, landSquareY * squareLength, 0);
                        newLandSquareBorderR.transform.SetParent((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().transform);
                        newLandSquareBorderR.GetComponent<SpriteRenderer>().color = nations[item.Key].GetComponent<Nation>().nationMainColor;

                    }

                }
                //Debug.Log("landSquareX: " + landSquareX + " landSquareY: " + landSquareY + " maxX: " + worldLandSquares.GetLength(0) + " maxY: " + worldLandSquares.GetLength(1) + " " + (landSquareX - 1 > worldLandSquares.GetLength(0) || landSquareX - 1 < 0));
                Debug.Log("-x: " + ((landSquareX - 1 > worldLandSquares.GetLength(0) || landSquareX - 1 < 0) || (this.worldLandSquares[landSquareX - 1, landSquareY] == null) || (this.worldLandSquares[landSquareX - 1, landSquareY].GetComponent<LandSquare>().factionOwner != landSquareFaction)));
                //-x
                if ((landSquareX - 1 > worldLandSquares.GetLength(0) || landSquareX - 1 < 0) || (this.worldLandSquares[landSquareX - 1, landSquareY] == null) || (this.worldLandSquares[landSquareX - 1, landSquareY].GetComponent<LandSquare>().factionOwner != landSquareFaction))
                {
                    if ((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>())
                    {
                        GameObject newLandSquareBorderR = (GameObject)Instantiate(Resources.Load("Prefabs/Borders/TileBorderWhite_Left_50x50"));
                        newLandSquareBorderR.transform.position = new Vector3(landSquareX * squareLength, landSquareY * squareLength, 0);
                        newLandSquareBorderR.transform.SetParent((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().transform);
                        newLandSquareBorderR.GetComponent<SpriteRenderer>().color = nations[item.Key].GetComponent<Nation>().nationMainColor;
                        
                    }

                }
                Debug.Log("+y: " + (!(landSquareY + 1 < worldLandSquares.GetLength(1)) || !(this.worldLandSquares[landSquareX, landSquareY + 1] != null || this.worldLandSquares[landSquareX, landSquareY + 1].GetComponent<LandSquare>().factionOwner == landSquareFaction)));
                //+y
                if ((landSquareY + 1 >= worldLandSquares.GetLength(1)) || (this.worldLandSquares[landSquareX, landSquareY + 1] == null) || this.worldLandSquares[landSquareX, landSquareY + 1].GetComponent<LandSquare>().factionOwner != landSquareFaction)
                {
                    if ((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>())
                    {
                        GameObject newLandSquareBorderR = (GameObject)Instantiate(Resources.Load("Prefabs/Borders/TileBorderWhite_Top_50x50"));
                        newLandSquareBorderR.transform.position = new Vector3(landSquareX * squareLength, landSquareY * squareLength, 0);
                        newLandSquareBorderR.transform.SetParent((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().transform);
                        newLandSquareBorderR.GetComponent<SpriteRenderer>().color = nations[item.Key].GetComponent<Nation>().nationMainColor;

                    }

                }
                Debug.Log("-y: " + ((landSquareY - 1 > worldLandSquares.GetLength(1) || landSquareY - 1 < 0) || (this.worldLandSquares[landSquareX, landSquareY - 1] == null) || (this.worldLandSquares[landSquareX, landSquareY - 1].GetComponent<LandSquare>().factionOwner != landSquareFaction)));
                //-y
                if ((landSquareY - 1 > worldLandSquares.GetLength(1) || landSquareY - 1 < 0) || (this.worldLandSquares[landSquareX, landSquareY - 1] == null) || (this.worldLandSquares[landSquareX, landSquareY - 1].GetComponent<LandSquare>().factionOwner != landSquareFaction))
                {
                    if ((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>())
                    {
                        GameObject newLandSquareBorderR = (GameObject)Instantiate(Resources.Load("Prefabs/Borders/TileBorderWhite_Bottom_50x50"));
                        newLandSquareBorderR.transform.position = new Vector3(landSquareX * squareLength, landSquareY * squareLength, 0);
                        newLandSquareBorderR.transform.SetParent((nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].GetComponent<LandSquare>().transform);
                        newLandSquareBorderR.GetComponent<SpriteRenderer>().color = nations[item.Key].GetComponent<Nation>().nationMainColor;

                    }

                }






                //(nations[item.Key].GetComponent(typeof(Nation)) as Nation).GetAndSetPopulation();



            }
        }
    }

    public void UpdateNation() 
    {


        //for (int i = 0; i < nations.Count; i++)
        foreach (var item in nations)
        {
            //float newPop = 0;
            //float newIron = 0;

            //float newGold = 0;
            //float newWood = 0;
            //float newWater = 0;
            //float newFuel = 0;

            for (int k = 0; k < (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Count; k++) 
            {
                //(nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].gameObject.GetComponent<LandSquare>().population += (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].gameObject.GetComponent<LandSquare>().population * hourPerTick * 0.00000134077f;

                //Debug.Log("owns " + k + " land squares" + (nations[item.Key].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[k].name);
                (nations[item.Key].GetComponent(typeof(Nation)) as Nation).GetAndSetPopulation();

                

            }

            //(nations[item.Key].GetComponent(typeof(Nation)) as Nation).population += newPop;



            //(nations[item.Key].GetComponent(typeof(Nation)) as Nation).population +=


        }

    }




    public void MakeMap1() 
    {
    
        for (int x = 0; x < worldSize; x++) 
        {
            for (int y = 0; y < worldSize; y++) 
            {
                //LandSquare newLandSquare = new LandSquare();

                GameObject newLandSquare;

                int randInt = Random.Range(0, 100);
                if (randInt <= 1)
                {
                    newLandSquare = (GameObject)Instantiate(Resources.Load("Prefabs/LakeTile_50x50"));
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "lake";
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).SetLandSquareResources(0, 1, 0, 0, 1);
                    //rand pop count
                    randInt = Random.Range(0, 4000);
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).population = randInt;
                }
                else if (randInt >= 15 && randInt <= 30)
                {
                    newLandSquare = (GameObject)Instantiate(Resources.Load("Prefabs/TreeTile_50x50"));
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "forest";
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).SetLandSquareResources(.08f, .2f, .3f, 1, .2f);
                    //rand pop count
                    randInt = Random.Range(0, 4000);
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).population = randInt;
                }
                
                else if (randInt >= 30 && randInt <= 45) 
                {
                    newLandSquare = (GameObject)Instantiate(Resources.Load("Prefabs/HillTile_50x50"));
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "hills";
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).SetLandSquareResources(.3f, .1f, .1f, 0, .2f);
                    //rand pop count
                    randInt = Random.Range(0, 4000);
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).population = randInt;
                }
                else if (randInt >= 98 && randInt <= 100)
                {
                    newLandSquare = (GameObject)Instantiate(Resources.Load("Prefabs/MountainTile_50x50"));
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "mountain";
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).SetLandSquareResources(1f, .1f, .1f, .1f, 0);
                    //rand pop count
                    randInt = Random.Range(0, 200);
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).population = randInt;
                }
                else 
                {
                    newLandSquare = (GameObject)Instantiate(Resources.Load("Prefabs/GrassTile_50x50"));
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).type = "grass_fields";
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).SetLandSquareResources(.08f, .1f, .1f, 0, 1);
                    //rand pop count
                    randInt = Random.Range(0, 4000);
                    (newLandSquare.GetComponent(typeof(LandSquare)) as LandSquare).population = randInt;

                }



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
                newLandSquare.GetComponent<LandSquare>().map = this.gameObject;




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
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[1].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[0,1]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[2].GetComponent<LandSquare>().factionOwner = "Nation1";
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares.Add(worldLandSquares[1,1]);
        (players[0].GetComponent(typeof(Nation)) as Nation).ownedLandSquares[3].GetComponent<LandSquare>().factionOwner = "Nation1";

        (players[0].GetComponent(typeof(Nation)) as Nation).gold = 10000000;
        nations.Add("Nation1", players[0]);

        players[0].GetComponent<Nation>().nationMainColor = new Color(0.59f, .75f, .18f, 1);//Green(0,1,0,1) //Darker Green(0.41f, .56f, .2f, 1) //Brigheter Green(0.59f, .75f, .18f, 1) //
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
        else if (type == "mountain")
        {
            sprite = Resources.Load<Sprite>("Sprites/Land/MountainTile_50x50");

        }
        else if (type == "hills") 
        {
            sprite = Resources.Load<Sprite>("Sprites/Land/HillTile_50x50");
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
