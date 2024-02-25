using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Level ", menuName = "Data/Level Data")]
public class LevelData : ScriptableObject
{
    public LevelLayoutData layoutData;
    public List<WaveData> wavesData;
    public float timeBetweenWaves;
}

[Serializable]
public class WaveData
{
    public int spawnerID;
    public int pathwayID;
    public List<int> listMonstersID;
    public float spawnCooldown;
}

[Serializable]
public class LevelLayoutData
{
    public Spawner spawnerPrefab;
    public Pathway pathwayPrefab;
    public Wave wavePrefab;
    public List<SpawnerData> spawnersData;
    public List<PathwayData> pathwaysData;
}

[Serializable]
public class SpawnerData
{
    public int spawnerID;
    public Vector2 position;
}

[Serializable]
public class PathwayData
{
    public int pathwayID;
    public Vector2 startPoint;
    public List<Vector2> waypoints;
}

