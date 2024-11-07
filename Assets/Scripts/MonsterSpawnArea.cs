using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnArea : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int maxMonsterInArea;
    public float spawnInterval;

    Collider2D spawnAreaCollier;
    List<GameObject> spawnedMonsters = new List<GameObject>();

    void Start() 
    {
        spawnAreaCollier = GetComponent<Collider2D>();
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnedMonsters.Count < maxMonsterInArea) {
                SpawnMonster();
            }
        }
    }

    void SpawnMonster() 
    {
        Vector2 randPos = GetRandomPosition();
        GameObject newMonster = Instantiate(monsterPrefab, randPos, Quaternion.identity);
        spawnedMonsters.Add(newMonster);
    }

    Vector2 GetRandomPosition() 
    {
        Bounds bounds = spawnAreaCollier.bounds;
        float randX = Random.Range(bounds.min.x, bounds.max.x);
        float randY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randX, randY);
    }
}