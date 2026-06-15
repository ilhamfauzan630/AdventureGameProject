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
            if (!other.CompareTag("Player"))
                return;

            Health health =
                other.GetComponentInParent<Health>();

            if (health != null)
            {
                health.ApplyDamage(damage);

                Debug.Log(
                    $"👊 Player kena pukul! Damage: {damage}, Sisa HP: {health.CurrentHealth}"
                );
            }
        }
    }
}