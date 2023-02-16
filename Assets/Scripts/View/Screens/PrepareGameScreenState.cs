using Cysharp.Threading.Tasks;
using Ji2Core.Core.States;

namespace Views.Screens
{
    public class PrepareGameScreenState : IState
    {
        private readonly GameScreen _gameScreen;

        public PrepareGameScreenState(GameScreen gameScreen)
        {
            _gameScreen = gameScreen;
        }

        public UniTask Enter()
        {
            _gameScreen.Overlay.gameObject.SetActive(false);
            _gameScreen.PauseButton.gameObject.SetActive(true);
            _gameScreen.PauseButton.interactable = false;
            _gameScreen.PlayButton.gameObject.SetActive(false);
            _gameScreen.ShowHandTip(true);
            _gameScreen.ShowTextTip("Tap to start!");
            return UniTask.CompletedTask;
        }
        
        public UniTask Exit()
        {
            _gameScreen.ShowHandTip(false);
            _gameScreen.ShowTextTip("Tap to turn!");
            _gameScreen.HideTextTip(5);
            return UniTask.CompletedTask;
        }
    }
}