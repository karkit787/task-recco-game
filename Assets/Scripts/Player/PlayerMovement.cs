using UnityEngine;
using UnityEngine.InputSystem;

namespace TaskReccoGame.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField, Min(0f)] private float _movementSpeed = 5f;

        private Rigidbody2D _rigidbody;
        private Vector2 _movementInput;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (_moveAction == null)
            {
                Debug.LogError("PlayerMovement requires a Move input action reference.", this);
                enabled = false;
                return;
            }

            _moveAction.action.Enable();
        }

        private void OnDisable()
        {
            _movementInput = Vector2.zero;

            if (_moveAction != null)
            {
                _moveAction.action.Disable();
            }
        }

        private void Update()
        {
            _movementInput = Vector2.ClampMagnitude(_moveAction.action.ReadValue<Vector2>(), 1f);
        }

        private void FixedUpdate()
        {
            Vector2 displacement = _movementInput * (_movementSpeed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(_rigidbody.position + displacement);
        }
    }
}
