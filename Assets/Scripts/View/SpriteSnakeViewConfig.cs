using UnityEngine;

namespace Views
{
    [CreateAssetMenu(menuName = "Configs/View/SpriteSnakeViewConfig", fileName = "SpriteSnakeViewConfig", order = 0)]
    public class SpriteSnakeViewConfig : ScriptableObject
    {
        [SerializeField][Range(.1f, .3f)] private float maxPartScale = .2f;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Gradient gradient;
        [SerializeField][Range(0,9)] private int pointPerSegment = 5;

        public float MaxPartScale => maxPartScale;
        public AnimationCurve Curve => curve;
        public Gradient Gradient => gradient;
        public int PointPerSegment => pointPerSegment;
    }
}