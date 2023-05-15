using SnakeGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Snake : MonoBehaviour, ISnake
{
    public Transform segmentPrefab;
    public int initialSize = 1;

    public List<Transform> segments = new List<Transform>();
    private Vector2 _direction = Vector2.right;
    
    public delegate void FoodCollected(int snakeLength, FoodEnum foodType);
    public event FoodCollected OnFoodCollected;
    public delegate void AbilityCollected();
    public event AbilityCollected OnAbilityCollected; 
   
    public int length { get { return segments.Count; }}
    public GameObject Camera;

    public void Initialize()
    {
        ResetSnake();
    }

    private void Update()
    {
        // foreach (var segment in segments)
        // {
        //     segment.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f);
        // }
        SetDirection();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    public void SetDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) && Mathf.Approximately(_direction.y, -1) == false)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && Mathf.Approximately(_direction.y, 1) == false)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Mathf.Approximately(_direction.x, 1) == false)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && Mathf.Approximately(_direction.x, -1) == false)
        {
            _direction = Vector2.right;
        }
    }

    public void HandleMovement()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    public void ResetSnake()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++)
        {
            segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;

        //OnFoodCollected.Invoke(1, FoodEnum.Normal);
        
    }

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void Decrease()
    {
        Destroy(segments[segments.Count - 1].gameObject);
        segments.RemoveAt(segments.Count - 1);
    }

    public void SuperGrow()
    {
        for (int i = 0; i<2; i++)
        {
            Transform segment = Instantiate(this.segmentPrefab);
            segment.position = segments[segments.Count - 1].position;
            segments.Add(segment);
        }
    }

    public void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Food"){
            Food food = other.GetComponent<Food>();
            ConsumeFood(food);
            food.OnFoodCollected();

        } else if (other.tag == "Obstacle"){
            ResetSnake();
        }
    }

    public void ConsumeFood(Food food)
    {
        switch (food.type)
        {
            case FoodEnum.Normal:
                Grow();
                break;

            case FoodEnum.Poison:
                Decrease();
                break;

            case FoodEnum.Super:
                SuperGrow();
                break;
            
            //Create Ability Controller
            //AddAbility function add and depending on food.type add to the corresponding Counter
            //Also you could create Dictionary<FoodEnum, int> foodCounters, and then in that function AddAbility(foodType) you can write foodCounters[foodType]++;
            
            case FoodEnum.AbilityAdds5Foods:
                //event that calls for AbilityView counter increase
                break;
                
            case FoodEnum.AbilityRainbow:
                break;
            
            case FoodEnum.AbilityFoodMagnet:
                //abilityController.AddAbility(food.type);
                break;
        }

        OnFoodCollected?.Invoke(length, food.type);
    }
}