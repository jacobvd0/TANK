using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject[] m_Tanks;
    public TextMeshProUGUI m_MessageText;
    public TextMeshProUGUI m_TimerText;

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
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(false);
        }

        m_TimerText.gameObject.SetActive(false);
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

                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                m_TimerText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));

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

                if (isGameOver == true)
                {
                    m_GameState = GameState.GameOver;
                    m_TimerText.gameObject.SetActive(false);
                    int currentscore = seconds;
                    int progress = 0;
                    foreach (int _scores in scores)
                    {
                        if (_scores < currentscore)
                        {
                            int tmpscore = _scores;
                            scores[progress] = currentscore;
                            currentscore = tmpscore;

                        } else
                        {
                            scores[progress] = currentscore;
                            currentscore = 0;
                        }

                        Debug.Log(_scores);
                        progress++;
                    }

                    if (IsPlayerDead() == true)
                    {
                        m_MessageText.text = "TRY AGAIN";
                    } else
                    {
                        m_MessageText.text = "WINNER!";
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
