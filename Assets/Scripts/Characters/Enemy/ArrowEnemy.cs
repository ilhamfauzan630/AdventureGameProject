using UnityEngine;
using Ilumisoft.HealthSystem;

namespace AdventureGame
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class ArrowEnemy : MonoBehaviour
    {
        public float lifetime = 5f;
        public float damage = 10f;
        public float speed = 10f;
        public float trackingStrength = 2f; // Semakin besar nilainya, semakin cepat panah menyesuaikan arah ke player

        private Rigidbody rb;
        private Transform target; // Referensi ke Player

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;

            Collider col = GetComponent<Collider>();
            col.isTrigger = true;

            // Cari player berdasarkan tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }

            // Arah awal panah (menghadap ke depan dari transform saat ditembak)
            rb.velocity = transform.forward * speed;

            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            if (target != null)
            {
                // Hitung arah menuju player
                Vector3 directionToPlayer = (target.position - transform.position).normalized;

                // Interpolasi rotasi agar smooth mengikuti player
                Vector3 newDirection = Vector3.Lerp(rb.velocity.normalized, directionToPlayer, trackingStrength * Time.deltaTime);
                rb.velocity = newDirection * speed;

                // Rotasi panah mengikuti arah terbang
                transform.forward = rb.velocity.normalized;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Cegah panah menabrak pemanah sendiri
            if (other.CompareTag("Enemy")) return;

            Debug.Log($"🎯 ArrowEnemy kena: {other.name} | Tag: {other.tag}");

            // Pastikan hanya kena Player
            if (other.CompareTag("Player") || other.GetComponentInParent<Health>() != null)
            {
                var health = other.GetComponentInParent<Health>();

                if (health != null)
                {
                    health.ApplyDamage(damage);
                    Debug.Log($"💔 Player kena {damage} damage! Sisa HP: {health.CurrentHealth}");
                }

                Destroy(gameObject);
            }
            else if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}