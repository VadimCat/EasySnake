using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class FoodView : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private FoodViewConfig config;
        
        public void Spawn()
        {
            gameObject.SetActive(true);
            sprite.sprite = config.GetNextSprite();
        }

        public void DeSpawn()
        {
            gameObject.SetActive(false);
        }
    }
}