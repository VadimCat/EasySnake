using Cysharp.Threading.Tasks;
using Ji2Core.Core.States;

namespace Views.Screens
{
    public class PauseGameScreenState : IState
    {
        private readonly GameScreen _gameScreen;

        public PauseGameScreenState(GameScreen gameScreen)
        {
            _gameScreen = gameScreen;
        }

        public UniTask Enter()
        {
            _gameScreen.Overlay.gameObject.SetActive(true);
            _gameScreen.PauseButton.gameObject.SetActive(false);
            _gameScreen.PlayButton.gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
        
        public UniTask Exit()
        {
            _gameScreen.Overlay.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}