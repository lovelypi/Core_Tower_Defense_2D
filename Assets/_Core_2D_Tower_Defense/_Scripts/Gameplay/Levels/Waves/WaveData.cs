using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Wave ", menuName = "Data/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<Monster> listMonsters;
}
