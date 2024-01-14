using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text equationText;

    public Text Yscore;
    public Text highscore;
    public int score { get; private set; }
    public int correct { get; private set; }
    public int inCorrect { get; private set; }
    private Player player;
    private Spawning spawner;

    public GameObject playButton;
    public GameObject menuButton;
    public GameObject gameOver;
    public GameObject pauseButton;
    public GameObject playAfterPauseButton;

    public List<string> equations = new List<string>();

    private void Awake()
    {
        Application.targetFrameRate = 60; 
        
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawning>();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        pauseButton.SetActive(false);
        playAfterPauseButton.SetActive(false);
        menuButton.SetActive(true);
        Yscore.text = "";
        highscore.text = "";

        //Play();
        PauseBut();
        //Play();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        //Hide play button and game over
        playButton.SetActive(false);
        gameOver.SetActive(false);
        menuButton.SetActive(false);

        pauseButton.SetActive(true);
        playAfterPauseButton.SetActive(false);


        Yscore.text = "";
        highscore.text = "";

        Time.timeScale = 1f;
        player.enabled = true;
        player.Reset();

        Clear();
    }

    private void Clear()
    {
        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }
    
    public void PauseBut()
    {
        pauseButton.SetActive(false);
        Pause();
        playAfterPauseButton.SetActive(true);
        menuButton.SetActive(true);
    }

    public void PlayBut()
    {
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        menuButton.SetActive(false);
        pauseButton.SetActive(true);
        playAfterPauseButton.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;
    }

    public void GameOver()
    {
        Debug.Log("Games over");
        equations.Clear();
        equationText.text = "";

        playButton.SetActive(true);
        gameOver.SetActive(true);
        pauseButton.SetActive(false);
        playAfterPauseButton.SetActive(false);
        menuButton.SetActive(true);

        Pause();
        Clear();

        if (PlayerPrefs.HasKey("hiScore"))
{
            int highScore = PlayerPrefs.GetInt("hiScore");
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("hiScore", highScore);
                PlayerPrefs.Save();
            }
        }
        else
        {
            PlayerPrefs.SetInt("hiScore", score);
            PlayerPrefs.Save();
        }

        Yscore.text = "Your score: " + score.ToString();
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("hiScore").ToString();
    }

    public void toMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();

        equationText.text = equations[0];
        equations.RemoveAt(0);
    }

    public void addEquation(string eq)
    {
        equations.Add(eq);
        if (equations.Count == 1 && equationText.text.Length == 0) { equationText.text = eq; equations.RemoveAt(0); }
    }

    public void GenerateEquation()
    {
        string[] signs = { "+", "-", "/", "x" };
        int a = Random.Range(1, 20);
        int b = Random.Range(1, 20);
        string sign = signs[Random.Range(0,3)];

        int result = 0;
        if(sign == "+") { result = a + b; }
        else if (sign == "-") 
        { 
            if (a < b) { int temp = a; a = b; b = temp; }
            result = a - b; 
        }
        else if (sign == "/") 
        {
            result = Random.Range(1, 5);
            a = b * result;
        }
        else if (sign == "x") { result = a * b; }

        correct = result;
        inCorrect = -1;
        equationText.text = a + " " + sign + " " + b;
    }
	
	
}
