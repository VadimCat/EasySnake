using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class RecordView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text nickText;
        [SerializeField] private TMP_Text placeText;
        [SerializeField] private Image icon;

        public void SetData((string, int) record, int place)
        {
            scoreText.text = record.Item1;
            nickText.text = record.Item2.ToString();
            placeText.text = place.ToString();
        }

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
            icon.gameObject.SetActive(true);
        }
    }
}