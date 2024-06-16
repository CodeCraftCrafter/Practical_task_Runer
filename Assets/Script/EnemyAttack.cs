using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 10f;
    public float attackDelay = 2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    [SerializeField] private Transform player;

    private bool isAttacking = false;

    private void OnEnable()
    {
        GameSpawner.OnPlayerSpawned += SetPlayer;
    }

    private void OnDisable()
    {
        GameSpawner.OnPlayerSpawned -= SetPlayer;
    }

    private void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);

        Shoot();

        isAttacking = false;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Calculate direction and velocity for the bullet to reach the edge of attack range
        Vector3 direction = (player.position - bulletSpawnPoint.position).normalized;
        rb.velocity = direction * bulletSpeed;

        // Set the attack range for the bullet
        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
        enemyBullet.attackRange = attackRange;

        // Log for shooting action
        Debug.Log("Enemy fired at player!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}