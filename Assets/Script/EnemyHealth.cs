using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float HP = 10;

    public void AddDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy defeated!");
        }
        else
        {
            Debug.Log($"Enemy took {damage} damage, remaining HP: {HP}");
        }
    }
}