using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth),typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        public EnemyHealth Health;
        public EnemyAnimator Animator;

        public GameObject DeathFx;

        public event Action Happened;

        private void Start()
        {
            Health.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            Health.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (Health.Current <= 0)
                Die();
        }

        private void Die()
        {
            Health.HealthChanged -= HealthChanged;
            
            Animator.PlayDeath();

            SpawnDeathFx();
            
            //
            GetComponent<AgentMoveToHero>().enabled = false;
            //

            StartCoroutine(DestroyTimer());
            
            Happened?.Invoke();
        }

        private void SpawnDeathFx()
        {
            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}