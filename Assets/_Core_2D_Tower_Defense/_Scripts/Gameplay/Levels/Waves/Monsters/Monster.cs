using UnityEngine;

public class Monster : MonoBehaviour
{
    public Wave wave;
    [SerializeField] private MonsterData monsterData;
    private Rigidbody2D rb;
    [SerializeField] private float curHP;

    public HealthBar healthBar;
    public Vector2 target;
    public int pathIndex = 0;
    public int IDInWave;
    private int spiritStoneAmount;
    private int damage;

    private float notTakeDamageTime = 0f;
    private float timeToHideHealthBar = 2f;

    public void InitMonster(MonsterData data)
    {
        monsterData = data;
        curHP = data.maxHP;
        spiritStoneAmount = data.spiritStoneAmount;
        damage = data.damage;
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHP(data.maxHP);
    }

    private void Start()
    {
        target = wave.pathway.wayPoints[0];
        healthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        notTakeDamageTime += Time.deltaTime;

        if (notTakeDamageTime >= timeToHideHealthBar)
        {
            healthBar.gameObject.SetActive(false);
        }
        
        if (Vector2.Distance(target, transform.position) <= 0.1f)
        {
            pathIndex++;
        }

        if (pathIndex == wave.pathway.wayPoints.Count)
        {
            EventDispatcher.Instance.PostEvent(EventID.On_Monster_Escaped, damage);
            wave.listMonsters.Remove(this);
            wave.CheckIfAllEnermyDead();
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

    public void TakeDamage(float amount)
    {
        healthBar.gameObject.SetActive(true);
        notTakeDamageTime = 0f;
        curHP -= amount;
        
        if (curHP <= 0f)
        {
            wave.listMonsters.Remove(this);
            wave.CheckIfAllEnermyDead();
            EventDispatcher.Instance.PostEvent(EventID.On_Monster_Killed, spiritStoneAmount);
            Destroy(gameObject);
        }
        
        healthBar.SetHP(curHP);
    }
}