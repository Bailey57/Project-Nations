using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public GameObject gameMenu;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu() 
    {
        if (gameMenu.activeInHierarchy)
        {
            gameMenu.SetActive(false);
        }
        else 
        {
            gameMenu.SetActive(true);
        }
    
    }


}
