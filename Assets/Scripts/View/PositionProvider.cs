using UnityEngine;

namespace Views
{
    public class PositionProvider
    {
        public readonly Vector2Int Size;
        private readonly float _cellSize;

        public PositionProvider(Vector2Int size, float cellSize)
        {
            Size = size;
            _cellSize = cellSize;
        }

        public Vector3 GetPoint(Vector2Int position)
        {
            return new Vector3(position.x * _cellSize - _cellSize * 5, position.y * _cellSize - _cellSize * 5) + new Vector3(_cellSize, _cellSize) / 2;
        }
    }
}