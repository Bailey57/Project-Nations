using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, Building
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Mine(string Name, int CooldownDays, float ProductionInKG) 
    {
        this.Name = Name;
        this.CooldownDays = CooldownDays;
        this.ProductionInKG = ProductionInKG;
    
    }

    public string Name { get; set; }
    public int CooldownDays { get; set; }

    public float ProductionInKG { get; set; }





}
