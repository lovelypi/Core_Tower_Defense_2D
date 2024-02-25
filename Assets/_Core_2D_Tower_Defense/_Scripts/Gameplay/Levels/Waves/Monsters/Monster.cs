using UnityEngine;

public class Monster : MonoBehaviour
{
    public Wave wave;
    [SerializeField] private MonsterData monsterData;
    private Rigidbody2D rb;
    private float curHP;

    public Vector2 target;
    public int pathIndex = 0;
    public int IDInWave;

    public void InitMonster(MonsterData data)
    {
        monsterData = data;
        curHP = data.maxHP;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitMonster(monsterData);
    }

    private void Start()
    {
        target = wave.pathway.wayPoints[0];
    }

    private void Update()
    {
        if (Vector2.Distance(target, transform.position) <= 0.1f)
        {
            pathIndex++;
        }

        if (pathIndex == wave.pathway.wayPoints.Count)
        {
            Destroy(gameObject);
        }
        else
        {
            target = wave.pathway.wayPoints[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = ((Vector3)target - transform.position).normalized;
        rb.velocity = direction * monsterData.speed;
    }

    private void TakeDamage(float amount)
    {
        
    }
}