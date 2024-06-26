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
        Generator.OnPlayerSpawned += SetTarget;
        Debug.Log("Camera subscribed to OnPlayerSpawned event");
    }

    private void OnDisable()
    {
        Generator.OnPlayerSpawned -= SetTarget;
        Debug.Log("Camera unsubscribed from OnPlayerSpawned event");
    }

    private void SetTarget(Transform player)
    {
        target = player;
        Debug.Log($"Camera target set to {player.name}");
    }

    private void Update()
    {
        if (target != null)
        {
            var targetPosition = new Vector3(target.position.x + horizontalOffset, target.position.y + verticalOffset, target.position.z + depthOffset);
            targetPosition.y = Mathf.Max(targetPosition.y, minYPosition);

            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Target is null, waiting for spawn event.");
        }
    }
}