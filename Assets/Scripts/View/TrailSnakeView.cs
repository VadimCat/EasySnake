using System.Collections.Generic;
using UnityEngine;

namespace Views
{
    public class TrailSnakeView : MonoBehaviour, ISnakeView
    {
        [SerializeField] private TrailRenderer _trailRenderer;

        private PositionProvider _positionProvider;
        
        public void SetDependencies(PositionProvider positionProvider)
        {
            _positionProvider = positionProvider;
        }

        public void Move(IReadOnlyList<Vector2Int> positions)
        {
            transform.position = _positionProvider.GetPoint(positions[0]);
        }
    }
}