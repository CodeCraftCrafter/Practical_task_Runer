using UnityEngine;
using System.Collections.Generic;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] platformPrefabs; // Префабы платформ
    [SerializeField] protected GameObject[] obstaclePrefabs; // Префабы препятствий (локационные объекты)
    [SerializeField] protected GameObject[] enemyPrefabs; // Префабы противников (взаимодействие с игроком)
    [SerializeField] protected Transform[] locationSpawnPoints; // Точки спавна локационных объектов
    [SerializeField] protected Transform[] obstacleEnemySpawnPoints; // Точки спавна противников
    [SerializeField] protected Transform[] playerSpawnPoints; // Точки спавна игрока
    [SerializeField] protected GameObject playerPrefab; // Префаб игрока
    protected Transform player; // Игрок

    // Коллекция для отслеживания вызовов метода SpawnObjectAtTarget
    protected HashSet<Transform> usedSpawnPoints = new HashSet<Transform>();

    // Метод для спавна платформы
    public virtual GameObject SpawnPlatform(Vector3 position, int prefabIndex)
    {
        GameObject platform = Instantiate(platformPrefabs[prefabIndex], position, Quaternion.identity);
        Debug.Log($"Spawned platform at {position}");
        SpawnObstaclesOnPlatform(platform);
        SpawnEnemiesOnPlatform(platform);
        return platform;
    }

    // Метод для спавна локационных объектов на платформе
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

    // Метод для спавна противников на платформе
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

    // Метод для спавна объектов локации на точках Target
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

    // Метод для инициализации спавна на старте игры
    protected virtual void Start()
    {
        // Спавн локационных объектов на точках Target
        SpawnLocationObjects();

        // Спавн игрока на стартовой точке
        SpawnPlayer();
    }

    // Метод для спавна объектов локации на точках Target
    protected void SpawnLocationObjects()
    {
        foreach (Transform point in locationSpawnPoints)
        {
            SpawnObjectAtTarget(point);
        }
    }

    // Метод для спавна игрока на стартовой точке
    protected void SpawnPlayer()
    {
        if (playerSpawnPoints.Length > 0)
        {
            Transform spawnPoint = playerSpawnPoints[0]; // Можно выбрать случайный или конкретный индекс
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