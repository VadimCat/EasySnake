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
            _gameScreen.PauseButton.gameObject.SetActive(false);
            _gameScreen.PlayButton.gameObject.SetActive(false);
            _gameScreen.ShowTextTip("Tap to start!");
            _gameScreen.ShowTextTip("TAP TO START");
            return UniTask.CompletedTask;
        }
        
        public UniTask Exit()
        {
            _gameScreen.ShowTextTip("TAP TO TURN");
            _gameScreen.HideTextTip(3);
            return UniTask.CompletedTask;
        }
    }
}