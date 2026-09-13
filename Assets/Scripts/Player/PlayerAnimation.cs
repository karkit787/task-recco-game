using UnityEngine;

namespace TaskReccoGame.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerMovement))]
    public sealed class PlayerAnimation : MonoBehaviour
    {
        private static readonly int MoveXHash =
            Animator.StringToHash("MoveX");

        private static readonly int MoveYHash =
            Animator.StringToHash("MoveY");

        private static readonly int SpeedHash =
            Animator.StringToHash("Speed");

        private Animator _animator;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            
            // Default facing direction: down
            _animator.SetFloat(MoveXHash, 0f);
            _animator.SetFloat(MoveYHash, -1f);
            _animator.SetFloat(SpeedHash, 0f);
        }

        private void Update()
        {
            Vector2 movement = _playerMovement.MovementInput;

            if (movement.sqrMagnitude > 0.001f)
            {
                _animator.SetFloat(MoveXHash, movement.x);
                _animator.SetFloat(MoveYHash, movement.y);
            }

            _animator.SetFloat(SpeedHash, movement.sqrMagnitude);
        }
    }
}