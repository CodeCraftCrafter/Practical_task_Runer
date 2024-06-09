using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : GameSpawner
{
    [SerializeField] private int maxPlatforms = 10; // Максимальное количество платформ в очереди
    [SerializeField] private float spawnDistance = 20f; // Расстояние до следующей платформы

    private Queue<GameObject> platformQueue = new Queue<GameObject>(); // Очередь для хранения платформ
    private float lastSpawnZ; // Z-координата последней спавненной платформы

    private void OnEnable()
    {
        GameSpawner.OnPlatformSpawned += ActivateObjectSpawner;
    }

    private void OnDisable()
    {
        GameSpawner.OnPlatformSpawned -= ActivateObjectSpawner;
    }

    protected override void Start()
    {
        // Используем базовую платформу с индексом 0 как начальную платформу
        Transform startPlatformTransform = platformPrefabs[0].transform;
        lastSpawnZ = startPlatformTransform.position.z;

        // Спавн начальных платформ
        for (int i = 0; i < maxPlatforms; i++)
        {
            Vector3 position = new Vector3(0, 0, lastSpawnZ + spawnDistance);
            int randomIndex = UnityEngine.Random.Range(1, platformPrefabs.Length); // Выбираем случайный индекс префаба > 0
            GameObject platform = SpawnPlatform(position, randomIndex); // Префаб с индексом 1 и выше
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
            int randomIndex = UnityEngine.Random.Range(1, platformPrefabs.Length); // Выбираем случайный индекс префаба > 0
            GameObject platform = SpawnPlatform(position, randomIndex); // Префаб с индексом 1 и выше
            platformQueue.Enqueue(platform);
            lastSpawnZ += spawnDistance;
            RemoveOldestPlatform();
        }
    }

    // Метод для активации ObjectSpawner на новой платформе
    private void ActivateObjectSpawner(GameObject platform)
    {
        ObjectSpawner spawner = platform.GetComponent<ObjectSpawner>();
        if (spawner != null)
        {
            spawner.enabled = true; // Активируем ObjectSpawner на новой платформе
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