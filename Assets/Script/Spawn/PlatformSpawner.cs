using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : GameSpawner
{
    [SerializeField] private int maxPlatforms = 10; // Максимальное количество платформ в очереди
    [SerializeField] private float spawnDistance = 20f; // Расстояние до следующей платформы

    private Queue<GameObject> platformQueue = new Queue<GameObject>(); // Очередь для хранения платформ
    private float lastSpawnZ; // Z-координата последней спавненной платформы

    protected override void Start()
    {
        // Используем базовую платформу с индексом 0 как начальную платформу
        Transform startPlatformTransform = platformPrefabs[0].transform;
        lastSpawnZ = startPlatformTransform.position.z;

        // Спавн начальных платформ
        for (int i = 0; i < maxPlatforms; i++)
        {
            Vector3 position = new Vector3(0, 0, lastSpawnZ + spawnDistance);
            GameObject platform = SpawnPlatform(position, 1); // Префаб с индексом 1
            platformQueue.Enqueue(platform);
            lastSpawnZ += spawnDistance;
        }

        // Спавн игрока на стартовой точке
        base.Start();
    }

    void Update()
    {
        if (player.position.z - lastSpawnZ > spawnDistance)
        {
            Vector3 position = new Vector3(0, 0, lastSpawnZ);
            GameObject platform = SpawnPlatform(position, 1); // Префаб с индексом 1
            platformQueue.Enqueue(platform);
            lastSpawnZ += spawnDistance;
            RemoveOldestPlatform();
        }
    }

    // Метод для спавна платформы
    public override GameObject SpawnPlatform(Vector3 position = default, int prefabIndex = 1)
    {
        if (position == default)
        {
            position = new Vector3(0, 0, lastSpawnZ + spawnDistance);
        }

        GameObject platform = base.SpawnPlatform(position, prefabIndex);
        Debug.Log($"Enqueued platform at {position}");
        return platform;
    }

    // Метод для удаления самой старой платформы из очереди
    private void RemoveOldestPlatform()
    {
        if (platformQueue.Count > maxPlatforms)
        {
            GameObject oldestPlatform = platformQueue.Dequeue();
            Destroy(oldestPlatform);
            Debug.Log($"Removed oldest platform");
        }
    }
}