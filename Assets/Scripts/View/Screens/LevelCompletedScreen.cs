using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ji2Core.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LevelCompletedScreen : BaseScreen
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private RecordView recordViewPrefab;
        [SerializeField] private Transform separatorPrefab;
        [SerializeField] private Transform recordsRoot;
        [SerializeField] private TMP_Text currentScoreTip;
        [SerializeField] private TMP_Text currentScore;
        [SerializeField] private GameObject newBestScore;
        [SerializeField] private GameObject yourScore;
        [SerializeField] private List<Sprite> cupIcons;

        public event Action ClickNext;

        private void Awake()
        {
            nextButton.onClick.AddListener(FireNext);
        }

        private async void FireNext()
        {
            await nextButton.transform.DOScale(0.9f, 0.1f).AwaitForComplete();
            Complete();
        }

        private void Complete()
        {
            ClickNext?.Invoke();
        }

        private void OnDestroy()
        {
            ClickNext = null;
        }

        public void ShowRecords(IReadOnlyList<(string, int)> leaderboardRecords, IReadOnlyList<(string, int)> oldRecord,
            int score)
        {
            currentScore.text = score.ToString();
            SetScoreObject(oldRecord, score);

            for (var i = 0; i < leaderboardRecords.Count; i++)
            {
                var record = leaderboardRecords[i];
                var recordView = Instantiate(recordViewPrefab, recordsRoot);
                recordView.SetData(record, i + 1);
                Instantiate(separatorPrefab, recordsRoot);
                if (i < 3)
                {
                    recordView.SetIcon(cupIcons?[i]);
                }
            }
        }

        private void SetScoreObject(IReadOnlyList<(string, int)> oldRecord, int score)
        {
            if (IsNewBestScoreMustBeShow(oldRecord, score))
            {
                newBestScore.SetActive(true);
            }
            else
            {
                yourScore.SetActive(true);
            }
        }

        private bool IsNewBestScoreMustBeShow(IReadOnlyList<(string, int)> lastRecord, int score)
        {
            if (lastRecord.Count == 0)
                return false;
            return  (lastRecord.Count == 1 || lastRecord[0].Item2 < score);
        }
    }
}