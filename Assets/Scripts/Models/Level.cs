using System;
using System.Collections.Generic;
using System.Linq;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Models;
using Ji2.Models.Analytics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    public class Level : LevelBase, IFixedUpdatable
    {
        private readonly UpdateService _updateService;
        public readonly Vector2Int Size;
        private readonly float _speed;
        private int _score;

        private Vector2Int _direction = Vector2Int.right;
        private Vector2Int _nextDirection = Vector2Int.right;

        private float movement = 0;

        private readonly HashSet<Vector2Int> fieldPoints;

        private List<Vector2Int> snake = new() { new(2, 5), new(1, 5), new(0, 5) };
        public IReadOnlyList<Vector2Int> Snake => snake.AsReadOnly();
        public event Action<IReadOnlyList<Vector2Int>> SnakeMove;


        private List<Vector2Int> food = new();
        public IReadOnlyList<Vector2Int> Food => food.AsReadOnly();

        public event Action<Vector2Int> FoodSpawn;
        public event Action<Vector2Int> FoodDeSpawn;

        public Level(UpdateService updateService, Vector2Int size, float speed, Analytics analytics, LevelData levelData,
            ISaveDataContainer saveDataContainer) : base(analytics, levelData, saveDataContainer)
        {
            _updateService = updateService;
            _speed = speed;
            Size = size;

            fieldPoints = new HashSet<Vector2Int>(size.x * size.y);

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    fieldPoints.Add(new Vector2Int(i, j));
                }
            }

            SpawnFood();
        }

        public void Start()
        {
            _updateService.Add(this);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            
            _updateService.Remove(this);
        }

        public void OnFixedUpdate()
        {
            movement += Time.deltaTime * _speed;
            HandleInput();
            while (movement >= 1)
            {
                movement--;
                MoveSnake();
            }
        }

        private void HandleInput()
        {
            if (Input.GetAxis("Horizontal") > .2f && _direction != Vector2Int.left)
            {
                _nextDirection = Vector2Int.right;
                // movement = 1;
            }
            else if (Input.GetAxis("Horizontal") < -.2f && _direction != Vector2Int.right)
            {
                _nextDirection = Vector2Int.left;
                // movement = 1;
            }
            else if (Input.GetAxis("Vertical") > .2f && _direction != Vector2Int.down)
            {
                _nextDirection = Vector2Int.up;
                // movement = 1;
            }
            else if (Input.GetAxis("Vertical") < -.2f && _direction != Vector2Int.up)
            {
                _nextDirection = Vector2Int.down;
                // movement = 1;
            }
        }

        private void MoveSnake()
        {
            var tale = snake.Last();

            for (var i = snake.Count - 1; i >= 1; i--)
            {
                snake[i] = snake[i - 1];
            }

            snake[0] += _nextDirection;
            _direction = _nextDirection;
            if (snake[0].x == Size.x)
            {
                // snake[0] = new Vector2Int(0, snake[0].y);
                OnComplete();
                return;
            }
            else if (snake[0].x == -1)
            {
                // snake[0] = new Vector2Int(Size.x - 1, snake[0].y);
                OnComplete();
                return;
            }
            else if (snake[0].y == Size.y)
            {
                // snake[0] = new Vector2Int(snake[0].x, 0);
                OnComplete();
                return;
            }
            else if (snake[0].y == -1)
            {
                // snake[0] = new Vector2Int(snake[0].x, Size.y - 1);
                OnComplete();
                return;
            }

            
            SnakeMove?.Invoke(snake.AsReadOnly());
            
            var collisionIndex = snake.FindLastIndex(el => el == snake[0]);
            if (collisionIndex != -1 && collisionIndex != 0)
            {
                OnComplete();
                return;
            }
            
            if (food.Contains(snake[0]))
            {
                snake.Add(tale);
                food.Remove(snake[0]);
                FoodDeSpawn?.Invoke(snake[0]);
                SpawnFood();
            }
        }

        private void SpawnFood()
        {
            var spawnPoints = fieldPoints.Except(snake).Except(food).ToArray();
            var newPos = spawnPoints[Random.Range(0, spawnPoints.Length)];
            food.Add(newPos);
            FoodSpawn?.Invoke(newPos);
        }
    }
}