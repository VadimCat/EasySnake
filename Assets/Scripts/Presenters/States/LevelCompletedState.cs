using System.Collections.Generic;
using Client;
using Cysharp.Threading.Tasks;
using Ji2.Ji2Core.Scripts.CommonCore;
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
        private readonly AudioService audioService;
        private readonly LocalLeaderboard _leaderboard;

        public LevelCompletedState(StateMachine stateMachine, ScreenNavigator screenNavigator,
            AudioService audioService, LocalLeaderboard leaderboard)
        {
            this.stateMachine = stateMachine;
            this.screenNavigator = screenNavigator;
            this.audioService = audioService;
            _leaderboard = leaderboard;
        }

        public async UniTask Enter(LevelCompletedPayload payload)
        {
            var screen = await screenNavigator.PushScreen<LevelCompletedScreen>();
            _leaderboard.Load();
            var oldRecord = _leaderboard.Records;
            if (payload.Level.Score != 0)
                _leaderboard.AddRecord("You", payload.Level.Score);
            screen.ShowRecords(_leaderboard.Records, oldRecord, payload.Level.Score);
            screen.ClickNext += ClickNext;
        }

        private void ClickNext()
        {
            audioService.PlaySfxAsync(SoundNamesCollection.ButtonTap);

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