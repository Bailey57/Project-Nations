using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfBotNations : MonoBehaviour
{
    public GameObject worldSizeSlider;

    public GameObject nationCountSlider;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nationCountSlider.GetComponent<Slider>().maxValue > worldSizeSlider.GetComponent<Slider>().value * worldSizeSlider.GetComponent<Slider>().value - 6)
        {
            nationCountSlider.GetComponent<Slider>().value = worldSizeSlider.GetComponent<Slider>().value * worldSizeSlider.GetComponent<Slider>().value - 6;
            nationCountSlider.GetComponent<Slider>().maxValue = worldSizeSlider.GetComponent<Slider>().value * worldSizeSlider.GetComponent<Slider>().value - 6;


        }
        else if (nationCountSlider.GetComponent<Slider>().maxValue < worldSizeSlider.GetComponent<Slider>().value * worldSizeSlider.GetComponent<Slider>().value - 6)
        {
            nationCountSlider.GetComponent<Slider>().maxValue = 30;


        }
        
       // worldSizeSlider.GetComponent<Slider>().value
    }
}
