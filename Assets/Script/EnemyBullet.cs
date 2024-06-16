using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject hitPrefab;
    public float attackRange = 10f;
    public string[] targetTags = { "Target_1", "Target_2" };

    private Vector3 startPoint;
    private Rigidbody rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        startPoint = transform.position;
    }

    private void Update()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);

        if (Vector3.Distance(startPoint, transform.position) > attackRange)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        foreach (string currentTag in targetTags)
        {
            if (currentTag == coll.transform.tag)
            {
                Player player = coll.transform.GetComponent<Player>();
                if (player != null)
                {
                    Debug.Log($"Player hit! Damage dealt: {damage}");

                    var contactPoint = coll.ClosestPoint(transform.position);
                    var rotation = Quaternion.LookRotation(coll.transform.position - transform.position);
                    Instantiate(hitPrefab, contactPoint, rotation);

                    Deactivate();
                    return;
                }
            }
        }

        Deactivate();
    }

    private void Deactivate()
    {
        CancelInvoke();
        Debug.Log("Enemy bullet deactivated");
    
    }
}