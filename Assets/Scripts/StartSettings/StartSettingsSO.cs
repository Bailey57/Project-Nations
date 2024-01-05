using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StartSettingsSO : ScriptableObject
{
    [SerializeField] public bool spawnPlayer;

    [SerializeField] public int mapSize;
    [SerializeField] public int numberOfNations;


    [SerializeField] public bool showAdvancedStats;

    public int MapSize
    {
		get { return mapSize; }
		set { mapSize = value; }
	}
    public int NumberOfNations
    {
        get { return numberOfNations; }
        set { numberOfNations = value; }
    }

    public bool SpawnPlayer
    {
        get { return spawnPlayer; }
        set { spawnPlayer = value; }
    }

    public bool ShowAdvancedStats
    {
        get { return showAdvancedStats; }
        set { showAdvancedStats = value; }
    }




}
