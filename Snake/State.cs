using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public enum StateStatus
    {
        Playing,
        Won,
        Lost
    }

    internal class State
    {
        public Level level;
        public Snake snake;
        public Food food;
        public StateStatus status;


        public State(StateStatus status, Level level, Snake snake, Food food)
        {
            this.status = status;
            this.level = level;
            this.snake = snake;
            this.food = food;
        }

        public static State Create(string levelPlan, char wallChar, Vector2D snakeHeadPos)
        {
            Level level = Level.FromString(levelPlan, wallChar);
            Snake snake = Snake.Create(snakeHeadPos);
            return new State(StateStatus.Playing, level, snake, Food.Spawn(snake, level));
        }
        public static State Create(Level level, Vector2D snakeHeadPos)
        {
            Snake snake = Snake.Create(snakeHeadPos);
            return new State(StateStatus.Playing, level, snake, Food.Spawn(snake, level));
        }

        public State Update(ref List<ConsoleKey> keys)
        {
            State newState = snake.Update(this, ref keys);
            newState = food.Update(newState);
            if (newState.status == StateStatus.Lost)
                return new State(StateStatus.Lost, level, snake, food);
            return newState;
        }
    }
}
