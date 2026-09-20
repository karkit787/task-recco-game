using System;
using TaskReccoGame.Progression;
using UnityEngine;

namespace TaskReccoGame.Objectives
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class WorldObjective : MonoBehaviour
    {
        [SerializeField] private string _instanceId;
        [SerializeField] private ObjectiveDefinition _definition;
        [SerializeField] private SpriteRenderer _marker;
        [SerializeField] private Color _completedColor = Color.gray;

        private ObjectiveInstance _instance;

        public string InstanceId => _instanceId;
        public ObjectiveDefinition Definition => _definition;
        public ObjectiveState State => _instance.State;

        public event Action<WorldObjective> Completed;

        private void Awake()
        {
            if (_definition == null || string.IsNullOrWhiteSpace(_definition.Id) ||
                string.IsNullOrWhiteSpace(_instanceId) || _definition.Type != ObjectiveType.Interaction ||
                _definition.CoinReward < 0 || _definition.ScoreReward < 0)
            {
                Debug.LogError("WorldObjective requires a valid Interaction definition, rewards, and instance ID.", this);
                enabled = false;
                return;
            }

            _instance = new ObjectiveInstance(_definition);
        }

        public void Interact(PlayerProgression player)
        {
            if (!isActiveAndEnabled || player == null || !_instance.TryComplete())
            {
                return;
            }

            player.AddCoins(_definition.CoinReward);
            player.AddScore(_definition.ScoreReward);

            if (_marker != null)
            {
                _marker.color = _completedColor;
            }

            Debug.Log($"Objective completed: {_definition.DisplayName} ({_instanceId}). " +
                      $"Coins +{_definition.CoinReward} (total {player.Coins}), " +
                      $"Score +{_definition.ScoreReward} (total {player.Score}).", this);
            Completed?.Invoke(this);
        }
    }
}
