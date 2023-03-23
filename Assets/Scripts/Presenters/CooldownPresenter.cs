using Ji2.Models;
using Views;

namespace Presenters
{
    public class CooldownPresenter
    {
        private readonly Cooldown _model;
        private readonly CooldownView _view;

        public CooldownPresenter(Cooldown model, CooldownView view)
        {
            _model = model;
            _view = view;

            view.SetCharges(model.CurrentCharges);
            
            _model.EventChargesUpdate += view.SetCharges;
            _model.EventNormalCooldownProgressUpdate += view.SetCooldownProgress;
        }
    }
}