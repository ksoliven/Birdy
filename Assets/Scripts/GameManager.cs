using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance;

    public delegate void GameDelegate();

    public static event GameDelegate onGameStarted;
    public static event GameDelegate onGameOverConfirmed;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;

    enum PageState
    {
        None,
        Start, 
        GameOver,
        Countdown
    }
    int score = 0;
    bool gameOver = false;

    public bool GameOver { get { return gameOver; } }

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountDownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;

    }

     void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountDownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountDownFinished()
    {
        SetPageState(PageState.None);
        onGameStarted();
        score = 0;
        gameOver = false;
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if(score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver()
    {
        onGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        SetPageState(PageState.Countdown);
    }
}
