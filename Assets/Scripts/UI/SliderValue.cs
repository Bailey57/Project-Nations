using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{


    public Slider slider;
    public GameObject text;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        //text.GetComponent<TMP_Text>().text = "";

        //text.GetComponent<TMP_Text>().text = "" + slider.value.ToString() + " by " + slider.value.ToString();
        text.GetComponent<TMP_Text>().text = "" + slider.value.ToString();
        //text.GetComponent<TMP_Text>().text = "";
        //Debug.Log("" + slider.name + " " + slider.value.ToString());

    }

   


}
