using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;
using Cysharp.Threading.Tasks;
using Ji2.Ji2Core.Scripts.CommonCore;
using Ji2.Models;
using Ji2Core.Core.Audio;
using Ji2Core.Core.Pools;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Models;
using UnityEngine;
using Views;
using Views.Screens;
using Random = UnityEngine.Random;

namespace Presenters
{
    public class LevelPresenter
    {
        public Level Model { get; }
        public GameScreen GameScreen => _gameScreen;

        public event Action LevelCompleted;

        private FoodContainerView _foodContainerView;
        private ISnakeView _snakeView;

        private SnakeGameView _snakeGameView;
        private readonly Pool<SnakePartView> _snakePartsPool;
        private readonly Pool<FoodView> _foodPartsPool;
        private readonly ScreenNavigator _screenNavigator;
        private readonly LocalLeaderboard _leaderboard;
        private readonly AudioService _audioService;

        private GameScreen _gameScreen;
        private StateMachine _screenStateMachine;
        private PositionProvider _positionProvider;
        private Head _head;

        public LevelPresenter(Level level, SnakeGameView snakeGameView, Pool<SnakePartView> snakePartsPool,
            Pool<FoodView> foodPartsPool, ScreenNavigator screenNavigator, LocalLeaderboard leaderboard,
            AudioService audioService)
        {
            Model = level;
            _snakeGameView = snakeGameView;
            _snakePartsPool = snakePartsPool;
            _foodPartsPool = foodPartsPool;
            _screenNavigator = screenNavigator;
            _leaderboard = leaderboard;
            _audioService = audioService;
            _head = snakeGameView.Head;
        }

        public void BuildLevel()
        {
            _positionProvider = new PositionProvider(Model.Size, _screenNavigator.Size, _screenNavigator.ScaleFactor);
            _snakeGameView.Initialize(_snakePartsPool, _foodPartsPool, Model.Size, _positionProvider);

            _foodContainerView = _snakeGameView.FoodContainerView;
            _snakeView = _snakeGameView.SpritesSnakeView;

            foreach (var food in Model.Food)
            {
                _snakeGameView.FoodContainerView.SpawnFood(food);
            }

            _snakeGameView.SpritesSnakeView.Move(Model.Snake);
        }

        public void PrepareStart()
        {
            _gameScreen = (GameScreen)_screenNavigator.CurrentScreen;
            _screenStateMachine = _gameScreen.GetStateMachine();
            
            CooldownPresenter cooldownPresenter = new CooldownPresenter(Model.Cooldown, _gameScreen.CooldownView);

            Model.FoodSpawn += _foodContainerView.SpawnFood;
            Model.FoodDeSpawn += HandleFoodDespawn;
            Model.SnakeMove += HandleSnakeMove;
            Model.Complete += Complete;
            Model.ScoreUpdate += HandleScoreUpdate;
            Model.DirectionChange += OnDirectionChange;

            Model.State.OnValueChanged += HandleStateChanged;

            _gameScreen.FieldClick += Model.HandleFieldClick;
            _gameScreen.PauseClick += Model.HandlePauseClick;
            _gameScreen.PlayClick += Model.HandlePlayClick;

            SetHighScore();
            
            _screenStateMachine.Enter<PrepareGameScreenState>();
            if (_leaderboard.GetHighRecord() == 0) _gameScreen.HideHighRecord();
        }

        private void OnDirectionChange(Vector2Int obj)
        {
            _audioService.PlaySfxAsync(SoundNamesCollection.ChangeDirection);
        }

        private void HandleSnakeMove(IReadOnlyList<Vector2Int> positions)
        {
            _snakeView.Move(positions);
            _head.ChangeDirection(Model.Direction);
        }

        private void HandleFoodDespawn(Vector2Int position)
        {
            _foodContainerView.DeSpawnFood(position);
            _head.SwitchState(HeadState.Eat);
            _snakeView.EatAnimation();
        }

        private void HandleScoreUpdate(int score)
        {
            _audioService.PlaySfxAsync(SoundNamesCollection.EatFood).Forget();

            var pos = _positionProvider.GetPoint(Model.Snake[0]) +
                      new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));

            _gameScreen.ShowPointsTip(pos).Forget();
            _gameScreen.SetScore(score);
            HandleHighScoreUpdate(score);
        }

        private void HandleHighScoreUpdate(int score)
        {
            var highRecord = _leaderboard.GetHighRecord();
            if(highRecord == 0) return;
            if(highRecord < score)
                _gameScreen.SetHighScore(score);
        }

        private void HandleStateChanged(GameState newState, GameState prevState)
        {
            _audioService.PlaySfxAsync(SoundNamesCollection.ButtonTap);
            switch (newState)
            {
                case GameState.Prepare:
                    _screenStateMachine.Enter<PrepareGameScreenState>();
                    break;
                case GameState.Game:
                    _screenStateMachine.Enter<PlayingGameScreenState>();
                    break;
                case GameState.Pause:
                    _screenStateMachine.Enter<PauseGameScreenState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private async void Complete()
        {
            Dispose();

            _audioService.PlaySfxAsync(SoundNamesCollection.SnakeCollision).Forget();
            _head.SwitchState(HeadState.Collision);
            await UniTask.Delay(1000);
            _audioService.PlaySfxAsync(SoundNamesCollection.WinScreenShow).Forget();

            LevelCompleted?.Invoke();

            _snakeGameView = null;
            _foodContainerView = null;
            _snakeView = null;
        }

        private void Dispose()
        {
            Model.FoodSpawn -= _foodContainerView.SpawnFood;
            Model.FoodDeSpawn -= HandleFoodDespawn;
            Model.SnakeMove -= HandleSnakeMove;
            Model.Complete -= Complete;
            Model.ScoreUpdate -= HandleScoreUpdate;
            Model.DirectionChange -= OnDirectionChange;

            Model.State.OnValueChanged -= HandleStateChanged;

            _gameScreen.FieldClick -= Model.HandleFieldClick;
            _gameScreen.PauseClick -= Model.HandlePauseClick;
            _gameScreen.PlayClick -= Model.HandlePlayClick;
        }

        private void SetHighScore()
        {
            var record = _leaderboard.GetHighRecord();
            _gameScreen.SetHighScore(record);
        }
    }
}