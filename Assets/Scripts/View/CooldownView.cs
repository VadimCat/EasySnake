using Ji2.UI;
using TMPro;
using UnityEngine;

namespace Views
{
    public class CooldownView : MonoBehaviour
    {
        [SerializeField] private FillingProgressBar progress;
        [SerializeField] private TMP_Text charges;

        public void SetCharges(int charges)
        {
            this.charges.text = charges.ToString();
        }

        public void SetCooldownProgress(float normalProgress)
        {
            progress.AnimateProgressAsync(normalProgress);
        }
        
    }
}
