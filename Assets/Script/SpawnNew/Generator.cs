using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct SectionPrefab
{
    public Section section;
    public float spawnChance;
}

public class Generator : MonoBehaviour
{
    public Section[] start; // ��������� ������ �� �����
    public SectionPrefab[] prefabs; // ������ � ����� ����������� ������� � ������� ���������
    public SectionPrefab[] stop; // �������� ������ � ������� ���������
    public int sections = 20; // ����. ��������� ����� ������
    public float sectionSize = 1; // ������ ������, ��� ������� ������ ���� �����

    public GameObject playerPrefab; // ������ ������
    public Transform[] playerSpawnPoints; // ����� ������ ������
    public static event System.Action<Transform> OnPlayerSpawned; // ������� ��� ������ ������
    public static event System.Action<Section> OnSectionSpawned; // ������� ��� ������ ������

    private List<Section> currentSections = new List<Section>();
    private Queue<Section> sectionQueue = new Queue<Section>();
    private int index;
    private Transform playerTransform;

    void Start()
    {
        Generate();
    }

    void InstSection(SectionPrefab[] arr, Transform previousEndPoint = null)
    {
        Section newSection = Instantiate(ChooseSection(arr).section) as Section;
        newSection.gameObject.name = "Section_" + index;
        newSection.transform.parent = transform;

        if (previousEndPoint != null)
        {
            Vector3 direction = GetRaycastDirection(previousEndPoint);
            newSection.transform.forward = direction;
            newSection.transform.position = previousEndPoint.position + (newSection.transform.position - newSection.startPoint.position);
        }

        currentSections.Add(newSection);
        sectionQueue.Enqueue(newSection);

        // ����� ������� ������ ������
        OnSectionSpawned?.Invoke(newSection);

        foreach (var endPoint in newSection.endPoints)
        {
            if (index < sections)
            {
                index++;
                StartCoroutine(SpawnNextSection(endPoint));
            }
        }
    }

    IEnumerator SpawnNextSection(Transform endPoint)
    {
        yield return null; // ����� �������� �������� ����� ������� ��������� ������, ���� ����������
        InstSection(prefabs, endPoint);
    }

    void Generate()
    {
        foreach (var startSection in start)
        {
            currentSections.Add(startSection);
            sectionQueue.Enqueue(startSection);
            foreach (var endPoint in startSection.endPoints)
            {
                if (index < sections)
                {
                    index++;
                    StartCoroutine(SpawnNextSection(endPoint));
                }
            }
        }

        if (playerSpawnPoints.Length > 0) // ������� ������ �� ����� �� ��������� ������
        {
            Debug.Log("Spawning player...");
            SpawnPlayer(playerSpawnPoints[0]);
        }
    }

    void SpawnPlayer(Transform spawnPoint)
    {
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerTransform = playerInstance.transform;
        Debug.Log($"Spawned player at {spawnPoint.position}");
        Debug.Log("Invoking OnPlayerSpawned event");
        OnPlayerSpawned?.Invoke(playerTransform);
        Debug.Log("OnPlayerSpawned event invoked");
    }

    void Update()
    {
        if (playerTransform != null && sectionQueue.Count > 0)
        {
            Section oldestSection = sectionQueue.Peek();
            if (Vector3.Distance(playerTransform.position, oldestSection.endPoints[0].position) > sectionSize * 3)
            {
                Destroy(sectionQueue.Dequeue().gameObject);
                Transform lastEndPoint = currentSections[currentSections.Count - 1].endPoints[0];
                InstSection(prefabs, lastEndPoint);
            }
        }
    }

    bool Check() // ��������, ���� �� �� ���� ����� ��������� ������
    {
        Vector3 position = currentSections[currentSections.Count - 1].endPoints[0].position + currentSections[currentSections.Count - 1].endPoints[0].forward * sectionSize / 2;
        Collider[] colliders = Physics.OverlapSphere(position, sectionSize / 4);
        foreach (Collider hit in colliders) if (hit) return true;
        return false;
    }

    Vector3 GetRaycastDirection(Transform endPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(endPoint.position, endPoint.forward, out hit, sectionSize))
        {
            Debug.Log($"Raycast hit {hit.collider.name} at {hit.point}");
            return (hit.point - endPoint.position).normalized;
        }
        else
        {
            Debug.Log("Raycast did not hit anything, using default forward direction.");
            return endPoint.forward;
        }
    }

    SectionPrefab ChooseSection(SectionPrefab[] arr)
    {
        float total = 0;
        foreach (SectionPrefab prefab in arr)
        {
            total += prefab.spawnChance;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < arr.Length; i++)
        {
            if (randomPoint < arr[i].spawnChance)
            {
                return arr[i];
            }
            else
            {
                randomPoint -= arr[i].spawnChance;
            }
        }
        return arr[arr.Length - 1];
    }
}