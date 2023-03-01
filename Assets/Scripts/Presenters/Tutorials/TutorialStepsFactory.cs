using System;
using Ji2.Presenters.Tutorial;
using Ji2.UI;
using Ji2Core.Core;
using Ji2Core.Core.States;

namespace Presenters.Tutorials
{
    public class TutorialStepsFactory : ITutorialFactory
    {
        private readonly Context _context;

        public TutorialStepsFactory(Context context)
        {
            _context = context;
        }

        public ITutorialStep Create<TStep>() where TStep : ITutorialStep
        {
            if (typeof(TStep) == typeof(InitialTutorialState))
            {
                return new InitialTutorialState(_context.GetService<StateMachine>(), _context.GetService<TutorialPointerView>());
            }

            throw new NotImplementedException();
        }
    }
}
