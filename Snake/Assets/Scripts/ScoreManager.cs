using System.Collections;
using System.Collections.Generic;
using SnakeGame;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IScoreManager
{
    void ChangeScore(int score, FoodEnum type);
}

public class ScoreManager : MonoBehaviour, IScoreManager
{
    public      TextMeshProUGUI         scoreText;
    public      Snake                   snake;

    public void Initialize()
    {
        snake = FindObjectOfType<Snake>();
        snake.OnFoodCollected += ChangeScore;

    }

    public void ChangeScore(int score, FoodEnum type)
    {
        scoreText.text = "Score: " + score;
    }
}