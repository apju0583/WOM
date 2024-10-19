using System;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public event Action OnCollected;
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CollectPlant();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void CollectPlant()
    {
        OnCollected?.Invoke();
        Destroy(gameObject);
    }
}