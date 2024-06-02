using UnityEngine;
using System.Collections.Generic;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] platformPrefabs; // ������� ��������
    [SerializeField] protected GameObject[] obstaclePrefabs; // ������� ����������� (����������� �������)
    [SerializeField] protected GameObject[] enemyPrefabs; // ������� ����������� (�������������� � �������)
    [SerializeField] protected Transform[] locationSpawnPoints; // ����� ������ ����������� ��������
    [SerializeField] protected Transform[] obstacleEnemySpawnPoints; // ����� ������ �����������
    [SerializeField] protected Transform[] playerSpawnPoints; // ����� ������ ������
    [SerializeField] protected GameObject playerPrefab; // ������ ������
    protected Transform player; // �����

    // ��������� ��� ������������ ������� ������ SpawnObjectAtTarget
    protected HashSet<Transform> usedSpawnPoints = new HashSet<Transform>();

    // ����� ��� ������ ���������
    public virtual GameObject SpawnPlatform(Vector3 position, int prefabIndex)
    {
        GameObject platform = Instantiate(platformPrefabs[prefabIndex], position, Quaternion.identity);
        Debug.Log($"Spawned platform at {position}");
        SpawnObstaclesOnPlatform(platform);
        SpawnEnemiesOnPlatform(platform);
        return platform;
    }

    // ����� ��� ������ ����������� �������� �� ���������
    protected void SpawnObstaclesOnPlatform(GameObject platform)
    {
        foreach (Transform point in locationSpawnPoints)
        {
            var platformPositionY = point.position.y + platform.transform.position.y;
            var platformPosition = new Vector3(point.position.x, platformPositionY, point.position.z);

            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
            var spawnedObstacle = Instantiate(obstaclePrefabs[obstacleIndex], platformPosition, Quaternion.identity, platform.transform);
            Debug.Log($"Spawned obstacle at {platformPosition}");
        }
    }

    // ����� ��� ������ ����������� �� ���������
    protected void SpawnEnemiesOnPlatform(GameObject platform)
    {
        foreach (Transform point in obstacleEnemySpawnPoints)
        {
            var platformPositionY = point.position.y + platform.transform.position.y;
            var platformPosition = new Vector3(point.position.x, platformPositionY, point.position.z);

            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            var spawnedEnemy = Instantiate(enemyPrefabs[enemyIndex], platformPosition, Quaternion.identity, platform.transform);
            Debug.Log($"Spawned enemy at {platformPosition}");
        }
    }

    // ����� ��� ������ �������� ������� �� ������ Target
    public virtual void SpawnObjectAtTarget(Transform target)
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

            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            var spawnedObject = Instantiate(obstaclePrefabs[randomIndex], platformPosition, Quaternion.identity, transform);
            Debug.Log($"Spawned object at {platformPosition}");
        }
    }

    // ����� ��� ������������� ������ �� ������ ����
    protected virtual void Start()
    {
        // ����� ����������� �������� �� ������ Target
        SpawnLocationObjects();

        // ����� ������ �� ��������� �����
        SpawnPlayer();
    }

    // ����� ��� ������ �������� ������� �� ������ Target
    protected void SpawnLocationObjects()
    {
        foreach (Transform point in locationSpawnPoints)
        {
            SpawnObjectAtTarget(point);
        }
    }

    // ����� ��� ������ ������ �� ��������� �����
    protected void SpawnPlayer()
    {
        if (playerSpawnPoints.Length > 0)
        {
            Transform spawnPoint = playerSpawnPoints[0]; // ����� ������� ��������� ��� ���������� ������
            GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player = playerInstance.transform;
            Debug.Log($"Spawned player at {spawnPoint.position}");
        }
        else
        {
            Debug.LogError("Player spawn points are not set.");
        }
    }
}