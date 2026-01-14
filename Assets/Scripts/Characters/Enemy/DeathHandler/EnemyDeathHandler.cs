using UnityEngine;
using UnityEngine.AI;

namespace AdventureGame
{
    [RequireComponent(typeof(EnemyHealthBridge))]
    public class EnemyDeathHandler : MonoBehaviour
    {
        private Animator animator;
        private EnemyHealthBridge healthBridge;
        private bool hasDied = false;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            healthBridge = GetComponent<EnemyHealthBridge>();
        }

        private void OnEnable()
        {
            healthBridge.OnDeath += OnEnemyDeath;
        }

        private void OnDisable()
        {
            healthBridge.OnDeath -= OnEnemyDeath;
        }

        private void OnEnemyDeath()
        {
            if (hasDied) return;
            hasDied = true;

            Debug.Log("☠ EnemyDeathHandler: Enemy mati!");

            // Animasi mati
            animator.SetTrigger("Die");

            // Matikan AI
            var agent = GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = false;

            var ai = GetComponent<EnemyGolemAI>(); // kalau ada
            if (ai != null)
                ai.enabled = false;

            // Matikan collider
            var col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            // Optional: destroy setelah animasi
            Destroy(gameObject, 5f);
        }
    }
}
