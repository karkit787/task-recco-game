using TaskReccoGame.Objectives;
using TaskReccoGame.Progression;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TaskReccoGame.Player
{
    [RequireComponent(typeof(PlayerProgression))]
    public sealed class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField, Min(0f)] private float _interactionRadius = 0.8f;
        [SerializeField] private LayerMask _interactableLayers;

        private PlayerProgression _progression;
        private InputAction _interactAction;

        private void Awake()
        {
            _progression = GetComponent<PlayerProgression>();
            if (_inputActions != null)
            {
                _interactAction = _inputActions.FindAction("Player/Interact");
            }
        }

        private void OnEnable()
        {
            if (_interactAction == null || _interactableLayers.value == 0 || _interactionRadius <= 0f)
            {
                Debug.LogError("PlayerInteraction requires Player/Interact, a positive radius, and interactable layers.", this);
                enabled = false;
                return;
            }

            _interactAction.Enable();
        }

        private void OnDisable()
        {
            if (_interactAction != null)
            {
                _interactAction.Disable();
            }
        }

        private void Update()
        {
            if (!_interactAction.WasPressedThisFrame())
            {
                return;
            }

            Collider2D target = Physics2D.OverlapCircle(transform.position, _interactionRadius, _interactableLayers);
            if (target != null && target.TryGetComponent(out WorldObjective objective))
            {
                objective.Interact(_progression);
            }
        }
    }
}
