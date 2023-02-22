using System;
using System.Collections.Generic;
using System.Linq;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Models;
using Ji2.Models.Analytics;
using Ji2.Utils;
using Ji2Core.Core.Audio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    public class Level : LevelBase, IFixedUpdatable
    {
        private readonly UpdateService _updateService;
        private readonly float _speed;
        private int _score;
        private Vector2Int _direction = Vector2Int.right;
        private Vector2Int _nextDirection = Vector2Int.right;
        private float movement = 0;
        private readonly HashSet<Vector2Int> fieldPoints;
        private List<Vector2Int> snake;

        private Vector2Int min = new(-1, -1);
        private Vector2Int max = new(1, 1);

        public readonly Vector2Int Size;

        public readonly ReactiveProperty<GameState> State = new(GameState.Prepare);

        public IReadOnlyList<Vector2Int> Snake => snake.AsReadOnly();
        public event Action<IReadOnlyList<Vector2Int>> SnakeMove;


        private List<Vector2Int> food = new();
        public IReadOnlyList<Vector2Int> Food => food.AsReadOnly();

        public int Score => _score;

        public Vector2Int Direction => _direction;

        public float speedRate = 1;

        public event Action<Vector2Int> FoodSpawn;
        public event Action<Vector2Int> FoodDeSpawn;
        public event Action<int> ScoreUpdate;
        public event Action<Vector2Int> DirectionChange;

        public Level(UpdateService updateService, Vector2Int size, float speed, Analytics analytics,
            LevelData levelData, ISaveDataContainer saveDataContainer, AudioService audioService)
            : base(analytics, levelData, saveDataContainer)
        {
            _updateService = updateService;
            _speed = speed;
            Size = size;

            fieldPoints = new HashSet<Vector2Int>(size.x * size.y);
            snake = new() { new(size.x / 2, size.y / 2), new(size.x / 2 - 1, size.y / 2) };

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    fieldPoints.Add(new Vector2Int(i, j));
                }
            }

            SpawnFood();
        }

        private void Start()
        {
            _updateService.Add(this);
            State.Value = GameState.Game;
        }

        private void Pause()
        {
            _updateService.Remove(this);
            State.Value = GameState.Pause;
        }

        protected override void OnComplete()
        {
            base.OnComplete();

            LogAnalyticsLevelFinish();
            _updateService.Remove(this);
        }

        public void OnFixedUpdate()
        {
            movement += Time.deltaTime * _speed * speedRate;
            while (movement >= 1)
            {
                movement--;
                speedRate = 1;
                MoveSnake();
            }
        }

        private void TryChangeDirection()
        {
            var headPos = snake[0];
            var foodPos = food[0];

            var foodDirection = foodPos - headPos;
            foodDirection.Clamp(min, max);

            if ((foodDirection.x == 0 || foodDirection.y == 0) && CheckFreePathBetween(headPos, foodPos))
            {
                ChangeDirection(foodDirection);
            }
            else
            {
                var pathes = TryFindPathWave(headPos, foodPos);
                if (pathes.Count == 0)
                {
                    for (int i = 2; i < snake.Count; i++)
                    {
                        if (!Mathf.Approximately(Vector2Int.Distance(headPos, snake[i]), 1))
                        {
                            pathes = TryFindPathWave(headPos, snake[i]);
                        }
                    }
                }

                if (pathes.Count > 0)
                {
                    var direction = pathes.FirstOrDefault(el => el - headPos != _direction);
                    ChangeDirection(direction - headPos);
                }
            }
        }

        private bool CheckFreePathBetween(Vector2Int p1, Vector2Int p2)
        {
            var direction = (p2 - p1);
            direction.Clamp(min, max);
            var i = p1 + direction;
            while (i != p2)
            {
                if (snake.Contains(i))
                {
                    return false;
                }

                i += direction;
            }

            return true;
        }

        private List<Vector2Int> TryFindPathWave(Vector2Int start, Vector2Int target)
        {
            int[][] pathGrid = new int[Size.x][];
            for (int index = 0; index < Size.x; index++)
            {
                pathGrid[index] = new int[Size.y];
            }

            foreach (var parts in snake)
            {
                pathGrid[parts.x][parts.y] = int.MaxValue;
            }

            pathGrid[start.x][start.y] = 1;
            List<List<Vector2Int>> poses = new List<List<Vector2Int>>();

            Vector2Int checkPoint = new Vector2Int(-1, -1);

            poses.Add(new List<Vector2Int>() { start });

            int weight = 2;
            while (checkPoint != target && poses[^1].Count != 0)
            {
                var lastPointsList = poses[^1];
                var newPoints = new List<Vector2Int>();
                poses.Add(newPoints);

                foreach (var t in lastPointsList)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2Int direction = i switch
                        {
                            0 => Vector2Int.down,
                            1 => Vector2Int.up,
                            2 => Vector2Int.left,
                            3 => Vector2Int.right,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        checkPoint = t + direction;

                        if (checkPoint.x < 0 || checkPoint.x >= Size.x || checkPoint.y < 0 || checkPoint.y >= Size.y)
                        {
                            continue;
                        }

                        if (checkPoint == target)
                            break;

                        if (pathGrid[checkPoint.x][checkPoint.y] == 0)
                        {
                            pathGrid[checkPoint.x][checkPoint.y] = weight;
                            newPoints.Add(checkPoint);
                        }
                    }

                    if (checkPoint == target)
                        break;
                }

                if (checkPoint != target)
                    weight++;
            }


            List<Vector2Int> endpoints = new List<Vector2Int>();
            endpoints.Add(checkPoint);

            while (weight != 2)
            {
                weight--;

                var newPoints = new List<Vector2Int>();

                foreach (var point in endpoints)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2Int direction = i switch
                        {
                            0 => Vector2Int.down,
                            1 => Vector2Int.up,
                            2 => Vector2Int.left,
                            3 => Vector2Int.right,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        var prevPoint = point + direction;

                        if (prevPoint.x < 0 || prevPoint.x >= Size.x || prevPoint.y < 0 || prevPoint.y >= Size.y ||
                            newPoints.Contains(prevPoint) || newPoints.Contains(prevPoint))
                        {
                            continue;
                        }

                        if (pathGrid[prevPoint.x][prevPoint.y] == weight)
                        {
                            newPoints.Add(prevPoint);
                        }
                    }
                }

                endpoints = newPoints;
            }

            return endpoints;
        }

        private void ChangeDirection(Vector2Int direction)
        {
            if (_direction == direction)
            {
                speedRate = 2;
            }
            else
            {
                DirectionChange?.Invoke(direction);
            }

            _nextDirection = direction;
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
                OnComplete();
                return;
            }
            else if (snake[0].x == -1)
            {
                OnComplete();
                return;
            }
            else if (snake[0].y == Size.y)
            {
                OnComplete();
                return;
            }
            else if (snake[0].y == -1)
            {
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
                _score++;
                ScoreUpdate?.Invoke(_score);
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

        public void HandleFieldClick()
        {
            switch (State.Value)
            {
                case GameState.Prepare:
                    Start();
                    break;
                case GameState.Game:
                    CheckAnalyticsLevelStart();
                    
                    TryChangeDirection();
                    break;
                case GameState.Pause:
                    Start();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckAnalyticsLevelStart()
        {
            if (State.PrevValue == GameState.Prepare)
            {
                LogAnalyticsLevelStart();
            }
        }

        public void HandlePauseClick()
        {
            switch (State.Value)
            {
                case GameState.Game:
                    Pause();
                    break;
                case GameState.Prepare:
                case GameState.Pause:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void HandlePlayClick()
        {
            switch (State.Value)
            {
                case GameState.Pause:
                    Start();
                    break;
                case GameState.Prepare:
                case GameState.Game:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum GameState
    {
        Prepare,
        Game,
        Pause
    }
}