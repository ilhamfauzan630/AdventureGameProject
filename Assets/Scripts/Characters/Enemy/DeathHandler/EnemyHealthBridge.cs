using UnityEngine;
using Ilumisoft.HealthSystem;

namespace AdventureGame
{
    [RequireComponent(typeof(HealthComponent))]
    public class EnemyHealthBridge : MonoBehaviour
    {
        private HealthComponent healthComponent;
        public System.Action OnDeath;

        private void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
        }

        private void OnEnable()
        {
            if (healthComponent != null)
                healthComponent.OnHealthEmpty += HandleDeath;
        }

        private void OnDisable()
        {
            if (healthComponent != null)
                healthComponent.OnHealthEmpty -= HandleDeath;
        }

        private void HandleDeath()
        {
            Debug.Log("☠ EnemyHealthBridge: Enemy mati!");
            OnDeath?.Invoke();
        }
    }
}
