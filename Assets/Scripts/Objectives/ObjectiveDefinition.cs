using UnityEngine;

namespace TaskReccoGame.Objectives
{
    [CreateAssetMenu(fileName = "Objective", menuName = "Task Recco Game/Objective Definition")]
    public sealed class ObjectiveDefinition : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private ObjectiveType _type;
        [SerializeField, Min(0)] private int _coinReward;
        [SerializeField, Min(0)] private int _scoreReward;
        [SerializeField] private ObjectiveDifficulty _difficulty;
        [SerializeField] private ObjectiveCapabilityRequirements _capabilityRequirements;

        public string Id => _id;
        public string DisplayName => _displayName;
        public ObjectiveType Type => _type;
        public int CoinReward => _coinReward;
        public int ScoreReward => _scoreReward;
        public ObjectiveDifficulty Difficulty => _difficulty;
        public ObjectiveCapabilityRequirements CapabilityRequirements => _capabilityRequirements;

        private void OnValidate()
        {
            _coinReward = Mathf.Max(0, _coinReward);
            _scoreReward = Mathf.Max(0, _scoreReward);
        }
    }
}
