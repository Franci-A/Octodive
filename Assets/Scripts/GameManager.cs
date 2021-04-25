using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject GameoverUI;
    public GameObject scoreText;
    public AudioMixerSnapshot soundON;
    public AudioMixerSnapshot soundOff;
    private void Start()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            soundOff.TransitionTo(.2f);
        }
        else
        {
            soundON.TransitionTo(.2f);
        }
    }
    public void LoadLevel(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame() {
        Time.timeScale = 0;

    }

    public void UnPauseGame() {
        Time.timeScale = 1;

    }

    public void GameOver(int score)
    {
        GameoverUI.SetActive(true);
        scoreText.GetComponent<TextMeshProUGUI>().text ="Your score is : " + score;
        Time.timeScale = 0;
    }

    public void SoundToggle()
    {
        
        if (PlayerPrefs.GetInt("Sound") == 0){
            soundOff.TransitionTo(.2f);
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            soundON.TransitionTo(.2f);
            PlayerPrefs.SetInt("Sound", 0);
        }
    }
}
