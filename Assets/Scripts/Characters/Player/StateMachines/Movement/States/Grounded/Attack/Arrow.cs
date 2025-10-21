using System.Collections;
using UnityEngine;

namespace AdventureGame
{
    public class Arrow : MonoBehaviour
    {
        [HideInInspector] public Transform target;  // Target yang dituju
        public float speed = 30f;
        public float rotateSpeed = 10f;

        [Header("Efek Ledakan")]
        public GameObject explosionPrefab;

        private Rigidbody rb;
        private Collider arrowCollider;

        [HideInInspector] public Collider shooterCollider; // Untuk abaikan collision dengan pemanah sendiri

        private bool canHit = false; // Delay agar panah tidak langsung meledak saat spawn

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            arrowCollider = GetComponent<Collider>();
        }

        private void Start()
        {
            // Abaikan benturan dengan pemanah sendiri
            if (shooterCollider != null)
            {
                Physics.IgnoreCollision(arrowCollider, shooterCollider);
            }

            // Luncurkan panah ke depan
            rb.velocity = transform.forward * speed;

            // Aktifkan deteksi tabrakan setelah delay
            StartCoroutine(EnableHitAfterDelay(0.2f));
        }

        private IEnumerator EnableHitAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            canHit = true;
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            // Belok ke arah target secara halus
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 newVelocity = Vector3.RotateTowards(rb.velocity, direction * speed, rotateSpeed * Time.fixedDeltaTime, 0f);
            rb.velocity = newVelocity;

            // Rotasi panah mengikuti arah terbang
            if (rb.velocity.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!canHit) return; // Belum boleh deteksi benturan
            if (shooterCollider != null && other == shooterCollider) return; // Abaikan pemanah sendiri

            Debug.Log("Arrow hit: " + other.name);

            // 💥 Jika kena player
            if (other.CompareTag("Player"))
            {
                var health = other.GetComponent<Ilumisoft.HealthSystem.Health>();
                if (health != null)
                {
                    health.ApplyDamage(10f);
                }
            }

            // 🔸 Efek ledakan visual (opsional)
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.SetParent(other.transform);
                Destroy(explosion, 1.5f); // hancurkan efek setelah 1.5 detik
            }

            Destroy(gameObject);
        }
    }
}
