using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ji2Core.Core.Pools;
using Ji2Core.Core.States;
using Ji2Core.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Screens
{
    public class GameScreen : BaseScreen
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button playButton;

        [SerializeField] private Button fieldButton;

        [SerializeField] private Image handTip;

        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text highScore;
        [SerializeField] private TMP_Text tipText;
        [SerializeField] private PoolableImage scoreIncTipPrefab;
        [SerializeField] private Image overlay;
        [SerializeField] private GameObject highScoreContainer;

        private Pool<PoolableImage> scoreTipsPool;
        private StateMachine gameScreenStateMachine;

        internal Button PauseButton => pauseButton;
        internal Button PlayButton => playButton;

        public event Action PauseClick;
        public event Action PlayClick;
        public event Action FieldClick;


        public Image Overlay => overlay;

        public StateMachine GetStateMachine()
        {
            if (gameScreenStateMachine == null)
            {
                gameScreenStateMachine = new StateMachine(new GameScreenStatesFactory(this));
                gameScreenStateMachine.Load();
            }

            return gameScreenStateMachine;
        }

        private void Awake()
        {
            scoreTipsPool = new Pool<PoolableImage>(scoreIncTipPrefab, transform);
            pauseButton.onClick.AddListener(FirePauseClick);
            playButton.onClick.AddListener(FirePlayClick);
            fieldButton.onClick.AddListener(FireFieldClick);
        }


        public async Task ShowPointsTip(Vector3 pos)
        {
            var image = scoreTipsPool.Spawn(pos, parent: transform, isWorldSpace: true);
            image.Image.color = new Color(1, 1, 1, 0);

            var sequence = DOTween.Sequence();

            sequence.Join(image.transform.DOMoveY(image.transform.position.y + .1f, .25f).SetEase(Ease.Linear));
            sequence.Join(image.Image.DOFade(1, .25f));
            sequence.Insert(.25f, image.transform.DOMoveY(image.transform.position.y + .2f, .5f));
            sequence.Insert(.25f, image.Image.DOFade(0, .5f));

            await sequence.AwaitForComplete();
            scoreTipsPool.DeSpawn(image);
        }

        public void ShowHandTip(bool isEnabled)
        {
            handTip.DOFade(isEnabled ? 1 : 0, 1f);
        }

        public void SetScore(int score)
        {
            this.score.text = score.ToString();
        }

        public void SetHighScore(int value)
        {
            highScore.text = value.ToString();
        }

        public void ShowTextTip(string text)
        {
            tipText.DOFade(1, 1f);
            tipText.text = text;
        }

        public void HideTextTip(float time = 1f)
        {
            tipText.DOFade(0, time);
        }

        public void HideHighRecord()
        {
            highScoreContainer.SetActive(false);
        }
        
        private void FirePauseClick()
        {
            PauseClick?.Invoke();
        }

        private void FirePlayClick()
        {
            PlayClick?.Invoke();
        }

        private void FireFieldClick()
        {
            FieldClick?.Invoke();
        }

        private void OnDestroy()
        {
            PauseClick = null;
            PlayClick = null;
            FieldClick = null;

            pauseButton.onClick.RemoveAllListeners();
            playButton.onClick.RemoveAllListeners();
            fieldButton.onClick.RemoveAllListeners();
        }
    }
}