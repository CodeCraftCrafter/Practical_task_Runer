using UnityEngine;

public class PlatformObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        public float spawnChance;
    }

    public SpawnableObject[] objectsToSpawn; // ������ �������� ��� ������
    public Transform[] spawnPoints; // ����� ������ �� ���������

    public SpawnableObject[] obstaclesToSpawn; // ������ ����������� ��� ������
    public Transform[] obstacleSpawnPoints; // ����� ������ ����������� �� ���������

    private void OnEnable()
    {
        Generator.OnSectionSpawned += OnSectionSpawned;
    }

    private void OnDisable()
    {
        Generator.OnSectionSpawned -= OnSectionSpawned;
    }

    private void OnSectionSpawned(Section section)
    {
        if (section == GetComponent<Section>())
        {
            SpawnObjects();
            SpawnObstacles();
        }
    }

    void SpawnObjects()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnableObject chosenObject = ChooseObjectToSpawn(objectsToSpawn);
            if (chosenObject.prefab != null)
            {
                Instantiate(chosenObject.prefab, spawnPoint.position, spawnPoint.rotation, transform);
            }
        }
    }

    void SpawnObstacles()
    {
        foreach (Transform spawnPoint in obstacleSpawnPoints)
        {
            SpawnableObject chosenObstacle = ChooseObjectToSpawn(obstaclesToSpawn);
            if (chosenObstacle.prefab != null)
            {
                Instantiate(chosenObstacle.prefab, spawnPoint.position, spawnPoint.rotation, transform);
            }
        }
    }

    SpawnableObject ChooseObjectToSpawn(SpawnableObject[] spawnables)
    {
        float totalChance = 0f;
        foreach (var obj in spawnables)
        {
            totalChance += obj.spawnChance;
        }

        float randomPoint = Random.value * totalChance;

        foreach (var obj in spawnables)
        {
            if (randomPoint < obj.spawnChance)
            {
                return obj;
            }
            else
            {
                randomPoint -= obj.spawnChance;
            }
        }

        return spawnables[spawnables.Length - 1];
    }
}