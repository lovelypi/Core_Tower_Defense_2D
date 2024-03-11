using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelData levelData;
    public Database database;
    
    public Transform spawnersTrf;
    public Transform pathWaysTrf;

    public List<Wave> listWaves = new List<Wave>();
    public List<Spawner> listSpawners = new List<Spawner>();
    public List<Pathway> listPathways = new List<Pathway>();

    private int spiritStone = 0;
    private int lives = 0;

    public int SpiritStone
    {
        get => spiritStone;
        set
        {
            spiritStone = value;
            EventDispatcher.Instance.PostEvent(EventID.On_Spirit_Stone_Change, value);
        }
    }

    public int Lives
    {
        get => lives;
        set
        {
            lives = value;
            EventDispatcher.Instance.PostEvent(EventID.On_Lives_Change, value);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        InitLevel();
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Spawn_Next_Wave, 
            param => OnCreateNextWave((int) param));
        EventDispatcher.Instance.RegisterListener(EventID.On_Monster_Killed, 
            param => OnMonsterKilled((int) param));
        EventDispatcher.Instance.RegisterListener(EventID.On_Monster_Escaped, 
            param => OnMonsterEscaped((int) param));
    }

    public void InitLevel()
    {
        SpiritStone = levelData.spiritStoneStart;
        Lives = levelData.liveStart;
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

    // Bắt sự kiện tạo Wave mới
    public void OnCreateNextWave(int waveID)
    {
        waveID++;
        StartCoroutine(UIManager.Instance.ShowWaveName(waveID));
    }

    // Sinh quái cho Wave 
    public void CreateWave(int waveID)
    {
        if (waveID >= levelData.wavesData.Count)
        {
            Debug.Log("Hết Wave mất rồi :v");
            return;
        }
        var wave = Instantiate(levelData.layoutData.wavePrefab);
        wave.waveID = waveID;
        wave.name = "Wave " + (waveID + 1);
        wave.InitWave(levelData.wavesData[waveID]);
        listWaves.Add(wave);
    }

    private void OnMonsterKilled(int spiritStoneGet)
    {
        SpiritStone += spiritStoneGet;
    }

    private void OnMonsterEscaped(int livesAmount)
    {
        Lives -= livesAmount;
        if (Lives <= 0)
        {
            Lives = 0;
            Debug.Log("Lost Game");
        }
    }
}