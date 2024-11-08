using System.Collections;
using UnityEngine;

public class KnockSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayKnockSound(5f));
    }

    private IEnumerator PlayKnockSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (audioSource != null)
        {
            audioSource.Play();
        }

        yield return new WaitForSeconds(audioSource.clip.length);
        GameManager_First.instance.ActivateFirstPanel();
    }
}