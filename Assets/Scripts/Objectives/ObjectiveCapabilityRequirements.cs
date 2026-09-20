using System;
using UnityEngine;

namespace TaskReccoGame.Objectives
{
    [Serializable]
    public struct ObjectiveCapabilityRequirements
    {
        [SerializeField, Min(0)] private int _movementLevel;
        [SerializeField, Min(0)] private int _combatLevel;
        [SerializeField] private string _equipmentId;

        public int MovementLevel => _movementLevel;
        public int CombatLevel => _combatLevel;
        public string EquipmentId => _equipmentId;
    }
}
