using Client;
using Cysharp.Threading.Tasks;
using Facebook.Unity;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Presenters.Tutorial;
using Ji2Core.Ads;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using UI.Screens;

namespace Presenters.States
{
    public class InitialState : IState
    {
        private readonly StateMachine stateMachine;
        private readonly ScreenNavigator screenNavigator;
        private readonly LevelsLoopProgress levelsLoopProgress;
        private readonly ISaveDataContainer saveDataContainer;
        private readonly TutorialService _tutorialService;
        private readonly IAdsProvider _adsProvider;


        public InitialState(StateMachine stateMachine, ScreenNavigator screenNavigator,
            ISaveDataContainer saveDataContainer, TutorialService tutorialService, IAdsProvider adsProvider)
        {
            this.stateMachine = stateMachine;
            this.screenNavigator = screenNavigator;
            this.saveDataContainer = saveDataContainer;
            _tutorialService = tutorialService;
            _adsProvider = adsProvider;
        }

        public async UniTask Exit()
        {
            await screenNavigator.CloseScreen<LoadingScreen>();
        }

        public async UniTask Enter()
        {
            var facebookTask = LoadFb();
            var adsProviderTask = _adsProvider.InitializeAsync();
            saveDataContainer.Load();
            // levelsLoopProgress.Load();

            await screenNavigator.PushScreen<LoadingScreen>();
            await UniTask.WhenAll(facebookTask, adsProviderTask);

            float fakeLoadingTime = 3;
#if !UNITY_EDITOR
            fakeLoadingTime = 5;
#endif
            _tutorialService.TryRunSteps();
            
            stateMachine.Enter<LoadLevelState, LoadLevelStatePayload>(
                new LoadLevelStatePayload(fakeLoadingTime));
        }

        private async UniTask LoadFb()
        {
            var taskCompletionSource = new UniTaskCompletionSource<bool>();
            FB.Init(OnFbInitComplete);

            await taskCompletionSource.Task;
            if (!FB.IsInitialized)
            {
                FB.ActivateApp();
            }

            void OnFbInitComplete()
            {
                taskCompletionSource.TrySetResult(FB.IsInitialized);
            }
        }
    }
}