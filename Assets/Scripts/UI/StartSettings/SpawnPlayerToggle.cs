using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayerToggle : MonoBehaviour
{
    // Start is called before the first frame update

    public StartSettingsSO startSettingsSO;
    public GameObject toggle;

    void Start()
    {
        toggle.GetComponent<Toggle>().isOn = startSettingsSO.spawnPlayer;
    }

    
}
