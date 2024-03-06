using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Transform target;
    public float speed = 10f;
    public float damage;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (target)
        {
            RotateToTarget();
        }
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            PoolingManager.Despawn(gameObject);
            return;
        }

        var direction = (target.position - transform.position).normalized;

        rb.velocity = direction * speed;
    }

    public void SetTarget(Transform tar)
    {
        target = tar;
    }

    private void RotateToTarget()
    {
        if (target)
        {
            var targetPosition = target.position;
            var position = transform.position;
            var angle = Mathf.Atan2(targetPosition.y - position.y, targetPosition.x - position.x) * Mathf.Rad2Deg - 90f;
            var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            transform.rotation = targetRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enermy"))
        {
            PoolingManager.Despawn(gameObject);
            var monster = other.gameObject.GetComponent<Monster>();
            monster.TakeDamage(damage);
        }
    }
}