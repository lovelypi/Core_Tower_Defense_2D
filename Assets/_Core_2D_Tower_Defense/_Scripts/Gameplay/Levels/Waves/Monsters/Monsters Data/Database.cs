using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Database", menuName = "Data/Database")]
public class Database : ScriptableObject
{
    public List<MonsterData> listMonstersData;
    [FormerlySerializedAs("turretPrefab")] public Tower towerPrefab;
    public List<TowerData> listTowersData;
}

[Serializable]
public class TowerData
{
    public string towerName;
    public int towerID;
    public bool canAirShoot;
    public List<TurretSpecification> listSpecifications;
}

[Serializable]
public class TurretSpecification
{
    public string name;
    public int spiritStoneToBuy;
    public int spiritStoneGetWhenSale;
    public float damage;
    public Sprite caseSprite;
    public Sprite towerSprite;
    public Bullet bulletPrefab;
    public float cooldown;
    public float shootingRange;
}
    
    
[Serializable]
public class MonsterData 
{
    public string monsterName;
    public int monsterID;
    public int spiritStoneAmount;
    public int damage;
    public Monster monsterPrefab;
    public float maxHP;
    public float speed = 2f;
}
