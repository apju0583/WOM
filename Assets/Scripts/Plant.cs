using System;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    public event Action OnCollected;
    public Item plantItemA;
    public Item plantItemB;
    public float probabilityA;

    public GameObject progressBarPrefab;
    private GameObject progressBarInstance;
    private Image progressBarImage;

    private bool isPlayerInRange = false;
    private bool isCollecting = false;

    public float collectTime = 2f;
    private float collectProgress = 0f;

    private Transform player;

    void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.E))
        {
            if (!isCollecting)
            {
                StartCollecting();
            }

            if (isCollecting && progressBarInstance != null)
            {
                collectProgress += Time.deltaTime / collectTime;
                progressBarImage.fillAmount = collectProgress;

                if (collectProgress >= 1f)
                {
                    CollectPlant();
                }
            }
        }
        else if (isCollecting && Input.GetKeyUp(KeyCode.E))
        {
            ResetCollection();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ResetCollection();
        }
    }

    private void StartCollecting()
    {
        isCollecting = true;
        progressBarInstance = Instantiate(progressBarPrefab, player.position + Vector3.up * 1, Quaternion.identity, player);
        progressBarImage = progressBarInstance.GetComponentInChildren<Image>();

        if (progressBarImage != null)
        {
            progressBarImage.fillAmount = 0f;
        }
    }

    private void ResetCollection()
    {
        isCollecting = false;
        collectProgress = 0f;

        if (progressBarInstance != null)
        {
            Destroy(progressBarInstance);
            progressBarInstance = null;
        }
    }

    private void CollectPlant()
    {
        OnCollected?.Invoke();
        AddRandomPlantToInventory();
        SoundManager.instance.SFXPlay(15);
        Destroy(gameObject);
        ResetCollection();
    }

    private void AddRandomPlantToInventory()
    {
        Inventory inventory = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
        Item selectedItem = (UnityEngine.Random.value <= probabilityA) ? plantItemA : plantItemB;

        if (selectedItem != null)
        {
            inventory.AddItem(selectedItem);
            inventory.FreshSlot();
            GameManager.instance.GetItemBar().RefreshSlot();
            Debug.Log("획득한 아이템 : " + selectedItem.name);
        }
    }
}