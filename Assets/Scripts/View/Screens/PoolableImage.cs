using Ji2Core.Core.Pools;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Screens
{
    public class PoolableImage : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image image;

        public Image Image => image;

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