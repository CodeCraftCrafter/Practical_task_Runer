using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] locationSpawnPoints; // ����� ������ ����������� ��������
    [SerializeField] private Transform[] obstacleEnemySpawnPoints; // ����� ������ �����������
    [SerializeField] private GameObject[] obstaclePrefabs; // ������� �����������
    [SerializeField] private GameObject[] enemyPrefabs; // ������� �����������

    private HashSet<Transform> usedSpawnPoints = new HashSet<Transform>();

    protected void Start()
    {
        if (!enabled) return;

        SpawnObjects();
    }

    // ����� ��� ������ �������� �� ������� ������
    private void SpawnObjects()
    {
        // ������� ����������� ������� �� ������ locationSpawnPoints
        foreach (Transform target in locationSpawnPoints)
        {
            if (!usedSpawnPoints.Contains(target))
            {
                SpawnObjectAtTarget(target);
            }
        }

        // ������� ����������� �� ������ obstacleEnemySpawnPoints
        foreach (Transform target in obstacleEnemySpawnPoints)
        {
            if (!usedSpawnPoints.Contains(target))
            {
                int enemyIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
                var platformPositionY = target.position.y + transform.position.y;
                var platformPosition = new Vector3(target.position.x, platformPositionY, target.position.z);
                var spawnedEnemy = Instantiate(enemyPrefabs[enemyIndex], platformPosition, target.rotation, transform);
                Debug.Log($"Spawned enemy at {platformPosition}");
            }
        }
    }

    // ����� ��� ������ ������� �� ��������� �����
    public void SpawnObjectAtTarget(Transform target)
    {
        if (usedSpawnPoints.Contains(target))
        {
            Debug.LogError($"SpawnObjectAtTarget called multiple times for the same target: {target.name}");
        }
        else
        {
            usedSpawnPoints.Add(target);
            var platformPositionY = target.position.y + transform.position.y;
            var platformPosition = new Vector3(target.position.x, platformPositionY, target.position.z);
            int randomIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Length);
            var spawnedObject = Instantiate(obstaclePrefabs[randomIndex], platformPosition, target.rotation, transform);
            Debug.Log($"Spawned object at {platformPosition}");
        }
    }
}