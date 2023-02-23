using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class SnakePartView : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Color GetColor() => _spriteRenderer.color;

        public SnakePartView SetColor(Color color)
        {
            _spriteRenderer.color = color;
            return this;
        }

        public SnakePartView SetInnerSpriteScale(float scale)
        {
            _spriteRenderer.transform.localScale = Vector3.one * scale;
            return this;
        }

        public SnakePartView SetLayer(int layer)
        {
            _spriteRenderer.sortingOrder = layer;
            return this;
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
        }

        public void DeSpawn()
        {
            gameObject.SetActive(false);
        }
    }
}