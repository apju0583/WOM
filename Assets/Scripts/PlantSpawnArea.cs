using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawnArea : MonoBehaviour
{
    public GameObject plantPrefab;
    public int maxPlantsInArea;
    public float spawnInterval;

    private BoxCollider2D spawnAreaCollider;
    private List<GameObject> spawnedPlants = new List<GameObject>();

    void Start()
    {
        spawnAreaCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SpawnPlantsAtIntervals());
    }

    private IEnumerator SpawnPlantsAtIntervals()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnedPlants.Count < maxPlantsInArea)
            {
                SpawnPlant();
            }
        }
    }

    private void SpawnPlant()
    {
        Vector2 randomPosition = GetRandomPositionWithinBounds();
        
        GameObject newPlant = Instantiate(plantPrefab, randomPosition, Quaternion.identity);
        spawnedPlants.Add(newPlant);

        newPlant.GetComponent<Plant>().OnCollected += () => {
            spawnedPlants.Remove(newPlant);
        };
    }

    private Vector2 GetRandomPositionWithinBounds()
    {
        Bounds bounds = spawnAreaCollider.bounds;
        
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}