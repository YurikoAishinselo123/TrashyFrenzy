using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    [Header("Spawn Range")]
    [SerializeField] float spawnRate = 5f;


    [Header("Spawn Range")]
    [SerializeField] float minX = -8f;
    [SerializeField] float maxX = 8f;

    // [Header("Spacing")]
    private float minSpacing = 1.5f;
    private int maxAttempts = 10;

    [Header("Y Positions")]
    [SerializeField] float spawnY = 6f;
    [SerializeField] float minStopY = -3f;
    [SerializeField] float maxStopY = 2f;

    [Header("Prefab")]
    [SerializeField] GameObject garbagePrefab;


    List<float> activeXPositions = new List<float>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnGarbage), 1f, spawnRate);
    }

    void SpawnGarbage()
    {
        float x;
        bool found = TryGetValidX(out x);

        if (!found)
        {
            return;
        }

        float stopY = Random.Range(minStopY, maxStopY);
        Vector3 spawnPos = new Vector3(x, spawnY, 0f);

        GameObject g = Instantiate(garbagePrefab, spawnPos, Quaternion.identity);
        g.GetComponent<GarbageController>().Init(stopY);

        activeXPositions.Add(x);
    }

    bool TryGetValidX(out float validX)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            float candidate = Random.Range(minX, maxX);
            bool tooClose = false;

            for (int j = 0; j < activeXPositions.Count; j++)
            {
                if (Mathf.Abs(candidate - activeXPositions[j]) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                validX = candidate;
                return true;
            }
        }

        validX = 0f;
        return false;
    }
}
