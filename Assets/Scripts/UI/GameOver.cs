using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    [SerializeField] public TMP_Text textMeshTxt;
    public GameObject map;



    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(GameEndScreen());
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    //StartCoroutine(PassTime());
    public IEnumerator GameEndScreen() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(1);
            if (map.GetComponent<Map>().allSquaresAnnexed)
            {
                
                textMeshTxt.text = GetEndGameString();
            }
            else
            {
                textMeshTxt.text = "";
            }

            //this.gameObject.SetActive(false);
            //textMeshTxt.text = GetEndGameString();
        }


    }



    public string GetEndingStats() 
    {
        string output = "";
        output += "";

        float biggestPopulation = 0;
        float mostLandSquares = 0;
        float mostGold = 0;
        float mostIronOre = 0;

        string nationWithBiggestPopulation = "";
        string nationWithMostLandSquares = "";
        string nationWithMostGold = "";
        string nationWithMostIronOre = "";


        foreach (var item in map.GetComponent<Map>().nations)
        {
            item.Value.GetComponent<Nation>().IncreaseGoldFor1Hour();
            if (item.Value.GetComponent<Nation>().population > biggestPopulation) 
            {
                biggestPopulation = item.Value.GetComponent<Nation>().population;
                nationWithBiggestPopulation = item.Value.GetComponent<Nation>().nationName;
            }
            if (item.Value.GetComponent<Nation>().ownedLandSquares.Count > mostLandSquares)
            {
                mostLandSquares = item.Value.GetComponent<Nation>().ownedLandSquares.Count;
                nationWithMostLandSquares = item.Value.GetComponent<Nation>().nationName;
            }
            if (item.Value.GetComponent<Nation>().gold > mostGold)
            {
                mostGold = item.Value.GetComponent<Nation>().gold;
                nationWithMostGold = item.Value.GetComponent<Nation>().nationName;
            }
            if (item.Value.GetComponent<Nation>().metricTonsOfIronOre > mostIronOre)
            {
                mostIronOre = item.Value.GetComponent<Nation>().metricTonsOfIronOre;
                nationWithMostIronOre = item.Value.GetComponent<Nation>().nationName;
                Debug.Log("nationWithMostIronOre: " + nationWithMostIronOre + " nationWithMostIronOre: " + nationWithMostIronOre);
            }
        }

        output += "\n\nNation With Biggest Population: " + nationWithBiggestPopulation;
        output += "\n\nNation With MostLand Squares: " + nationWithMostLandSquares;
        output += "\n\nNation With Most Gold: " + nationWithMostGold;
        output += "\n\nNation With Most IronOre: " + nationWithMostIronOre;
        output += "\n";
      
        return output;

    }

    public string GetEndGameString()
    {
        string output = "Game Over\n";
        output += GetEndingStats();


        return output;
    }
}
