﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour {

    public Text goldLabel;
    public Text waveLabel;
    public GameObject[] nextWaveLabels;
    public bool gameOver = false;
    public Text healthLabel;
    public GameObject[] healthIndicator;



    // Use this for initialization
    void Start()
    {
        Gold = 1000;
        Wave = 0;
        Health = 5;
     //   GameWon = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
        }
    }

    private int wave;
    public int Wave
    {
        get { return wave; }
        set
        {
            wave = value;
            if (!gameOver)
            {
                for (int i = 0; i < nextWaveLabels.Length; i++)
                {
                    nextWaveLabels[i].GetComponent<Animator>().SetTrigger("nextWave");
                }
            }
            waveLabel.text = "WAVE: " + (wave + 1);
        }
    }

    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            // 1
            if (value < health)
            {
                Camera.main.GetComponent<CameraShake>().Shake();
            }
            // 2
            health = value;
            healthLabel.text = "HEALTH: " + health;
            // 3
            if (health <= 0 && !gameOver)
            {
                gameOver = true;
                GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
            }
            // 4 
            for (int i = 0; i < healthIndicator.Length; i++)
            {
                if (i < Health)
                {
                    healthIndicator[i].SetActive(true);
                }
                else
                {
                    Debug.Log("removing: " + i.ToString());
                    healthIndicator[i].SetActive(false);
                }
            }
        }
    }

    private bool gameWon;
    public bool GameWon
    {
        get { return gameWon; }
    }

}
