using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject[] m_Tanks;
    public TextMeshProUGUI m_MessageText;
    public TextMeshProUGUI m_TimerText;
    public TextMeshProUGUI m_HighscoreText;

    private float m_gameTime = 0;
    public float GameTime {  get { return m_gameTime; } }

    public enum GameState
    {
        Start, // 0
        Playing, // 1
        GameOver // 2
    };

    private GameState m_GameState;
    public GameState State { get { return m_GameState; } }
    int[] scores = new int[10];


    private void Awake()
    {
        m_GameState = GameState.Start;
    }

    private void Start()
    {
        // Read scores from file
        if (!File.Exists(Application.dataPath + "\\highscores"))
        {
            StreamWriter _scoresfile = new StreamWriter(Application.dataPath + "\\highscores");
            _scoresfile.Close();
            string[] result = Array.ConvertAll(scores, x => x.ToString());
            File.WriteAllLines(Application.dataPath + "\\highscores", result);
        }
        string[] _fileScores = File.ReadAllLines(Application.dataPath + "\\highscores");
        scores = Array.ConvertAll(_fileScores, x => Int32.Parse(x));


        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(false);
        }

        m_TimerText.gameObject.SetActive(false);
        m_HighscoreText.gameObject.SetActive(false);
        m_MessageText.text = "Get Ready";
    }

    private void Update()
    {
        switch (m_GameState)
        {
            case GameState.Start:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_TimerText.gameObject.SetActive(true);
                    m_HighscoreText.gameObject.SetActive(true);
                    m_MessageText.text = "";
                    m_GameState = GameState.Playing;

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                }
                break;
            case GameState.Playing:
                bool isGameOver = false;
                int scoreToSet = 0;
                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                m_TimerText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));
                foreach(int _scores in scores) // Loops through all scores to find the lowest time (fastest win)
                {
                    if (_scores != 0 && _scores != null)
                    {
                        scoreToSet = _scores;
                    }
                    else
                    {
                        break; // end the loop if it finds one that is 0 (since they're already in order it is safe to stop when finding an empty time)
                    }
                }
                if(scoreToSet == 0)
                {
                    m_HighscoreText.gameObject.SetActive(false);
                }
                m_HighscoreText.text = string.Format("Time to beat:\n{0:D2}:{1:D2}", (scoreToSet / 60), (scoreToSet % 60));

                //if (OneTankLeft() == true || IsPlayerDead() == true)
                //{
                //    isGameOver = true;
                //}
                if (OneTankLeft() == true)
                {
                    isGameOver = true;
                }
                else if (IsPlayerDead() == true)
                {
                    isGameOver = true;
                }

                if (isGameOver == true) // Checks if the game is over
                {

                    // Sets the game state to being over & removes the timer
                    m_GameState = GameState.GameOver;
                    m_TimerText.gameObject.SetActive(false);
                    m_HighscoreText.gameObject.SetActive(false);




                    if (IsPlayerDead() == true)
                    {
                        m_MessageText.text = "TRY AGAIN";
                    } else
                    {
                        m_MessageText.text = "WINNER!";


                        // Sets temporary variables for the foreach loop below
                        int currentscore = seconds;
                        int progress = 0;

                        // Temporary separater for debugging
                        Debug.Log("===");
                        Debug.Log("===");
                        Debug.Log("===");

                        // Loops through all scores and tries to add the score in the correct position to keep it in order (highest score -> lowest score)
                        foreach (int _scores in scores)
                        {
                            if (_scores > currentscore || _scores == 0) // Checks if the current score is lower than the score it's checking
                            {
                                int tmpscore = _scores; // Sets the score being replaced as a temporary score
                                scores[progress] = currentscore; // Replaces the score with the current score
                                currentscore = tmpscore; // Sets the current score to the old one (does this as there may be another score this one can replace instead of being deleted)

                            }
                            Debug.Log(_scores); // Logs the scores (for debugging)
                            progress++; // Moves the progress counter up by 1
                        }

                        // Writes high scores to the highscores file
                        string[] result = Array.ConvertAll(scores, x => x.ToString());
                        File.WriteAllLines(Application.dataPath + "\\highscores", result);
                    }
                }
                break;
            case GameState.GameOver:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_gameTime = 0;
                    m_GameState = GameState.Playing;
                    m_MessageText.text = "";
                    m_TimerText.gameObject.SetActive(true);

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                }
                break;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == true)
            {
                numTanksLeft++;
            }
        }
        return numTanksLeft <= 1;
    }

    private bool IsPlayerDead()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == false)
            {
                if (m_Tanks[i].tag == "Player")
                {
                    return true;
                }
            }
        }

        return false;
    }
}
