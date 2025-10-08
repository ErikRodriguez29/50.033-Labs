using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent<int> gameOver;

    public int score = 0;
    private bool isGameOver = false;

    public AudioSource gameOverAudioSource;
    public AudioSource backgroundAudioSource;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
    }

    // Update is called once per frame
    void Update()
    {

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