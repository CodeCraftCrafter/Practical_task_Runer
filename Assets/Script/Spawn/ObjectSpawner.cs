using UnityEngine;

public class ObjectSpawner : GameSpawner
{
    protected override void Start()
    {
        base.Start();
        SpawnObjects();
    }
    private void SpawnObjects()
    {
        foreach (Transform target in locationSpawnPoints)
        {
            if (!usedSpawnPoints.Contains(target))
            {
                SpawnObjectAtTarget(target);
            }
        }

        foreach (Transform target in obstacleEnemySpawnPoints)
        {
            if (!usedSpawnPoints.Contains(target))
            {
                bool spawnObstacle = Random.value > 0.5f;

                if (spawnObstacle)
                {
                    int randomIndexObstacle = Random.Range(0, obstaclePrefabs.Length);
                    Instantiate(obstaclePrefabs[randomIndexObstacle], target.position, target.rotation, target);
                }
                else
                {
                    int randomIndexEnemy = Random.Range(0, enemyPrefabs.Length);
                    Instantiate(enemyPrefabs[randomIndexEnemy], target.position, target.rotation, target);
                }
            }
        }
    }

    public override void SpawnObjectAtTarget(Transform target)
    {
        if (usedSpawnPoints.Contains(target))
        {
            Debug.LogError("SpawnObjectAtTarget called multiple times for the same target: " + target.name);
        }
        else
        {
            usedSpawnPoints.Add(target);
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[randomIndex], target.position, target.rotation, target);
        }
    }
}