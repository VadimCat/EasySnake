using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class LineSnakeView : MonoBehaviour, ISnakeView
    {
        [SerializeField] private LineRenderer line;

        private int prevPointsCount;

        private PositionProvider _positionProvider;

        public void SetDependencies(PositionProvider positionProvider, float speed)
        {
            _positionProvider = positionProvider;
        }

        public void Move(IReadOnlyList<Vector2Int> positions)
        {
            var positionCount = line.positionCount = positions.Count * 2 - 1;

            var keyPositions = new Vector3[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                keyPositions[i] = _positionProvider.GetPoint(positions[i]);
            }
            
            var lastPoint = positions.Count - 1;
            for (var i = 0; i < lastPoint; i++)
            {
                var keyPoint = keyPositions[i];
                var helpPoint = keyPositions[i + 1];

                
                int iClosure = i;
                
                DOTween.To(() => line.GetPosition(iClosure * 2),
                    pos => line.SetPosition(iClosure * 2, pos),
                    keyPoint, .25f);
                
                DOTween.To(() => line.GetPosition(iClosure * 2 + 1),
                    pos => line.SetPosition(iClosure * 2 + 1, pos),
                    helpPoint, .25f);
            }

            DOTween.To(() => line.GetPosition(lastPoint * 2),
                pos => line.SetPosition(lastPoint * 2, pos),
                keyPositions[lastPoint], .25f);
        }
    }
}