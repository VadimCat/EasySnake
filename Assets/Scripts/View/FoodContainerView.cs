using System.Collections.Generic;
using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class FoodContainerView
    {
        private readonly Pool<FoodView> _foodPool;
        private readonly PositionProvider _positionProvider;
        private Dictionary<Vector2Int, FoodView> _foodViews = new();

        public FoodContainerView(Pool<FoodView> foodPool, PositionProvider positionProvider)
        {
            _foodPool = foodPool;
            _positionProvider = positionProvider;
        }

        public void SpawnFood(Vector2Int pos)
        {
            var newFood = _foodPool.Spawn(_positionProvider.GetPoint(pos) + Vector3.back);
            newFood.transform.localScale = new Vector3(_positionProvider._cellSize, _positionProvider._cellSize, 1);
            
            _foodViews.Add(pos, newFood);
        }

        public void DeSpawnFood(Vector2Int pos)
        {
            var food = _foodViews[pos];
            _foodPool.DeSpawn(food);
            _foodViews.Remove(pos);
        }

        public void Destroy()
        {
            foreach (var foodView in _foodViews.Values)
            {
                _foodPool.DeSpawn(foodView);
            }

            _foodViews = null;
        }
    }
}