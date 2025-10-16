using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent<int> gameOver;

    public int score = 0;
    private bool isGameOver = false;

    private AudioSource gameOverAudioSource;
    private AudioSource backgroundAudioSource;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        
        // Set to be 30 FPS
        Application.targetFrameRate =  30;

        gameOverAudioSource = GameObject.Find("GameOverMusic").GetComponent<AudioSource>();
        backgroundAudioSource = GameObject.Find("Static Environment").GetComponent<AudioSource>();

        // subscribe to scene manager scene change;
        SceneManager.activeSceneChanged += SceneSetup;
        Debug.Log("Game Manager invoke Start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        gameOverAudioSource = GameObject.Find("GameOverMusic").GetComponent<AudioSource>();
        backgroundAudioSource = GameObject.Find("Static Environment").GetComponent<AudioSource>();
        SetScore(score);
    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        isGameOver = false;
        backgroundAudioSource.Play();
        gameOverAudioSource.Stop();
    }

    public void IncreaseScore(int increment)
    {
        // Debug.Log("Increasing score by: " + increment);
        score += increment;
        SetScore(score);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        if(isGameOver) return;
        isGameOver = true;
    }

    public void sendScore(int score)
    {
        Time.timeScale = 0.0f;
        if(gameOver != null)
        {
            gameOver.Invoke(score);
            gameOverAudioSource.Play();
            backgroundAudioSource.Stop();
        }
    }
}