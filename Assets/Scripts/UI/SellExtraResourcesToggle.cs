using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellExtraResourcesToggle : MonoBehaviour
{
  
    public GameObject nation;

    public GameObject toggle;

    public bool selling;
    // Start is called before the first frame update
    void Start()
    {
        //selling = toggle.GetComponent<Toggle>().IsActive();
        StartCoroutine(SellExtra());
    }

    void Update() 
    {

        //selling = toggle.GetComponent<Toggle>().IsActive();


    }

  

    public IEnumerator SellExtra()
    { 
        while (true) 
        {
            //TODO: put in nation actions so bots can sell extra ore
            yield return new WaitForSeconds(1);
            if (toggle.GetComponent<Toggle>().isOn) 
            {
                float oneMetricTonOfIronWorth = 120;

                nation.GetComponent<Nation>().gold += nation.GetComponent<Nation>().metricTonsOfIronOre * oneMetricTonOfIronWorth;
                nation.GetComponent<Nation>().metricTonsOfIronOre = 0;
            }

        }
    
    }


}
