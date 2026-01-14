using UnityEngine;
using Ilumisoft.HealthSystem;

namespace AdventureGame
{
    public class GolemPunchHitBox : MonoBehaviour
    {
        [Header("Damage")]
        public float damage = 10f;

        private void OnTriggerEnter(Collider other)
        {
            // Pastikan yang kena adalah Player atau punya Health
            if (other.CompareTag("Player") || other.GetComponentInParent<Health>() != null)
            {
                Health health = other.GetComponentInParent<Health>();

                if (health != null)
                {
                    health.ApplyDamage(damage);
                    Debug.Log($"👊 Player kena pukul! Damage: {damage}, Sisa HP: {health.CurrentHealth}");
                }
            }
        }
    }
}
