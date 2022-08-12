using Infrastructure.Services;
using UnityEngine;

namespace Services.Input
{
    public interface IInputService : IService
    {
        Vector3 Axis { get; }

        bool IsAttackButtonUp();
    }
}