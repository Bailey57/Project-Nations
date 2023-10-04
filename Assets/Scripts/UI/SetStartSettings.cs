using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetStartSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startSettingsSO.mapSize = (int)worldSizeSlider.GetComponent<Slider>().value;

        startSettingsSO.numberOfNations = (int)nationsSlider.GetComponent<Slider>().value;

    }

    public GameObject worldSizeSlider;

    public GameObject nationsSlider;

    public StartSettingsSO startSettingsSO;
}
