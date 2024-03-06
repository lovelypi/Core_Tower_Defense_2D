using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "Data/Database")]
public class Database : ScriptableObject
{
    public List<MonsterData> listMonstersData;
    public Turret turretPrefab;
    public List<TurretData> ListTurretsData;
}

[Serializable]
public class TurretData
{
    public string turretName;
    public int turretID;
    public bool canAirShoot;
    public List<TurretSpecification> listSpecifications;
}

[Serializable]
public class TurretSpecification
{
    public string name;
    public float damage;
    public Sprite caseSprite;
    public Sprite turretSprite;
    public Bullet bulletPrefab;
    public float cooldown;
    public float shootingRange;
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
