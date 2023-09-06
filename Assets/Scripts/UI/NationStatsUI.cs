using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NationStatsUI : MonoBehaviour
{


    public GameObject nation;
    public TMP_Text textMeshTxt;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMeshTxt.text = (nation.GetComponent("Nation") as Nation).NationToString();
    }

    




}
