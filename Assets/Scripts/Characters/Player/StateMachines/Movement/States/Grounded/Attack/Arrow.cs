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

        [Header("Despawn")]
        public float lifeTime = 8f;                // Hapus otomatis setelah sekian detik
        public float stuckSpeedThreshold = 0.5f;   // Jika kecepatan di bawah ini dianggap "tersangkut"
        public float stuckDespawnDelay = 1.0f;     // Tunggu sekian detik setelah tersangkut lalu hapus

        private Rigidbody rb;
        private Collider arrowCollider;

        [HideInInspector] public Collider shooterCollider; // Untuk abaikan collision dengan pemanah sendiri

        private bool canHit = false; // Delay agar panah tidak langsung meledak saat spawn
        private bool isDead = false; // Flag agar efek/destroy hanya dijalankan sekali
        private float stuckTimer = 0f;

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

            // Auto-destroy setelah lifeTime (cadangan)
            StartCoroutine(DestroyAfterDelay(lifeTime));
        }

        private IEnumerator EnableHitAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            canHit = true;
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!isDead)
            {
                // Optional: bisa spawn efek kecil atau suara "puff" sebelum destroy
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (target == null)
            {
                // optional: masih mau arahkan kalau gak ada target? skip.
            }
            else
            {
                // Belok ke arah target secara halus
                Vector3 direction = (target.position - transform.position).normalized;
                Vector3 newVelocity = Vector3.RotateTowards(rb.velocity, direction * speed, rotateSpeed * Time.fixedDeltaTime, 0f);
                rb.velocity = newVelocity;
            }

            // Rotasi panah mengikuti arah terbang
            if (rb.velocity.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }

            // DETEKSI "TERSANGKUT": jika kecepatan sangat kecil, mulai hitung
            if (rb.velocity.magnitude < stuckSpeedThreshold)
            {
                stuckTimer += Time.fixedDeltaTime;
                if (stuckTimer >= stuckDespawnDelay && !isDead)
                {
                    isDead = true;
                    Destroy(gameObject);
                }
            }
            else
            {
                stuckTimer = 0f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!canHit) return;
            if (shooterCollider != null && other == shooterCollider) return;
            if (isDead) return;

            isDead = true;
            Debug.Log("Arrow hit: " + other.name);

            // Jika kena player
            if (other.CompareTag("Player"))
            {
                var health = other.GetComponent<Ilumisoft.HealthSystem.Health>();
                if (health != null)
                {
                    health.ApplyDamage(10f);
                }
            }

            // Jika kena enemy / target yang bisa hancur
            if (other.CompareTag("Target"))
            {
                // Bisa spawn efek dulu
                if (explosionPrefab != null)
                {
                    GameObject explosion = Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);
                    Destroy(explosion, 1.5f);
                }

                Destroy(other.gameObject); // Hancurkan target

                if (GameManager.Instance != null)
                    GameManager.Instance.RegisterHit();
            }

            // Efek ledakan visual panah
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.SetParent(other.transform);
                Destroy(explosion, 1.5f);
            }

            Destroy(gameObject); // Hancurkan panah
        }

    }
}
