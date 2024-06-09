using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float verticalOffset = 10.0f; // ¬ертикальное смещение камеры от игрока
    [SerializeField] private float horizontalOffset = 0.0f; // √оризонтальное смещение камеры от игрока
    [SerializeField] private float depthOffset = -10.0f; // —мещение камеры по оси Z (глубина)
    [SerializeField] private float followSpeed = 2.0f;
    [SerializeField] private float minYPosition = 0.0f; // ћинимальна€ позици€ по Y дл€ камеры

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
            // ќпредел€ем целевую позицию дл€ камеры
            var targetPosition = new Vector3(target.position.x + horizontalOffset, target.position.y + verticalOffset, target.position.z + depthOffset);
            targetPosition.y = Mathf.Max(targetPosition.y, minYPosition);

            // ѕлавное перемещение камеры к целевой позиции
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}