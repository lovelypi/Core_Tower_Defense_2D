using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerPosition towerPosition;
    [SerializeField] private TowerData towerData;
    public SpriteRenderer caseSprite;
    public SpriteRenderer turretSprite;
    public LayerMask enermyLayer;

    [SerializeField] private Transform firePoint;
    private Transform target;
    public int towerID;
    public int curLevel = 0;
    private float rotateSpeed = 1000f;
    private float fireTime = 0f;

    #region Specifications

    public Bullet bulletPrefab;
    public float cooldown;
    public float damage;
    public float shootingRange;

    #endregion

    public void InitTower(TowerData data)
    {
        caseSprite = transform.Find("Case").GetComponent<SpriteRenderer>();
        turretSprite = transform.Find("Turret").GetComponent<SpriteRenderer>();
        firePoint = transform.Find("Fire Point");
        towerData = data;
        towerID = data.towerID;
        var specification = data.listSpecifications[curLevel];
        LoadSpecification(specification);
    }

    public void LoadSpecification(TurretSpecification specification)
    {
        damage = specification.damage;
        caseSprite.sprite = specification.caseSprite;
        turretSprite.sprite = specification.towerSprite;
        bulletPrefab = specification.bulletPrefab;
        cooldown = specification.cooldown;
        shootingRange = specification.shootingRange;
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!CheckIfTargetInShootingRange())
        {
            target = null;
        }
        else
        {
            fireTime += Time.deltaTime;

            if (fireTime >= cooldown)
            {
                fireTime = 0f;
                Shoot();
            }
        }

        RotateToTarget();
    }
    
    private void FindTarget()
    {
        var position = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, shootingRange, enermyLayer);

        if (colliders.Length > 0)
        {
            List<Monster> monstersInShootingRange = new List<Monster>();

            // Lọc ra tất cả các quái vật nằm trong tầm bắn
            foreach (var collider in colliders)
            {
                Monster monster = collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monstersInShootingRange.Add(monster);
                }
            }

            // Nếu không có quái vật nào trong tầm bắn, thoát khỏi hàm
            if (monstersInShootingRange.Count == 0)
            {
                return;
            }

            // Tìm quái vật có IDInWave nhỏ nhất
            Monster targetMonster = monstersInShootingRange[0];
            foreach (var monster in monstersInShootingRange)
            {
                if (monster.IDInWave < targetMonster.IDInWave)
                {
                    targetMonster = monster;
                }
            }

            // Đặt quái vật có IDInWave nhỏ nhất làm mục tiêu
            target = targetMonster.transform;
        }
    }
    
    private bool CheckIfTargetInShootingRange()
    {
        return Vector2.Distance(target.position, transform.position) <= shootingRange;
    }

    private void RotateToTarget()
    {
        if (target)
        {
            var targetPosition = target.position;
            var position = transform.position;
            var angle = Mathf.Atan2(targetPosition.y - position.y, targetPosition.x - position.x) * Mathf.Rad2Deg - 90f;
            var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                rotateSpeed * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        var bullet = PoolingManager.Spawn(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.damage = damage;
        bullet.SetTarget(target);
        bullet.transform.SetParent(transform);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.forward, shootingRange);
    }
}