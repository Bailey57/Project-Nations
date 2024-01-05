using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public GameObject gameMenu;



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
