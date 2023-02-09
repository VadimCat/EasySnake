using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class SpritesSnakeView : ISnakeView
    {
        private readonly Pool<SnakePartView> _partsPool;
        private readonly List<SnakePartView> _parts;
        private readonly PositionProvider _positionProvider;
        private readonly SpriteSnakeViewConfig _viewConfig;
        
        private Gradient Gradient => _viewConfig.Gradient;
        private AnimationCurve AnimationCurve => _viewConfig.Curve;
        private int PointsPerSegment => _viewConfig.PointPerSegment;
        
        public SpritesSnakeView(Pool<SnakePartView> pool, PositionProvider positionProvider,
            SpriteSnakeViewConfig viewConfig)
        {
            _partsPool = pool;
            _positionProvider = positionProvider;
            _viewConfig = viewConfig;
            
            _parts = new List<SnakePartView>(positionProvider.Size.x * positionProvider.Size.y *
                viewConfig.PointPerSegment - 1);
        }

        public void Move(IReadOnlyList<Vector2Int> positions)
        {
            var renderPositionsCount = (positions.Count - 1) * PointsPerSegment + positions.Count;

            var keyPositions = new Vector3[positions.Count];
            
            for (int i = 0; i < positions.Count; i++)
            {
                keyPositions[i] = _positionProvider.GetPoint(positions[i]);
            }

            var diff = renderPositionsCount - _parts.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    var newPart = _partsPool.Spawn(keyPositions.Last());
                    _parts.Add(newPart);
                    newPart
                        .SetLayer(_parts.Count - 1)
                        .SetInnerSpriteScale(_viewConfig.MaxPartScale);
                }
            }

            float moveTime = .2f;

            var lastPoint = positions.Count - 1;
            for (var i = 0; i < lastPoint; i++)
            {
                var keyPoint = keyPositions[i];
                var midPoint = keyPositions[i + 1];

                for (int j = 0; j < (PointsPerSegment + 1); j++)
                {
                    var currentPartIndex = i * (PointsPerSegment + 1) + j;

                    SetParametersByPos(currentPartIndex);

                    var currentPart = _parts[currentPartIndex];
                    var finalPoint = midPoint + (PointsPerSegment + 1 - j) * (keyPoint - midPoint) /
                        (PointsPerSegment + 1);

                    var dist1 = Vector3.Distance(midPoint, currentPart.transform.position);
                    var dist2 = Vector3.Distance(finalPoint, midPoint);

                    var t1 = moveTime * dist1 / (dist1 + dist2);
                    var t2 = moveTime * dist2 / (dist1 + dist2);

                    var seq = DOTween.Sequence();

                    seq.Append(currentPart.transform.DOMove(midPoint, t1)
                        .SetEase(Ease.Linear));

                    seq.Append(currentPart.transform.DOMove(finalPoint, t2)
                        .SetEase(Ease.Linear));
                }
            }

            var lastPart = _parts.Last();
            lastPart.SetColor(Gradient.Evaluate(1));

            var scale = Vector3.one * AnimationCurve.Evaluate(1);
            lastPart.transform.localScale = scale;

            var tween = lastPart.transform.DOMove(keyPositions.Last(), moveTime)
                .SetEase(Ease.Linear);

            void SetParametersByPos(int currentPartIndex)
            {
                float time = (float)currentPartIndex / _parts.Count;

                var color = Gradient.Evaluate(time);
                _parts[currentPartIndex].SetColor(color);

                var scale = Vector3.one * AnimationCurve.Evaluate(time);
                _parts[currentPartIndex].transform.localScale = scale;
            }
        }

        public void Destroy()
        {
            foreach (var part in _parts)
            {
                _partsPool.DeSpawn(part);
            }

            _parts.Clear();
        }
    }

    public interface ISnakeView
    {
        public void Move(IReadOnlyList<Vector2Int> positions);
    }
}