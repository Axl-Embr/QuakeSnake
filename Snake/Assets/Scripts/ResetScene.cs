using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public interface IResetScene{
    void Reset();   //Reset game from other classes - possible
}

public class ResetScene : MonoBehaviour, IResetScene
{
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}