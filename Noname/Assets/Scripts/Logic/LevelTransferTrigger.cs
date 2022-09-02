using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;

namespace Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        public string TransferTo;
        private IGameStateMachine _stateMachine;


        private bool _triggered;
        
        private void Awake()
        {
            _stateMachine = AllServices.Containter.Single<IGameStateMachine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_triggered)
                return;
            
            if (other.CompareTag(PlayerTag))
            {
                _stateMachine.Enter<LoadSceneState, string>(TransferTo);
                _triggered = true;
            }
            
        }
    }
}