using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStrengthBar : MonoBehaviour
{

    public GameObject unit;
    public GameObject slideBar;



    // Start is called before the first frame update
    void Start()
    {
        slideBar.GetComponent<Slider>().maxValue = unit.GetComponent<Unit>().maxForce;
        slideBar.GetComponent<Slider>().value = unit.GetComponent<Unit>().currentForce;

    }

    // Update is called once per frame
    void Update()
    {
        slideBar.GetComponent<Slider>().value = unit.GetComponent<Unit>().currentForce;

    }

}
