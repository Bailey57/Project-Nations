using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightLandSquaresToggle : MonoBehaviour
{
    public GameObject map;

    public void SwitchHighlightToggle() 
    {
        map.GetComponent<Map>().highlightLandSquares = !map.GetComponent<Map>().highlightLandSquares;
        map.GetComponent<Map>().UpdateBorders();
    }
}
