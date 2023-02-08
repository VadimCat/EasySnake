using Client;
using Cysharp.Threading.Tasks;
using Ji2Core.Core.Audio;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Models;
using Views;

namespace Presenters.States
{
    public class LevelCompletedState : IPayloadedState<LevelCompletedPayload>
    {
        private readonly StateMachine stateMachine;
        private readonly ScreenNavigator screenNavigator;
        private readonly LevelsLoopProgress levelsLoopProgress;
        // private readonly LevelsConfig levelsConfig;
        private readonly AudioService audioService;
        // private readonly LevelResultViewConfig levelResultViewConfig;

        public LevelCompletedState(StateMachine stateMachine, ScreenNavigator screenNavigator,
            AudioService audioService)
        {
            this.stateMachine = stateMachine;
            this.screenNavigator = screenNavigator;
            this.audioService = audioService;
        }

        public async UniTask Enter(LevelCompletedPayload payload)
        {
            var screen = await screenNavigator.PushScreen<LevelCompletedScreen>();
            
            screen.ClickNext += ClickNext;
            // screen.ClickRetry += OnClickRetry;
        }

        private void OnClickRetry()
        {
            var levelData = levelsLoopProgress.GetRetryLevelData();
            stateMachine.Enter<LoadLevelState, LoadLevelStatePayload>(new LoadLevelStatePayload(1f));
        }

        private void ClickNext()
        {
            audioService.PlaySfxAsync(AudioClipName.ButtonFX);
            var levelData = levelsLoopProgress.GetNextLevelData();
            stateMachine.Enter<LoadLevelState, LoadLevelStatePayload>(new LoadLevelStatePayload(1f));
        }

        public async UniTask Exit()
        {
            await screenNavigator.CloseScreen<LevelCompletedScreen>();
        }
    }


    public class LevelCompletedPayload
    {
        public Level Level;
    }
}