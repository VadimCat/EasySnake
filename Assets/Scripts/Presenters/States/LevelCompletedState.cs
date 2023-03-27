using System.Collections.Generic;
using System.Threading;
using Client;
using Cysharp.Threading.Tasks;
using Ji2.Ji2Core.Scripts.CommonCore;
using Ji2Core.Ads;
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
        private readonly IAdsProvider _adsProvider;

        public LevelCompletedState(StateMachine stateMachine, ScreenNavigator screenNavigator,
            AudioService audioService, LocalLeaderboard leaderboard, IAdsProvider adsProvider)
        {
            this.stateMachine = stateMachine;
            this.screenNavigator = screenNavigator;
            this.audioService = audioService;
            _leaderboard = leaderboard;
            _adsProvider = adsProvider;
        }

        public async UniTask Enter(LevelCompletedPayload payload)
        {
            if (payload.Level.ShowAds)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                var adTask = _adsProvider.InterstitialAsync(cancellationTokenSource.Token);

                var delayTask = UniTask.Delay(3000, cancellationToken: cancellationTokenSource.Token);

                await UniTask.WhenAny(adTask, delayTask);

                cancellationTokenSource.Cancel();
            }

            var screen = await screenNavigator.PushScreen<LevelCompletedScreen>();

            _leaderboard.Load();

            var isNewScore = _leaderboard.Records.Count == 0 || _leaderboard.Records.Count > 0 &&
                IsNewBestScoreMustBeShow(_leaderboard.Records[0], payload.Level.Score);
            
            screen.SetScoreObject(isNewScore);

            _leaderboard.TryAddRecord("You", payload.Level.Score);

            screen.ShowRecords(_leaderboard.Records, payload.Level.Score);
            screen.ClickNext += ClickNext;
        }

        private bool IsNewBestScoreMustBeShow((string, int) lastRecord, int score)
        {
            return score > lastRecord.Item2;
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