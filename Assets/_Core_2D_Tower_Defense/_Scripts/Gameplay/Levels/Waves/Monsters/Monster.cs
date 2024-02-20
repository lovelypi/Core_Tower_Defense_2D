using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    #region Components

    [SerializeField] private MonsterData monsterData;
    private Rigidbody2D rb;

    #endregion

    #region Specific Data

    [SerializeField] private float monsterID;
    [SerializeField] private float curHP;

    #endregion

    private Transform target;
    private int pathIndex = 0;

    private void Awake()
    {
        curHP = monsterData.maxHP;
        target = LevelManager.Instance.pathway[pathIndex];
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            
            if (pathIndex >= LevelManager.Instance.pathway.Length)
            {
                Destroy(gameObject);
            }
            else
            {
                target = LevelManager.Instance.pathway[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * monsterData.speed;
    }
}