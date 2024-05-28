using UnityEngine;
using System.Collections.Generic;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] platformPrefabs; // Префабы платформ
    [SerializeField] protected GameObject[] obstaclePrefabs; // Префабы препятствий
    [SerializeField] protected GameObject[] enemyPrefabs; // Префабы противников
    [SerializeField] protected Transform[] locationSpawnPoints; // Точки спавна объектов локации
    [SerializeField] protected Transform[] obstacleEnemySpawnPoints; // Точки спавна препятствий и противников
    [SerializeField] protected Transform player; // Игрок, по которому ориентируются точки спавна

    // Коллекция для отслеживания вызовов метода SpawnObjectAtTarget
    protected HashSet<Transform> usedSpawnPoints = new HashSet<Transform>();

    // Метод для спавна платформы
    public virtual void SpawnPlatform(Vector3 position)
    {
        int randomIndex = Random.Range(0, platformPrefabs.Length);
        GameObject platform = Instantiate(platformPrefabs[randomIndex], position, Quaternion.identity);
        SpawnObstaclesAndEnemiesOnPlatform(platform);
    }

    // Метод для спавна препятствий и противников на платформе
    protected void SpawnObstaclesAndEnemiesOnPlatform(GameObject platform)
    {
        foreach (Transform point in obstacleEnemySpawnPoints)
        {
            // Случайно выбираем, спавнить ли препятствие или противника
            bool spawnObstacle = Random.value > 0.5f;

            if (spawnObstacle)
            {
                // Спавн препятствия
                int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
                Instantiate(obstaclePrefabs[obstacleIndex], point.position, Quaternion.identity, platform.transform);
            }
            else
            {
                // Спавн противника
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[enemyIndex], point.position, Quaternion.identity, platform.transform);
            }
        }
    }

    // Метод для спавна объектов локации на точках Target
    public virtual void SpawnObjectAtTarget(Transform target)
    {
        if (usedSpawnPoints.Contains(target))
        {
            Debug.LogError("SpawnObjectAtTarget called multiple times for the same target: " + target.name);
        }
        else
        {
            usedSpawnPoints.Add(target);
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[randomIndex], target.position, Quaternion.identity);
        }
    }

    // Метод для инициализации спавна на старте игры
    protected virtual void Start()
    {
        // Спавн объектов локации на точках Target
        foreach (Transform point in locationSpawnPoints)
        {
            SpawnObjectAtTarget(point);
        }
    }
}
