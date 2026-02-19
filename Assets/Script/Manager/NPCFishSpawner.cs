using UnityEngine;

public class NPCFishSpawner : MonoBehaviour
{
    [Header("Fish Types")]
    [SerializeField] NPCFishController fishPrefab;
    [SerializeField] NPCFishDataSO[] fishTypes;

    [Header("Spawn Area")]
    [SerializeField] float minY = -4f;
    [SerializeField] float maxY = 4f;
    [SerializeField] float spawnXLeft = -12f;
    [SerializeField] float spawnXRight = 12f;

    float[] timers;

    void Awake()
    {
        timers = new float[fishTypes.Length];
    }

    void Update()
    {
        for (int i = 0; i < fishTypes.Length; i++)
        {
            timers[i] += Time.deltaTime;

            if (timers[i] >= fishTypes[i].spawnInterval)
            {
                SpawnFish(fishTypes[i]);
                timers[i] = 0f;
            }
        }
    }

    void SpawnFish(NPCFishDataSO data)
    {
        float y = Random.Range(minY, maxY);
        bool fromLeft = Random.value > 0.5f;

        float x = fromLeft ? spawnXLeft : spawnXRight;
        int direction = fromLeft ? 1 : -1;

        Vector3 pos = new Vector3(x, y, 0f);

        NPCFishController fish =
            Instantiate(fishPrefab, pos, Quaternion.identity);

        fish.Init(data, direction);
    }
}
