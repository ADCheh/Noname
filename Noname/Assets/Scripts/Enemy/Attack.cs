using System.Linq;
using Data;
using Hero;
using Infrastructure.Factory;
using Infrastructure.Services;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;
        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 0.5f;
        public float Damage = 10f;

        private IGameFactory _factory;
        private Transform _heroTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;

        private void Awake()
        {
            _factory = AllServices.Containter.Single<IGameFactory>();

            _layerMask = 1 << LayerMask.NameToLayer("Player");
            
            _factory.HeroCreated += OnHeroCreated;
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1);
                hit.transform.GetComponent<HeroHealth>().TakeDamage(Damage);
            }
        }

        public void DisableAttack()
        {
            _attackIsActive = false;
        }

        public void EnableAttack()
        {
            _attackIsActive = true;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();
            
            return hitCount > 0;
        }

        private Vector3 StartPoint()
        {
            return new Vector3(transform.position.x,transform.position.y,transform.position.z).AddY(0.5f) + transform.forward*EffectiveDistance;
        }

        private void OnAttackEnded()
        {
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
        }

        private void UpdateCooldown()
        {
            if (!CoolDownIsUp())
                _attackCooldown -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            Animator.PlayAttack();

            _isAttacking = true;
        }

        private bool CanAttack()
        {
            return _attackIsActive && !_isAttacking && CoolDownIsUp();
        }

        private bool CoolDownIsUp()
        {
            return _attackCooldown <= 0f;
        }

        private void OnHeroCreated()
        {
            _heroTransform = _factory.HeroGameObject.transform;
        }
    }
}