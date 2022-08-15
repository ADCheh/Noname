using UnityEngine;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public Bootstrapper BootstrapperPrefab;
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<Bootstrapper>();

            if (bootstrapper == null)
                Instantiate(BootstrapperPrefab);
        }
    }
}