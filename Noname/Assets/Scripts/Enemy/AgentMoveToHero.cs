using System;
using Infrastructure.Factory;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class AgentMoveToHero : Follow
    {
        private const float MinimalDistance = 1;
        
        public NavMeshAgent Agent;
        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        private void Start()
        {
            _gameFactory = AllServices.Containter.Single<IGameFactory>();

            if (_gameFactory.HeroGameObject != null)
            {
                InitializeHeroTransform();
            }
            else
            {
                _gameFactory.HeroCreated += HeroCreated;
            }
        }

        private void Update()
        {
            if(Initialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
        }

        private bool Initialized()
        {
            return _heroTransform != null;
        }

        private void InitializeHeroTransform()
        {
            _heroTransform = _gameFactory.HeroGameObject.transform;
        }

        private void HeroCreated()
        {
            InitializeHeroTransform();
        }

        private bool HeroNotReached()
        {
            return Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
        }
    }
}
