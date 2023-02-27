using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views
{
    [CreateAssetMenu(menuName = "Configs/View/SnakeFoodAnimationConfig", fileName = "SnakeFoodAnimationConfig")]
    public class SnakeFoodAnimationConfig : ScriptableObject
    {
         [MaxValue(30)][SerializeField] private float animationSpeedFactor;
        public float AnimationSpeedFactor => animationSpeedFactor;
    }
}