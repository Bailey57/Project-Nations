using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdvancedStatsToggle : MonoBehaviour
{
    public StartSettingsSO startSettingsSO;
  
    public GameObject toggle;




    public GameObject advancedStats;
    public GameObject simpleStats;

    // Start is called before the first frame update
    void Start()
    {
        toggle.GetComponent<Toggle>().isOn = startSettingsSO.showAdvancedStats;
    }

    void Update()
    {
        UpdateActive();



    }



    public void UpdateActive() 
    {
        try 
        {
            startSettingsSO.showAdvancedStats = toggle.GetComponent<Toggle>().isOn;
            if (startSettingsSO.showAdvancedStats)
            {
                advancedStats.SetActive(true);
                simpleStats.SetActive(false);
            }
            else 
            {
                advancedStats.SetActive(false);
                simpleStats.SetActive(true);
            }
        }
        catch(Exception e) 
        {
            Debug.LogException(e);
        }
        
    }






}
