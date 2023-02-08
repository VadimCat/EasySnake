using System;
using Ji2Core.Core.Pools;
using Models;
using Views;

namespace Presenters
{
    public class LevelPresenter
    {
        public Level Model { get; }
        
        public event Action LevelCompleted;

        private FoodContainerView _foodContainerView;
        private ISnakeView _snakeView;
        
        private readonly SnakeGameView _snakeGameView;
        private readonly Pool<SnakePartView> _snakePartsPool;
        private readonly Pool<FoodView> _foodPartsPool;

        public LevelPresenter(Level level, SnakeGameView snakeGameView, Pool<SnakePartView> snakePartsPool,
            Pool<FoodView> foodPartsPool)
        {
            Model = level;
            _snakeGameView = snakeGameView;
            _snakePartsPool = snakePartsPool;
            _foodPartsPool = foodPartsPool;
        }

        public void BuildLevel()
        {
            _snakeGameView.Initialize(_snakePartsPool, _foodPartsPool, Model.Size);

            _foodContainerView = _snakeGameView.FoodContainerView;
            _snakeView = _snakeGameView.SpritesSnakeView;
            
            foreach (var food in Model.Food)
            {
                _snakeGameView.FoodContainerView.SpawnFood(food);
            }
            
            _snakeGameView.SpritesSnakeView.Move(Model.Snake);
        }

        public void StartLevel()
        {
            Model.FoodSpawn += _foodContainerView.SpawnFood;
            Model.FoodDeSpawn += _foodContainerView.DeSpawnFood;

            Model.SnakeMove += _snakeView.Move;

            Model.Complete += Complete;
            
            Model.Start();
        }

        private void Complete()
        {
            LevelCompleted?.Invoke();
        }
    }
}