using SnakeGame;

public interface ISnake
{
    void ResetSnake();
    void Grow();
    void Decrease();
    void SuperGrow();
    void ConsumeFood(Food food);
}