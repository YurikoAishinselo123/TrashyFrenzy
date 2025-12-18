using UnityEngine;
using System.Collections.Generic;

public class SchoolFishSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 3f;
    [SerializeField] int laneCount = 5;
    [SerializeField] float minY = -4f;
    [SerializeField] float maxY = 4f;
    [SerializeField] float spawnOffsetX = 12f;
    [SerializeField] GameObject fishPrefab;

    float timer;

    List<float> lanes = new();
    HashSet<int> occupied = new();

    void Awake()
    {
        float step = (maxY - minY) / (laneCount - 1);
        for (int i = 0; i < laneCount; i++)
            lanes.Add(minY + step * i);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        if (occupied.Count >= laneCount)
            return;

        int lane;
        do lane = Random.Range(0, laneCount);
        while (occupied.Contains(lane));

        occupied.Add(lane);

        bool fromLeft = Random.value > 0.5f;
        float x = fromLeft ? -spawnOffsetX : spawnOffsetX;

        GameObject fish = Instantiate(
            fishPrefab,
            new Vector3(x, lanes[lane], 0f),
            Quaternion.identity
        );

        fish.GetComponent<SchoolFishController>().Init(
            fromLeft ? Vector2.right : Vector2.left,
            lane,
            this
        );
    }

    public void ReleaseLane(int lane)
    {
        occupied.Remove(lane);
    }
}
