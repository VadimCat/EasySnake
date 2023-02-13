using Client;
using Cysharp.Threading.Tasks;
using Facebook.Unity;
using Ji2.CommonCore.SaveDataContainer;
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


        public InitialState(StateMachine stateMachine, ScreenNavigator screenNavigator,
            ISaveDataContainer saveDataContainer)
        {
            this.stateMachine = stateMachine;
            this.screenNavigator = screenNavigator;
            this.saveDataContainer = saveDataContainer;
        }

        public async UniTask Exit()
        {
            await screenNavigator.CloseScreen<LoadingScreen>();
        }

        public async UniTask Enter()
        {
            var facebookTask = LoadFb();

            saveDataContainer.Load();
            // levelsLoopProgress.Load();

            await screenNavigator.PushScreen<LoadingScreen>();
            await facebookTask;

            float fakeLoadingTime = 3;
#if !UNITY_EDITOR
            fakeLoadingTime = 5;
#endif
            stateMachine.Enter<LoadLevelState, LoadLevelStatePayload>(
                new LoadLevelStatePayload(fakeLoadingTime));
        }

        private async UniTask LoadFb()
        {
// #if UNITY_EDITOR
//             await UniTask.CompletedTask;
//             Debug.LogWarning("FB IS NOT SETTED");
// #else 

            var taskCompletionSource = new UniTaskCompletionSource<bool>();
            FB.Init(() => OnFbInitComplete(taskCompletionSource));

            await taskCompletionSource.Task;
            if (!FB.IsInitialized)
            {
                FB.ActivateApp();
            }
// #endif
        }

        private void OnFbInitComplete(UniTaskCompletionSource<bool> uniTaskCompletionSource)
        {
            uniTaskCompletionSource.TrySetResult(FB.IsInitialized);
        }
    }
}