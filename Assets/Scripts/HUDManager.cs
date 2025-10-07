using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Transform restartButton;

    // Game over screen controller variables
    public CanvasGroup gameOverScreen;
    public TextMeshProUGUI finalScoreText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        // hide game over screen
        gameOverScreen.alpha = 0;
        finalScoreText.text = "";
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    public void gameOverScene(int score)
    {
        gameOverScreen.alpha = 1;
        // Debug.Log(score);
        finalScoreText.GetComponent<TextMeshProUGUI>().text = "Final Score: " + score.ToString();
        scoreText.text = "";
    }

    public void GameRestart()
    {
        // hide game over screen
        gameOverScreen.alpha = 0;
        finalScoreText.text = "";
    }
}
