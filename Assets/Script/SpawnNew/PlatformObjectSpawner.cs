using UnityEngine;

public class PlatformObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        public float spawnChance;
    }

    public SpawnableObject[] objectsToSpawn; // Массив объектов для спавна
    public Transform[] spawnPoints; // Точки спавна на платформе

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
        }
    }

    void SpawnObjects()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnableObject chosenObject = ChooseObjectToSpawn();
            if (chosenObject.prefab != null)
            {
                Instantiate(chosenObject.prefab, spawnPoint.position, spawnPoint.rotation, transform);
            }
        }
    }

    SpawnableObject ChooseObjectToSpawn()
    {
        float totalChance = 0f;
        foreach (var obj in objectsToSpawn)
        {
            totalChance += obj.spawnChance;
        }

        float randomPoint = Random.value * totalChance;

        foreach (var obj in objectsToSpawn)
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

        return objectsToSpawn[objectsToSpawn.Length - 1];
    }
}