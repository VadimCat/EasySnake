using Cysharp.Threading.Tasks;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Ji2Core.Scripts.CommonCore;
using Ji2Core.Core;
using Ji2Core.Core.Pools;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Models;
using UI.Background;
using UI.Screens;
using UnityEngine;
using Views;
using Analytics = Ji2.Models.Analytics.Analytics;

namespace Presenters.States
{
    public class LoadLevelState : IPayloadedState<LoadLevelStatePayload>
    {
        private const string GAME_SCENE_NAME = "LevelScene";

        private readonly StateMachine stateMachine;
        private readonly SceneLoader sceneLoader;
        private readonly ScreenNavigator screenNavigator;
        private readonly Context context;
        private readonly BackgroundService backgroundService;
        private readonly LevelConfig _levelConfig;

        private LoadingScreen loadingScreen;
        private LevelData levelData;

        public LoadLevelState(Context context, StateMachine stateMachine, SceneLoader sceneLoader,
            ScreenNavigator screenNavigator, BackgroundService backgroundService, LevelConfig levelConfig)
        {
            this.context = context;
            this.stateMachine = stateMachine;
            this.sceneLoader = sceneLoader;
            this.screenNavigator = screenNavigator;
            this.backgroundService = backgroundService;
            _levelConfig = levelConfig;
        }

        public async UniTask Enter(LoadLevelStatePayload payload)
        {
            var sceneTask = sceneLoader.LoadScene(GAME_SCENE_NAME);
            loadingScreen = await screenNavigator.PushScreen<LoadingScreen>();

            if (payload.FakeLoadingTime == 0)
            {
                sceneLoader.OnProgressUpdate += loadingScreen.SetProgress;
                await sceneTask;
                sceneLoader.OnProgressUpdate -= loadingScreen.SetProgress;
            }
            else
            {
                var progressBarTask = loadingScreen.AnimateLoadingBar(payload.FakeLoadingTime);
                await UniTask.WhenAll(sceneTask, progressBarTask);
            }

            var gamePayload = BuildLevel();

            stateMachine.Enter<GameState, GameStatePayload>(gamePayload);
        }

        private GameStatePayload BuildLevel()
        {
            var level = new Level(context.GetService<UpdateService>(), _levelConfig.Size, _levelConfig.Speed,
                context.GetService<Analytics>(), new LevelData(), context.SaveDataContainer);

            var snakeView = context.GetService<SnakeGameView>();

            LevelPresenter levelPresenter =
                new LevelPresenter(level, snakeView, context.GetService<Pool<SnakePartView>>(),
                    context.GetService<Pool<FoodView>>(), context.ScreenNavigator,
                    new LocalLeaderboard(context.SaveDataContainer));

            levelPresenter.BuildLevel();

            return new GameStatePayload
            {
                levelPresenter = levelPresenter
            };
        }

        public async UniTask Exit()
        {
            await screenNavigator.CloseScreen<LoadingScreen>();
        }
    }

    public class LoadLevelStatePayload
    {
        public readonly float FakeLoadingTime;

        public LoadLevelStatePayload(float fakeLoadingTime = 0)
        {
            FakeLoadingTime = fakeLoadingTime;
        }
    }
}