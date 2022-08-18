using System;
using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress
    {
        public HeroAnimator Animator;

        public Action HealthChanged;
        public float Current
        {
            get => _state.CurrentHP;
            set
            {
                if (_state.CurrentHP != value)
                {
                    HealthChanged?.Invoke();
                    _state.CurrentHP = value;
                }
                
            }
        }

        public float Max
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        private State _state;


        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Current;
            progress.HeroState.MaxHP = Max;
        }

        public void TakeDamage(float damage)
        {
            if(Current <=0)
                return;
            
            Current -= damage;
            Animator.PlayHit();
        }
    }
}