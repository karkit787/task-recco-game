using System;

namespace TaskReccoGame.Objectives
{
    public sealed class ObjectiveInstance
    {
        public ObjectiveDefinition Definition { get; }
        public ObjectiveState State { get; private set; } = ObjectiveState.Available;

        public ObjectiveInstance(ObjectiveDefinition definition)
        {
            Definition = definition != null ? definition : throw new ArgumentNullException(nameof(definition));
        }

        public bool TryComplete()
        {
            if (State != ObjectiveState.Available)
            {
                return false;
            }

            State = ObjectiveState.Completed;
            return true;
        }
    }
}
