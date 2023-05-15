using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GameManager gameManager;

    void Start()
    {
        gameManager.Initialize();
    }
}