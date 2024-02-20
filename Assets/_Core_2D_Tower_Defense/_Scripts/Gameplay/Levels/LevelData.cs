using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Level ", menuName = "Data/Level Data")]
public class LevelData : ScriptableObject
{
    public List<Wave> listWaves;
}
