using SnakeGame;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public interface IFood
    {
        void RandomizePosition();
        void OnFoodCollected();
        void Respawn();
        FoodEnum GetRandomType();
    }

    public class Food : MonoBehaviour, IFood
    {
        public  BoxCollider2D                   spawnBounds;
        public  FoodEnum                        type;

        public  Color                           PoisonColor;
        public  Color                           SuperFoodColor;
        public  Color                           NormalColor;
        public  Color                           FirstAbilityColor;
        public Color SecondAbilityColor;
        public Color ThirdAbilityColor;
        public  Snake                           snake;

        private SpriteRenderer                  _spriteRenderer;

        private Dictionary <FoodEnum, Color>    _colorMap;


        public void Awake()
        {
            _colorMap = new Dictionary<FoodEnum, Color> { 
                { FoodEnum.Poison,  PoisonColor }, 
                { FoodEnum.Normal,  NormalColor }, 
                { FoodEnum.Super,   SuperFoodColor },
                { FoodEnum.AbilityAdds5Foods, FirstAbilityColor },
                { FoodEnum.AbilityRainbow, SecondAbilityColor },
                { FoodEnum.AbilityFoodMagnet, ThirdAbilityColor }
            };
            snake = FindObjectOfType<Snake>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize()
        {
            Respawn();
        }

        public void RandomizePosition()
        {
            Bounds bounds = this.spawnBounds.bounds; //bounds now stores boundaries

            float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

            this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
        }

        public void OnFoodCollected()
        {
                Respawn();
        }

        public void Respawn()
        {
            RandomizePosition();
            type = GetRandomType();
            Debug.Log(type);
            _spriteRenderer.color = _colorMap[type];
        }

        public FoodEnum GetRandomType()
        {
            Array values = Enum.GetValues(typeof(FoodEnum));

            if (snake.segments.Count <= 1)
            {
                List<FoodEnum> arrayWithoutPoison = new List<FoodEnum>();
                foreach (var value in values)
                {
                    FoodEnum food = (FoodEnum)value;
                    if (food != FoodEnum.Poison)
                    {
                        arrayWithoutPoison.Add(food);
                    }
                }

                return arrayWithoutPoison[UnityEngine.Random.Range(0, arrayWithoutPoison.Count)];
            }
            else
            {
                System.Random random = new System.Random();

                FoodEnum randomType = (FoodEnum)values.GetValue(random.Next(values.Length));

                return randomType;
            }
        }
    }
}