using UnityEngine;
using System;
using System.Collections.Generic;

public class GameSpawner : MonoBehaviour
{
    public static event Action<GameObject> OnPlatformSpawned;
    public static event Action<Transform> OnPlayerSpawned;

    [SerializeField] protected GameObject[] platformPrefabs; 
    [SerializeField] protected GameObject[] obstaclePrefabs;
    [SerializeField] protected GameObject[] enemyPrefabs;
    [SerializeField] protected Transform[] locationSpawnPoints;
    [SerializeField] protected Transform[] obstacleEnemySpawnPoints;
    [SerializeField] protected Transform[] playerSpawnPoints;
    [SerializeField] protected GameObject playerPrefab;
    protected Transform player;

      protected HashSet<Transform> usedSpawnPoints = new HashSet<Transform>();

     public virtual GameObject SpawnPlatform(Vector3 position, int prefabIndex)
    {
        GameObject platform = Instantiate(platformPrefabs[prefabIndex], position, Quaternion.identity);
        Debug.Log($"Spawned platform at {position}");
        OnPlatformSpawned?.Invoke(platform);
        return platform;
    }

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

            int randomIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Length);
            var spawnedObject = Instantiate(obstaclePrefabs[randomIndex], platformPosition, Quaternion.identity, transform);
            Debug.Log($"Spawned object at {platformPosition}");
        }
    }

      protected virtual void Start()
    {
        SpawnLocationObjects();

         SpawnPlayer();
    }

     protected void SpawnLocationObjects()
    {
        foreach (Transform point in locationSpawnPoints)
        {
            SpawnObjectAtTarget(point);
        }
    }

      protected void SpawnPlayer()
    {
        if (playerSpawnPoints.Length > 0)
        {
            Transform spawnPoint = playerSpawnPoints[0];
            GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player = playerInstance.transform;
            Debug.Log($"Spawned player at {spawnPoint.position}");
            OnPlayerSpawned?.Invoke(player);
        }
        else
        {
            Debug.LogError("Player spawn points are not set.");
        }
    }
}