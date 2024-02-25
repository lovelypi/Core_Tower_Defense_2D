using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Transform wavesTrf;
    public Transform spawnersTrf;
    public Transform pathWaysTrf;

    public LevelData levelData;
    public MonstersDatabase monstersDatabase;

    public List<Wave> listWaves = new List<Wave>();
    public List<Spawner> listSpawners = new List<Spawner>();
    public List<Pathway> listPathways = new List<Pathway>();

    protected override void Awake()
    {
        base.Awake();

        InitLevel();
    }

    public void InitLevel()
    {
        CreateSpawnersAndPathWays();
        CreateWaves();
    }

    public void CreateSpawnersAndPathWays()
    {
        for (int i = 0; i < levelData.layoutData.spawnersData.Count; i++)
        {
            var spawner = Instantiate(levelData.layoutData.spawnerPrefab, spawnersTrf);
            spawner.spawnerID = i;
            spawner.InitSpawner(levelData.layoutData.spawnersData[i]);
            listSpawners.Add(spawner);
        }

        for (int i = 0; i < levelData.layoutData.pathwaysData.Count; i++)
        {
            var pathway = Instantiate(levelData.layoutData.pathwayPrefab, pathWaysTrf);
            pathway.pathwayID = i;
            pathway.InitPathway(levelData.layoutData.pathwaysData[i]);
            listPathways.Add(pathway);
        }
    }

    public void CreateWaves()
    {
        StartCoroutine(SpawnWavesWithDelay());
    }

    private IEnumerator SpawnWavesWithDelay()
    {
        for (int i = 0; i < levelData.wavesData.Count; i++)
        {
            CreateWave(i);
            yield return new WaitForSeconds(levelData.timeBetweenWaves);
        }
    }

    public void CreateWave(int waveID)
    {
        var wave = Instantiate(levelData.layoutData.wavePrefab);
        wave.waveID = waveID;
        wave.InitWave(levelData.wavesData[waveID]);
        listWaves.Add(wave);
    }
}