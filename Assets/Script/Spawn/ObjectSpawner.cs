using UnityEngine;

public class ObjectSpawner : GameSpawner
{
    protected override void Start()
    {
        base.Start();
        SpawnObjects();
    }

    // Метод для спавна объектов на целевых точках
    private void SpawnObjects()
    {
        // Спавним объекты локации на точках locationSpawnPoints
        foreach (Transform target in locationSpawnPoints)
        {
            if (!usedSpawnPoints.Contains(target))
            {
                SpawnObjectAtTarget(target);
            }
        }

        // Спавним объекты (препятствия и/или противников) на точках obstacleEnemySpawnPoints
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

    // Метод для спавна объекта на указанной точке (переписываем метод базового класса)
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