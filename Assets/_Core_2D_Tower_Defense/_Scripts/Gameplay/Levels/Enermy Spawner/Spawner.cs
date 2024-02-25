using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int spawnerID;
    public SpawnerData spawnerData;

    public void InitSpawner(SpawnerData data)
    {
        spawnerData = data;
        transform.position = data.position;
    }
}
