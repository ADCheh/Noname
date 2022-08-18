using System;
using Data;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Logic;
using Services.Input;
using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(HeroAnimator),typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgress
    {
        public HeroAnimator HeroAnimator;
        public CharacterController CharacterController;

        private IInputService _input;

        private static int _layerMask;
        private float radius;
        private Collider[] _hits = new Collider[3];
        private Stats _stats;

        private void Awake()
        {
            _input = AllServices.Containter.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if(_input.IsAttackButtonUp() && !HeroAnimator.IsAttacking)
                HeroAnimator.PlayAttack();
        }

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        private int Hit()
        {
            return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);
        }

        private Vector3 StartPoint()
        {
            return new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            
        }
    }
}