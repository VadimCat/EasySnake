using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Views
{
    public class Head : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int Default = Animator.StringToHash("Default");
        private static readonly int Collision = Animator.StringToHash("Collision");
        private static readonly int Eat = Animator.StringToHash("Eat");

        public void ChangeDirection(Vector2Int rotation)
        {
            if (rotation == Vector2Int.down)
            {
                transform.rotation = Quaternion.identity;
            }
            else if(rotation == Vector2Int.up)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if(rotation == Vector2Int.right)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if(rotation == Vector2Int.left)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
        
        public async void SwitchState(HeadState state)
        {
            switch (state)
            {
                case HeadState.Default:
                    _animator.SetTrigger(Default);
                    break;
                case HeadState.Eat:
                    _animator.SetTrigger(Eat);
                    await UniTask.Delay(500);
                    SwitchState(HeadState.Default);
                    break;
                case HeadState.Collision:
                    _animator.SetTrigger(Collision);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }

    public enum HeadState
    {
        Default,
        Eat,
        Collision
    }

}
