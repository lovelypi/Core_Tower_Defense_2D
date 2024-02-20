using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] public WaveData waveData;
    [SerializeField] private List<Monster> listMonsters;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private int waveID;
}
