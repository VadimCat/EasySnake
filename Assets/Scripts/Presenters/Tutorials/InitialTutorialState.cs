using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ji2.Presenters.Tutorial;
using Ji2.UI;
using Ji2Core.Core.States;
using Models;
using UnityEngine;
using Views.Screens;
using GameState = Presenters.States.GameState;

namespace Presenters.Tutorials
{
    public class InitialTutorialState : ITutorialStep
    {
        private readonly StateMachine _stateMachine;
        private readonly TutorialPointerView _pointer;
        private LevelPresenter _presenter;
        private Level _model;
        private GameScreen _screen;
        private CancellationTokenSource _cancellationTokenSource;
        public string SaveKey => "InitialTutorialState";

        public event Action Completed;

        public InitialTutorialState(StateMachine stateMachine, TutorialPointerView pointer)
        {
            _stateMachine = stateMachine;
            _pointer = pointer;
        }

        public void Run()
        {
            _stateMachine.StateEntered += OnStateEnter;
        }

        private void OnStateEnter(IExitableState obj)
        {
            if (obj is GameState state)
            {
                _presenter = state.Payload.levelPresenter;
                _model = _presenter.Model;
                _screen = _presenter.GameScreen;

                ShowStartTip();
            }
        }

        private void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        private async void ShowStartTip()
        {
            _screen.TogglePauseButtonInteraction(false);
            PlayClickAnimation();


            await UniTask.WaitWhile(() => _model.State.Value == Models.GameState.Prepare);

            _screen.ToggleFieldButtonInteraction(false);
            _pointer.Hide();
            Cancel();

            Vector2Int pos1 = new(_model.Size.x / 4 * 3, _model.Size.y / 2);
            await UniTask.WaitUntil(() => _model.Snake[0] == pos1);

            EnableTip();

            await _screen.AwaitFieldClick();
            
            DisableTip();

            await UniTask.WaitUntil(() => _model.Snake[0].y == _model.Food[0].y);

            EnableTip();

            await _screen.AwaitFieldClick();

            _model.TogglePause(false);
            _pointer.Hide();
            Cancel();
            
            _screen.TogglePauseButtonInteraction(true);
            
            _stateMachine.StateEntered -= OnStateEnter;
            Completed?.Invoke();
        }

        private void EnableTip()
        {
            _model.TogglePause(true);
            PlayClickAnimation();
            _screen.ToggleFieldButtonInteraction(true);
        }

        private void DisableTip()
        {
            _model.TogglePause(false);
            _pointer.Hide();
            Cancel();
            _screen.ToggleFieldButtonInteraction(false);
        }

        private void PlayClickAnimation()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _pointer.PlayClickAnimation(new Vector3(0, -1), _cancellationTokenSource.Token);
        }
    }
}