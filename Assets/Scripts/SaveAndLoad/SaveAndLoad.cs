using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{

    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool SaveGame()
    {
        try 
        {
            var jsonSave = JsonUtility.ToJson(map);
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = baseDir + @"\Saves";



            if (Directory.Exists(fileName)) 
            {
                
                fileName += DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss");
                File.WriteAllText(fileName, jsonSave);
            }

            
        }
        catch (Exception e)
        {
            Debug.Log("Failed saving game: " + e);
        }

        return false;
    }


    //TODO
    public bool SaveGame(GameObject map)
    {
        try
        {
            //var options = new JsonSerializerOptions { IncludeFields = true };

            var jsonSave = JsonUtility.ToJson(map, true);
            //var jsonSave = System.Text.Json.JsonSerializer.Serialize(map, options);
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = baseDir + @"\Saves";



            if (Directory.Exists(fileName))
            {

                fileName += DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss");
                File.WriteAllText(fileName, jsonSave);
            }


        }
        catch (Exception e)
        {
            Debug.Log("Failed saving game: " + e);
        }

        return false;
    }

}
