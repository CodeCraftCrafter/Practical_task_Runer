using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : GameSpawner
{
    [SerializeField] private int maxPlatforms = 10;
    [SerializeField] private float spawnDistance = 20f;

    private Queue<GameObject> platformQueue = new Queue<GameObject>();
    private float lastSpawnZ;

    protected override void Start()
    {
        base.Start();
        lastSpawnZ = player.position.z;
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

    private void RemoveOldestPlatform()
    {
        if (platformQueue.Count > maxPlatforms)
        {
            GameObject oldestPlatform = platformQueue.Dequeue();
            Destroy(oldestPlatform);
        }
    }
}