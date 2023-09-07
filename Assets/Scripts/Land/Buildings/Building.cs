using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Building
{
    string Name { get; set; }

    bool IsActive { get; set; }

    float HoursToBuild { get; set; }

    float ProductionInKGPerHour { get; set; }

    float MaintenanceCostPerHour { get; set; }

    string BuildingToString() { return ""; }
}
