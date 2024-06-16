using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float verticalOffset = 10.0f;
    [SerializeField] private float horizontalOffset = 0.0f;
    [SerializeField] private float depthOffset = -10.0f;
    [SerializeField] private float followSpeed = 2.0f;
    [SerializeField] private float minYPosition = 0.0f;

    private void OnEnable()
    {
        GameSpawner.OnPlayerSpawned += SetTarget;
    }

    private void OnDisable()
    {
        GameSpawner.OnPlayerSpawned -= SetTarget;
    }

    private void SetTarget(Transform player)
    {
        target = player;
    }

    private void Update()
    {
        if (target != null)
        {
 
            var targetPosition = new Vector3(target.position.x + horizontalOffset, target.position.y + verticalOffset, target.position.z + depthOffset);
            targetPosition.y = Mathf.Max(targetPosition.y, minYPosition);

  
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}