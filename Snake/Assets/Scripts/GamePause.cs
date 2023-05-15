using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGamePause
{
    void Resume();  // Resume game from other classes - possible
    void Pause();   // Pause game from other classes - possible
}

public class GamePause : MonoBehaviour, IGamePause
{
    public static bool GameIsPaused = false;
    public GameObject gamePauseUI;
    public GameObject pauseButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        gamePauseUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        gamePauseUI.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}