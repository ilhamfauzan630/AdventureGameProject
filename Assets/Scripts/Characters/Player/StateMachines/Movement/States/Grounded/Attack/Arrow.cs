using System.Collections;
using UnityEngine;

namespace AdventureGame
{
    public class Arrow : MonoBehaviour
    {
        [HideInInspector] public Transform target; 
        public float speed = 30f;
        public float rotateSpeed = 10f;

        [Header("Efek Ledakan")]
        public GameObject explosionPrefab;

        [Header("Despawn")]
        public float lifeTime = 8f;
        public float stuckSpeedThreshold = 0.5f;  
        public float stuckDespawnDelay = 1.0f; 

        private Rigidbody rb;
        private Collider arrowCollider;

        private PlayerAudio playerAudio;

        private int enemyLayer;

        [HideInInspector] public Collider shooterCollider;

        private bool canHit = false; 
        private bool isDead = false; 
        private float stuckTimer = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            arrowCollider = GetComponent<Collider>();
            enemyLayer = LayerMask.NameToLayer("Enemy");
            playerAudio = FindObjectOfType<PlayerAudio>();
        }

        private void Start()
        {
            
            if (shooterCollider != null)
            {
                Physics.IgnoreCollision(arrowCollider, shooterCollider);
            }

            rb.velocity = transform.forward * speed;

            StartCoroutine(EnableHitAfterDelay(0.2f));

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
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (target == null)
            {
                // mengarahkan panah jika tidak ada target (opsional)
            }
            else
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Vector3 newVelocity = Vector3.RotateTowards(rb.velocity, direction * speed, rotateSpeed * Time.fixedDeltaTime, 0f);
                rb.velocity = newVelocity;
            }

            if (rb.velocity.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }

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

            // ================= PLAYER =================
            if (other.CompareTag("Player"))
            {
                var health = other.GetComponent<Ilumisoft.HealthSystem.Health>();
                if (health != null)
                    health.ApplyDamage(10f);
            }

            // ================= ENEMY=================
            if (other.gameObject.layer == enemyLayer)
            {
                var health = other.GetComponentInParent<Ilumisoft.HealthSystem.Health>();
                if (health != null)
                {
                    health.ApplyDamage(20f);
                    Debug.Log("🏹 Enemy terkena panah!");
                }
            }

            // ================= TARGET =================
            if (other.CompareTag("Target") && other.gameObject.layer != enemyLayer)
            {
                if (explosionPrefab != null)
                {
                    playerAudio.PlayExplode();

                    var explosion = Instantiate(
                        explosionPrefab,
                        other.transform.position,
                        Quaternion.identity
                    );

                    Destroy(explosion, 1.5f);
                }

                // Ambil script Spirit
                Spirit spirit = other.GetComponent<Spirit>();

                // Jika ada script Spirit
                if (spirit != null)
                {
                    spirit.DestroySpirit();
                }
                else
                {
                    Destroy(other.gameObject);
                }

                if (GameManager.Instance != null)
                    GameManager.Instance.RegisterHit();
            }

            // ================= EFEK PANAH =================
            if (explosionPrefab != null)
            {
                var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.SetParent(other.transform);
                Destroy(explosion, 1.5f);
            }

            Destroy(gameObject);
        }

    }
}
