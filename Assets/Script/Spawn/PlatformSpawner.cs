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
        base.Start();
        lastSpawnZ = player.position.z; // Инициализация начальной координаты
        for (int i = 0; i < maxPlatforms; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (player.position.z - lastSpawnZ > spawnDistance)
        {
            SpawnPlatform();
            RemoveOldestPlatform();
        }
    }

    // Метод для спавна платформы
    public override void SpawnPlatform(Vector3 position = default)
    {
        if (position == default)
        {
            position = new Vector3(0, 0, lastSpawnZ + spawnDistance);
        }

        base.SpawnPlatform(position);
        lastSpawnZ += spawnDistance;
        platformQueue.Enqueue(Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], position, Quaternion.identity));
    }

    // Метод для удаления самой старой платформы из очереди
    private void RemoveOldestPlatform()
    {
        if (platformQueue.Count > maxPlatforms)
        {
            GameObject oldestPlatform = platformQueue.Dequeue();
            Destroy(oldestPlatform);
        }
    }
}