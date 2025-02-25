﻿using UnityEngine;

namespace Views
{
    [CreateAssetMenu(menuName = "Configs/View/SpriteSnakeViewConfig", fileName = "SpriteSnakeViewConfig", order = 0)]
    public class SpriteSnakeViewConfig : ScriptableObject
    {
        [SerializeField] [Range(.001f, 1f)] private float normalizedPartScale = 1;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Color color1;
        [SerializeField] private Color color2;
        
        [SerializeField] [Range(0, 9)] private int pointPerSegment = 5;

        public float NormalizedPartScale => normalizedPartScale;
        public AnimationCurve Curve => curve;
        public Gradient Gradient => gradient;
        public int PointPerSegment => pointPerSegment;

        public Color Color1 => color1;
        public Color Color2 => color2;
    }
}