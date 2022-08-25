using Infrastructure.Factory;
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

        public void Construct(Transform heroTransform)
        {
            _heroTransform = heroTransform;
        }
        

        private void Update()
        {
            SetDestinationForAgent();
        }

        private void SetDestinationForAgent()
        {
            if (_heroTransform)
                Agent.destination = _heroTransform.position;
        }
    }
}
