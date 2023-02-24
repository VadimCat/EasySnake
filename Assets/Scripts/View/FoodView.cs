using DG.Tweening;
using Ji2Core.Core.Pools;
using UnityEngine;
using Ji2.Utils;

namespace Views
{
    public class FoodView : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private FoodViewConfig config;

        private Tween currentTween;
        
        public void Spawn()
        {
            gameObject.SetActive(true);
            sprite.sprite = config.GetNextSprite();
            currentTween = sprite.transform.DoPulseScale(1.04f, 1, sprite.gameObject);
        }

        public void DeSpawn()
        {
            gameObject.SetActive(false);
            currentTween.Kill();
            sprite.transform.localScale = Vector3.one;
        }
    }
}