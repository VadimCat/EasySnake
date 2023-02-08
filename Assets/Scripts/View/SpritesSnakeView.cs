using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class SpritesSnakeView : ISnakeView
    {
        private readonly Pool<SnakePartView> partsPool;
        private List<SnakePartView> _parts;
        private PositionProvider _positionProvider;

        private readonly Gradient _gradient;

        public SpritesSnakeView(Pool<SnakePartView> partsPool, PositionProvider positionProvider, Gradient gradient)
        {
            _positionProvider = positionProvider;
            _gradient = gradient;
            this.partsPool = partsPool;
            _parts = new List<SnakePartView>(positionProvider.Size.x * positionProvider.Size.y);
        }

        public void Move(IReadOnlyList<Vector2Int> positions)
        {
            var renderPostionsCount = positions.Count * 2 - 1;

            var keyPositions = new Vector3[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                keyPositions[i] = _positionProvider.GetPoint(positions[i]) + new Vector3(0, 0, .05f * i);
            }

            var diff = renderPostionsCount - _parts.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    _parts.Add(partsPool.Spawn(keyPositions.Last()));
                }
            }
            
            var lastPoint = positions.Count - 1;
            for (var i = 0; i < lastPoint; i++)
            {
                var keyPoint = keyPositions[i];
                var helpPoint = (keyPositions[i] + keyPositions[i + 1]) / 2;

                int iClosure = i;

                var time = (float)iClosure * 2 / _parts.Count;
                var time1 = ((float)iClosure * 2 + 1) / _parts.Count;

                var color = _gradient.Evaluate(time);
                var color1 = _gradient.Evaluate(time1);
                
                _parts[iClosure * 2].transform.DOMove(keyPoint, .25f);;
                _parts[iClosure * 2].SetColor(color);
                _parts[iClosure * 2 + 1].transform.DOMove(helpPoint, .25f);
                _parts[iClosure * 2 + 1].SetColor(color1);

                Debug.LogError((float)iClosure * 2 / _parts.Count);
                Debug.LogError(((float)iClosure * 2 + 1) / _parts.Count);
            }

            _parts.Last().transform.DOMove(keyPositions.Last(), .25f);
        }

        public void Destroy()
        {
            foreach (var part in _parts)
            {
                partsPool.DeSpawn(part);
            }

            _parts.Clear();
        }
    }

    public interface ISnakeView
    {
        public void Move(IReadOnlyList<Vector2Int> positions);
    }
}