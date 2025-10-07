using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    void Start()
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        StartCoroutine(Wait(0.1f));
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = true;
        }

        StartCoroutine(Wait(1f));

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = false;
        }
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}