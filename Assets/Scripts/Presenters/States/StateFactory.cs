using System;
using System.Collections.Generic;
using Ji2.Ji2Core.Scripts.CommonCore;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core;
using Ji2Core.Core.Audio;
using Ji2Core.Core.States;
using UI.Background;

namespace Presenters.States
{
    public class StateFactory : IStateFactory
    {
        private readonly Context context;

        public StateFactory(Context context)
        {
            this.context = context;
        }

        public Dictionary<Type, IExitableState> GetStates(StateMachine stateMachine)
        {
            var screenNavigator = context.ScreenNavigator;
            var dict = new Dictionary<Type, IExitableState>();

            dict[typeof(InitialState)] = new InitialState(stateMachine, screenNavigator, context.SaveDataContainer);

            dict[typeof(LoadLevelState)] = new LoadLevelState(context, stateMachine, context.SceneLoader(),
                screenNavigator, context.GetService<BackgroundService>(), context.GetService<LevelConfig>());

            dict[typeof(GameState)] = new GameState(stateMachine, screenNavigator);

            dict[typeof(LevelCompletedState)] = new LevelCompletedState(stateMachine, screenNavigator,
                context.GetService<AudioService>(), new LocalLeaderboard(context.SaveDataContainer));

            return dict;
        }
    }
}