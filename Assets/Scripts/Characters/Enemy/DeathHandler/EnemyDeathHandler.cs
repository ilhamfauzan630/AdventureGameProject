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

            Debug.Log("☠ Enemy mati!");

            // Animasi mati
            animator.SetTrigger("Die");

            // Matikan AI
            var agent = GetComponent<NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            var ai = GetComponent<EnemyGolemAI>();
            if (ai != null) ai.enabled = false;

            // KUNCI PHYSICS
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            // Ganti layer (opsional)
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");

            // Destroy setelah animasi
            Destroy(gameObject, 5f);
        }
    }
}
