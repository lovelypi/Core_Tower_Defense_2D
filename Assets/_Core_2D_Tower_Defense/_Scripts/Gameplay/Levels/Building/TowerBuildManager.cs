using UnityEngine;

public class TowerBuildManager : Singleton<TowerBuildManager>
{
    [SerializeField] private GameObject[] towerPrefabs;
    private int selectedTowerID = 0;

    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTowerID];
    }
}
