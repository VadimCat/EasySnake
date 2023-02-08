using UnityEngine;

namespace Views
{
    public class PositionProvider
    {
        public readonly Vector2Int Size;

        public PositionProvider(Vector2Int size)
        {
            Size = size;
        }

        public Vector3 GetPoint(Vector2Int position)
        {
            return new Vector3(position.x * .2f - 1, position.y * .2f - 1);
        }
    }
}