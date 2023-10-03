using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        textMeshTxt.text = GetTimeString();
    }


    public GameObject map;
    public TMP_Text textMeshTxt;

    public string GetTimeString() 
    {

        int hours = map.GetComponent<Map>().gameHoursPassed;

       

        int days = hours / 24;
        int remainderHrs = hours % 24;

        int weeks = days / 7;
        int remainderDays = days % 7;

        //int months = weeks / 7;
        //int remainderWeeks = weeks % 7;

        int years = weeks / 52;
        int remainderWeeks = weeks % 52;

        //int years = remainderDays

        string output = "Years: " + years + "Weeks: " + remainderWeeks + " Days: " + remainderDays + " Hours: " + remainderHrs;
        return output;


    }


    
}
