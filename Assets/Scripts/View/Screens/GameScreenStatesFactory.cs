using System;
using System.Collections.Generic;
using Ji2Core.Core.States;

namespace Views.Screens
{
    public class GameScreenStatesFactory : IStateFactory
    {
        private readonly GameScreen _gameScreen;

        public GameScreenStatesFactory(GameScreen gameScreen)
        {
            _gameScreen = gameScreen;
        }
        
        public Dictionary<Type, IExitableState> GetStates(StateMachine stateMachine)
        {
            return new Dictionary<Type, IExitableState>()
            {
                [typeof(PrepareGameScreenState)] = new PrepareGameScreenState(_gameScreen),
                [typeof(PlayingGameScreenState)] = new PlayingGameScreenState(_gameScreen),
                [typeof(PauseGameScreenState)] = new PauseGameScreenState(_gameScreen)
            };
        }
    }
}