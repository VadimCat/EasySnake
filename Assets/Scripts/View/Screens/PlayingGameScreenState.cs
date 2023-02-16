using Cysharp.Threading.Tasks;
using Ji2Core.Core.States;

namespace Views.Screens
{
    public class PlayingGameScreenState : IState
    {
        private readonly GameScreen _gameScreen;

        public PlayingGameScreenState(GameScreen gameScreen)
        {
            _gameScreen = gameScreen;
        }
        
        public UniTask Enter()
        {
            _gameScreen.Overlay.gameObject.SetActive(false);
            _gameScreen.PauseButton.gameObject.SetActive(true);
            _gameScreen.PauseButton.interactable = true;
            return UniTask.CompletedTask;
        }
        
        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}