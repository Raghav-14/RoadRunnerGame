using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "HighScore : " + ((int)PlayerPrefs.GetFloat("HighScore")).ToString();
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
}
