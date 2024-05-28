using UnityEngine;
using System.Collections.Generic;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] platformPrefabs;
    [SerializeField] protected GameObject[] obstaclePrefabs;
    [SerializeField] protected GameObject[] enemyPrefabs;
    [SerializeField] protected Transform[] locationSpawnPoints;
    [SerializeField] protected Transform[] obstacleEnemySpawnPoints;
    [SerializeField] protected Transform player;

    protected HashSet<Transform> usedSpawnPoints = new HashSet<Transform>();


    public virtual void SpawnPlatform(Vector3 position)
    {
        int randomIndex = Random.Range(0, platformPrefabs.Length);
        GameObject platform = Instantiate(platformPrefabs[randomIndex], position, Quaternion.identity);
        SpawnObstaclesAndEnemiesOnPlatform(platform);
    }

    protected void SpawnObstaclesAndEnemiesOnPlatform(GameObject platform)
    {
        foreach (Transform point in obstacleEnemySpawnPoints)
        {
            bool spawnObstacle = Random.value > 0.5f;

            if (spawnObstacle)
            {
                int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
                Instantiate(obstaclePrefabs[obstacleIndex], point.position, Quaternion.identity, platform.transform);
            }
            else
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[enemyIndex], point.position, Quaternion.identity, platform.transform);
            }
        }
    }

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

    protected virtual void Start()
    {
        foreach (Transform point in locationSpawnPoints)
        {
            SpawnObjectAtTarget(point);
        }
    }
}
