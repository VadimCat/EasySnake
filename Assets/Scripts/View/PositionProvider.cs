using UnityEngine;

namespace Views
{
    public class PositionProvider
    {
        private const float borderL = 56;
        private const float borderR = 56;
        private const float borderT = 56;
        private const float borderB = 109;

        public readonly Vector2Int Size;
        private readonly Vector2 _screenSize;
        private readonly Vector2 _availableScreenSize;

        public readonly float _cellSize;

        public readonly Vector2 fieldImageSize;

        public PositionProvider(Vector2Int size, Vector2 screenSize, float screenNavigatorScaleFactor)
        {
            Size = size;
            _screenSize = screenSize;
            _availableScreenSize = screenSize - new Vector2(borderL + borderR, borderB + borderT);

            float worldToPixels = screenSize.y / 4;
            float worldWidth = _availableScreenSize.x / worldToPixels;

            _cellSize = worldWidth / size.x;

            fieldImageSize = new Vector2(size.x, size.y) * (_cellSize * worldToPixels)
                             + new Vector2(borderL + borderR, borderB + borderT) * screenNavigatorScaleFactor;
        }

        public Vector3 GetPoint(Vector2Int position)
        {
            return new Vector3(position.x * _cellSize - _cellSize * Size.x / 2,
                position.y * _cellSize - _cellSize * Size.x / 2) + new Vector3(_cellSize, _cellSize) / 2;
        }
    }
}