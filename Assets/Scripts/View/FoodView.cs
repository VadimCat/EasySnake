using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class FoodView : MonoBehaviour, IPoolable
    {
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