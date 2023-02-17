using UnityEngine;

namespace Presenters.States
{
    [CreateAssetMenu(menuName = "Configs/Models/LevelConfig", fileName = "LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int size;
        [SerializeField] private float speed;

        public Vector2Int Size => size;
        public float Speed => speed;
    }
}