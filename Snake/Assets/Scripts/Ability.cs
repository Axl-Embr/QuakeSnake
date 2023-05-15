using System;
using SnakeGame;
using UnityEditor.Experimental;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public interface IAbilityService
    {
        
    }

    public class Ability : MonoBehaviour
    {
        public Food food;
        public AbilityView view;
        public int count;
        private Snake _snake;
        public bool rainbowAbilityActive;
        private float _rainbowAbilityStartTime;
        private float _foodMagnetAbilityStartTime;
        private float _foodMagnetAbilityDuration = 5f;
        private float _rainbowAbilityDuration = 5f;
        private bool _foodMagnetAbilityActive;
        private Food[] foodsArr;
        public float magnetSpeed = 1f;
        //speed of segments color changing custom add 
        
        void OnEnable()
        {
            _snake = FindObjectOfType<Snake>();
            view.use.interactable = count > 0;
        }

        void Update()
        {
            if (rainbowAbilityActive)
            {
                MakeSnakeRainbow();
            }

            if (_foodMagnetAbilityActive)
            {
                MagnetFoodTowardsSnake();
            }
        }

        public void Use(FoodEnum foodEnum)
        {
            switch (foodEnum)
            {
                case FoodEnum.AbilityAdds5Foods:
                    for (int i = 0; i < 5; i++)
                    {
                        Food foodAgain = Instantiate(food);
                        foodAgain.Initialize();
                    }
                    
                    break;
                
                case FoodEnum.AbilityRainbow:
                    rainbowAbilityActive = true;
                    _rainbowAbilityStartTime = Time.time;
                    break;
                
                case FoodEnum.AbilityFoodMagnet:
                    _foodMagnetAbilityActive = true;
                    _foodMagnetAbilityStartTime = Time.time;
                    break;
            }

            if (count > 0)
            {
                count -= 1;
                if (count == 0)
                {
                    view.use.interactable = false;
                    view.count.text = count.ToString();
                }
            }
        }
        
        public void OnAbilityCollected()
        {
            count += 1;
            if (view.use.interactable == false)
            {
                view.use.interactable = true;
            }
            view.count.text = count.ToString();
        }

        void MakeSnakeRainbow()
        {
            if (Time.time > _rainbowAbilityStartTime + _rainbowAbilityDuration)
            {
                rainbowAbilityActive = false;
                foreach (var segment in _snake.segments)
                {
                    segment.GetComponent<SpriteRenderer>().color = Color.white;
                }
                return;
            }
            
            foreach (var segment in _snake.segments)
            {
                segment.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f);
            }
        }

        void MagnetFoodTowardsSnake()
        {
            //find all foods on scene and add to ARRAY
            foodsArr ??= FindObjectsOfType<Food>();
            
            if (Time.time > _foodMagnetAbilityStartTime + _foodMagnetAbilityDuration)
            {
                _foodMagnetAbilityActive = false;
                foreach (Food food in foodsArr)
                {
                    //move all foods to closest grid (round number)
                    var position = food.transform.position;
                    position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0.0f);
                    food.transform.position = position;
                }

                foodsArr = null;
                return;
            }
            
            //Move each food towards snake
            foreach (Food food in foodsArr)
            {
                Vector3 distanceToSnake = _snake.segments[0].position - food.transform.position; //vector from food to snake
                distanceToSnake = distanceToSnake.normalized * magnetSpeed * Time.deltaTime;
                
                food.transform.position += distanceToSnake;
            }
        }
    }
}