using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Transform spawnersTrf;
    public Transform pathWaysTrf;

    public LevelData levelData;
    public Database database;

    public List<Wave> listWaves = new List<Wave>();
    public List<Spawner> listSpawners = new List<Spawner>();
    public List<Pathway> listPathways = new List<Pathway>();

    [SerializeField] private int nextWaveID = 0;

    protected override void Awake()
    {
        base.Awake();

        InitLevel();
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.Spawn_Next_Wave, OnCreateNextWave);
    }

    public void InitLevel()
    {
        CreateSpawnersAndPathWays();
        StartCoroutine(UIManager.Instance.ShowWaveName(0));
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
            nextWaveID++;
            yield return new WaitForSeconds(levelData.timeBetweenWaves + listWaves[i].listMonstersData.Count * listWaves[i].spawnCooldown);
        }
    }

    public void CreateWave(int waveID)
    {
        var wave = Instantiate(levelData.layoutData.wavePrefab);
        wave.waveID = waveID;
        wave.name = "Wave " + (waveID + 1);
        wave.InitWave(levelData.wavesData[waveID]);
        listWaves.Add(wave);
    }


    public void OnCreateNextWave(object obj)
    {
        nextWaveID++;
        StartCoroutine(UIManager.Instance.ShowWaveName(nextWaveID));
    }

    public void CreateNextWave()
    {
        if (nextWaveID >= levelData.wavesData.Count)
        {
            Debug.Log("Hết Wave mất rồi :v");
            return;
        }

        var wave = Instantiate(levelData.layoutData.wavePrefab);
        wave.waveID = nextWaveID;
        wave.name = "Wave " + (nextWaveID + 1);
        wave.InitWave(levelData.wavesData[nextWaveID]);
        listWaves.Add(wave);
    }
}