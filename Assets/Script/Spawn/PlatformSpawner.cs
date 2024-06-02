using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : GameSpawner
{
    [SerializeField] private int maxPlatforms = 10; // ������������ ���������� �������� � �������
    [SerializeField] private float spawnDistance = 20f; // ���������� �� ��������� ���������

    private Queue<GameObject> platformQueue = new Queue<GameObject>(); // ������� ��� �������� ��������
    private float lastSpawnZ; // Z-���������� ��������� ���������� ���������

    protected override void Start()
    {
        // ���������� ������� ��������� � �������� 0 ��� ��������� ���������
        Transform startPlatformTransform = platformPrefabs[0].transform;
        lastSpawnZ = startPlatformTransform.position.z;

        // ����� ��������� ��������
        for (int i = 0; i < maxPlatforms; i++)
        {
            Vector3 position = new Vector3(0, 0, lastSpawnZ + spawnDistance);
            GameObject platform = SpawnPlatform(position, 1); // ������ � �������� 1
            platformQueue.Enqueue(platform);
            lastSpawnZ += spawnDistance;
        }

        // ����� ������ �� ��������� �����
        base.Start();
    }

    void Update()
    {
        if (player.position.z - lastSpawnZ > spawnDistance)
        {
            Vector3 position = new Vector3(0, 0, lastSpawnZ);
            GameObject platform = SpawnPlatform(position, 1); // ������ � �������� 1
            platformQueue.Enqueue(platform);
            lastSpawnZ += spawnDistance;
            RemoveOldestPlatform();
        }
    }

    // ����� ��� ������ ���������
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

    // ����� ��� �������� ����� ������ ��������� �� �������
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