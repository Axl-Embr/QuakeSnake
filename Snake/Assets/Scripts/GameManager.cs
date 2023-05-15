using Assets.Scripts;
using SnakeGame;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public interface IGameManager
{
    void ChangeBackground(int snakeLength);
    void CameraRotate();
}

public class GameManager : MonoBehaviour, IGameManager
{
    public      Snake               snake;
    public      Food                food;
    public      ScoreManager        scoreManager;
    public      Camera              cam;
    public      Screen              screen;
    public      int                 foodNum = 2;
    
    //Camera Settings
    public      CameraController    cameraController;
    public      Vector3             cam_axis = Vector3.forward * -90f;
    public      float               cam_angle = 90f;
    public      float               duration = 8.0f;
    
    public Button pauseButton;
    public GamePause gamePause;
    public Button resumeButton;

    public Ability abilityAdds5Foods;
    public Ability abilityRainbow;
    public Ability abilityFoodMagnet;

    //Connecting IAbilityService To Game
    //Settings VS Constants
    //Settings - pre-game, Constants - always

    public void Initialize()
    {
        scoreManager.Initialize();
        snake.Initialize();
        food.Initialize();

        abilityAdds5Foods.view.use.onClick.AddListener(() => abilityAdds5Foods.Use(FoodEnum.AbilityAdds5Foods));
        abilityRainbow.view.use.onClick.AddListener(() => abilityRainbow.Use(FoodEnum.AbilityRainbow));
        abilityFoodMagnet.view.use.onClick.AddListener(() => abilityFoodMagnet.Use(FoodEnum.AbilityFoodMagnet));
        
        pauseButton.onClick.AddListener(OnPauseButtonClick);
        resumeButton.onClick.AddListener(OnResumeButtonClick);

        snake.OnFoodCollected += OnFoodCollected;

        //snake.OnFoodCollected += OnChangeBackground;
        //snake.OnFoodCollected += OnCameraRotate;

        for (int i = 0; i < foodNum; i++)
        {
            Food foodAgain = Instantiate(food);
            foodAgain.Initialize();
        }
    }

    private void OnAbilityUseButtonClick(FoodEnum foodType)
    {
        //if counter <= 0 then button is not interactable (property)
        abilityAdds5Foods.Use(FoodEnum.AbilityAdds5Foods);
    }

    private void OnFoodCollected(int snakeLength, FoodEnum type)
    {
        if (snakeLength % 5 == 0)
        {
            CameraRotate();
        }

        ChangeBackground(snakeLength);

        switch (type)
        {
            case FoodEnum.AbilityAdds5Foods:
                abilityAdds5Foods.OnAbilityCollected();
                break;
                
            case FoodEnum.AbilityRainbow:
                abilityRainbow.OnAbilityCollected();
                break;
            
            case FoodEnum.AbilityFoodMagnet:
                abilityFoodMagnet.OnAbilityCollected();
                break;
        }
    }

    public void ChangeBackground(int snakeLength)
    {
        cam.backgroundColor = new Color(Random.Range(0, snakeLength/20f), Random.Range(0, snakeLength / 20f), Random.Range(0, snakeLength / 20f), 1);
    }

    public void CameraRotate()
    {
        Debug.Log("we are supposed to rotate now");
        cameraController.Rotate(cam_axis, cam_angle, duration);
    }

    public void OnPauseButtonClick()
    {
        gamePause.Pause();
    }

    public void OnResumeButtonClick()
    {
        gamePause.Resume();
    }
}