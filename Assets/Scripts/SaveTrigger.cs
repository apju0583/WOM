using UnityEngine;
using System.Collections;

public class SaveTrigger : MonoBehaviour
{
    private bool playerInRange;
    public GameObject saveText;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.SaveGame();
            Debug.Log("Game Saved!");
            StartCoroutine(ShowSaveText());
        }
    }

    private IEnumerator ShowSaveText()
    {
        if (saveText != null)
        {
            saveText.SetActive(true);
            yield return new WaitForSeconds(1f);
            saveText.SetActive(false);
        }
    }
}