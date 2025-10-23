
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButtonController : MonoBehaviour, IInteractiveButton
{
    private bool isPaused = false;
    public Sprite pauseIcon;
    public Sprite playIcon;
    private Image image;

    public AudioSource pauseSound;
    private AudioSource backgroundAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        backgroundAudioSource = GameObject.Find("Static Environment").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SceneSetup(Scene current, Scene next)
    {
        backgroundAudioSource = GameObject.Find("Static Environment").GetComponent<AudioSource>();
    }

    public void ButtonClick()
    {
        pauseSound.Stop();
        backgroundAudioSource.Stop();
        Time.timeScale = isPaused ? 1.0f : 0.0f;
        isPaused = !isPaused;
        if (isPaused)
        {
            image.sprite = playIcon;
        }
        else
        {
            image.sprite = pauseIcon;
        }
        pauseSound.Play();
    }
}
