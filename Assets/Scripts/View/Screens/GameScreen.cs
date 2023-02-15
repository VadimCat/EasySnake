using Ji2Core.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Screens
{
    public class GameScreen : BaseScreen
    {
        [SerializeField] private Button cornerButton;
        [SerializeField] private Image cornerButtonGraphic;
        
        [SerializeField] private Image handTip;

        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text tipText;
        [SerializeField] private Image scoreIncTip;
     
       
    }
}
