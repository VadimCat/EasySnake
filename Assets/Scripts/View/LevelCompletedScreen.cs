using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ji2.Utils;
using Ji2Core.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LevelCompletedScreen : BaseScreen
    {
        [SerializeField] private Button nextButton;

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
    }
}