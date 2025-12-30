using System.Collections;
using System.Collections.Generic;
using Ilumisoft.HealthSystem;
using UnityEngine;

namespace AdventureGame
{
    public class EnemyGolemAttack : MonoBehaviour
    {
        [Header("Attack Settings")]
        public int damage = 10;
        public float radius = 2f;
        public LayerMask playerMask;

        [Header("Debug")]
        public bool showGizmo = true;

        public void Hit()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, playerMask);

            foreach (var hit in hits)
            {
                // Cek health di object ataupun parent
                Health health = hit.GetComponent<Health>() ?? hit.GetComponentInParent<Health>();

                if (health != null)
                {
                    health.ApplyDamage(damage);
                    Debug.Log($"💥 Golem menyerang! Player menerima {damage} damage. Sisa HP: {health.CurrentHealth}");
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmo) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
