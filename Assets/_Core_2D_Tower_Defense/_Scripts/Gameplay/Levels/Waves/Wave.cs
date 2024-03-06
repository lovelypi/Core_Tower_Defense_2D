using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int waveID;
    public WaveData waveData;
    public List<MonsterData> listMonstersData;
    public List<Monster> listMonsters;
    public float spawnCooldown;

    private LevelData levelData;
    private Database _database;
    
    private Transform spawnerTrf;
    public Pathway pathway;

    public void InitWave(WaveData data)
    {
        levelData = LevelManager.Instance.levelData;
        _database = LevelManager.Instance.database;
        
        waveData = data;
        spawnCooldown = data.spawnCooldown;

        var listMonstersID = data.listMonstersID;
        for (int i = 0; i < listMonstersID.Count; i++)
        {
            listMonstersData.Add(_database.listMonstersData[listMonstersID[i]]);
        }

        spawnerTrf = LevelManager.Instance.listSpawners[data.spawnerID].transform;
        pathway = LevelManager.Instance.listPathways[data.pathwayID];
        transform.SetParent(spawnerTrf);
        
        SpawnEnermyWave();
    }

    private void SpawnEnermyWave()
    {
        StartCoroutine(SpawnEnermyWithDelay());
    }

    private IEnumerator SpawnEnermyWithDelay()
    {
        for (int i = 0; i < listMonstersData.Count; i++)
        {
            SpawnEnermy(i);
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    private void SpawnEnermy(int IDInWave)
    {
        var enermy = PoolingManager.Spawn(_database.listMonstersData[waveData.listMonstersID[IDInWave]].monsterPrefab);
        enermy.name = listMonstersData[IDInWave].monsterName + " " + (IDInWave + 1);
        enermy.wave = this;
        enermy.IDInWave = IDInWave;
        enermy.transform.position = spawnerTrf.position;
        enermy.InitMonster(listMonstersData[IDInWave]);
        listMonsters.Add(enermy);
        enermy.transform.SetParent(transform);
    }
}
