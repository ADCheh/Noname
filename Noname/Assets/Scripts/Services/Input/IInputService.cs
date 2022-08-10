using UnityEngine;

namespace Services.Input
{
    public interface IInputService
    {
        Vector3 Axis { get; }

        bool IsAttackButtonUp();
    }
}