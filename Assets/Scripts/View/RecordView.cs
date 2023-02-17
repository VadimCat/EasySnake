using TMPro;
using UnityEngine;

namespace Views
{
    public class RecordView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text nickText;
        [SerializeField] private TMP_Text placeText;

        public void SetData((string, int) record, int place)
        {
            scoreText.text = record.Item1;
            nickText.text = record.Item2.ToString();
            placeText.text = place.ToString();
        }
    }
}