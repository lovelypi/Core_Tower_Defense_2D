using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : Singleton<TowerBuildManager>
{
    public GameObject towerPrefab;
    public List<TowerInLevel> towersInLevel;

    public void Init()
    {
        towersInLevel = LevelManager.Instance.levelData.towersInLevel;
    }
}
