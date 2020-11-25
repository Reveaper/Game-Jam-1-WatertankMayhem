using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DiscordPresence;

public class MainMenuHandler : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game_Level1");
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("Menu_HowToPlay");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        Player.Instance.SavePlayer();
        SceneManager.LoadScene("Menu_Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
