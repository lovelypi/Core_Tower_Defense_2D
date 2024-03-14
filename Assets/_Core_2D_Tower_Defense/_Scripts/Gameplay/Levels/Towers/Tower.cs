using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerPosition towerPosition;
    public int turretID;
    [SerializeField] private TowerData towerData;
    public SpriteRenderer caseSprite;
    public SpriteRenderer turretSprite;
    public LayerMask enermyLayer;

    [SerializeField] private Transform firePoint;
    private Transform target;
    private int curLV = 0;
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
        var dataTurret = data.listSpecifications[curLV];
        damage = dataTurret.damage;
        caseSprite.sprite = dataTurret.caseSprite;
        turretSprite.sprite = dataTurret.turretSprite;
        bulletPrefab = dataTurret.bulletPrefab;
        cooldown = dataTurret.cooldown;
        shootingRange = dataTurret.shootingRange;
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
        var hits = Physics2D.CircleCastAll(position, shootingRange, position, 
            0f, enermyLayer);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
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
        Handles.DrawWireDisc(transform.position, transform.forward,shootingRange);
    }
}