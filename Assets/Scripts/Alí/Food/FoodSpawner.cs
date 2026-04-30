using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public float rangeX = 10f;
    public float rangeZ = 10f;
    public float timeBetweenSpawns = 2f;

    private float timer;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenSpawns)
        {
            SpawnFood();
            timer = 0f;
        }
    }
    void SpawnFood()
    {
        float randomX = Random.Range(-rangeX, rangeX);
        float randomZ = Random.Range(-rangeZ, rangeZ);

        Vector3 spawnPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        Instantiate(foodPrefab, spawnPos, Quaternion.identity);
    }
}

