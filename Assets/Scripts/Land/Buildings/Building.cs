using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Building
{
    string Name { get; set; }
    int CooldownDays { get; set; }

    float ProductionInKG { get; set; }

    
}
