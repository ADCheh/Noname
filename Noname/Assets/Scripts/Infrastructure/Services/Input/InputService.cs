using UnityEngine;

namespace Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        protected const string AttackButton = "Attack";
        
        public abstract Vector3 Axis { get; }
        public abstract bool IsAttackButtonUp();

    }
}