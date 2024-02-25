using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monsters Database", menuName = "Data/Monster Database")]
public class MonstersDatabase : ScriptableObject
{
    public List<MonsterData> listMonstersData;
}

[Serializable]
public class MonsterData 
{
    public string monsterName;
    public int monsterID;
    public Monster monsterPrefab;
    public float maxHP;
    public float speed = 2f;
}
