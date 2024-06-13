using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject hitPrefab; // Префаб точки попадания
    [SerializeField] private float destroyDelay = 0f; // Задержка перед уничтожением пули
    public string[] targetTags = { "Target_1", "Target_2" };

    private Weapon weapon;
    private Rigidbody rb;

    private void OnEnable()
    {
        weapon = FindObjectOfType<Weapon>(); // Найти оружие в сцене
        rb = GetComponent<Rigidbody>();
        Invoke("Deactivate", lifeTime);
    }

    private void Update()
    {
        // Перемещаем пулю вперед
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider coll)
    {
        foreach (string currentTag in targetTags)
        {
            if (currentTag == coll.transform.tag)
            {
                EnemyHealth enemyHealth = coll.transform.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.AddDamage(damage);
                    Debug.Log($"Enemy hit! Damage dealt: {damage}. Remaining HP: {enemyHealth.HP}");

                    if (enemyHealth.HP <= 0)
                    {
                        Debug.Log("Enemy defeated!");
                    }
                }

                // Создание точки попадания
                var contactPoint = coll.ClosestPoint(transform.position);
                var rotation = Quaternion.LookRotation(coll.transform.position - transform.position);
                Instantiate(hitPrefab, contactPoint, rotation);

                // Деактивируем пулю сразу после столкновения
                Deactivate();
                return;
            }
        }
    }

    private void Deactivate()
    {
        CancelInvoke(); // Отменяем все вызовы Invoke
        Debug.Log("Bullet deactivated");
        weapon.ReturnBulletToPool(gameObject);
    }
}