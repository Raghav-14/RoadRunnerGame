using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    private float score = 0.0f;
    public Text scoreText;

    public Text coinText;
    private int coinCount = 0;

    public DeathMenu deathMenu;

    private float difficultyLevel = 1.0f;
    private int maxDifficultyLevel = 10;
    private int scoreToNextLevel = 10;

    private bool isDead = false;
    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;
        if(score>scoreToNextLevel)
        {
            LevelUp();
        }
        score += Time.deltaTime * 2.0f;
        scoreText.text = ((int)score).ToString(); 
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Coin")
        {
            Destroy(col.gameObject);
            coinCount = coinCount + 1;
            coinText.text = coinCount.ToString();
        }
    }


    void LevelUp()
    {
        if (difficultyLevel == maxDifficultyLevel)
            return;
        scoreToNextLevel *= 2;
        difficultyLevel++;

        GetComponent<PlayerController>().setSpeedToNextLevel(difficultyLevel);
    }

   public void OnDeath()
    {
        isDead = true;
        PlayerPrefs.SetFloat("HighScore", score);
        deathMenu.ToggleMainMenuForScore(score , coinCount);
    }
}
