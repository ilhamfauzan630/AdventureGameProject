using System.Collections;
using UnityEngine;

namespace AdventureGame
{
    public class Arrow : MonoBehaviour
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public Collider shooterCollider;

        [Header("Movement")]
        public float speed = 30f;
        public float rotateSpeed = 8f;

        [Header("Effects")]
        public GameObject explosionPrefab;

        [Header("Lifetime")]
        public float lifeTime = 8f;
        public float stuckSpeedThreshold = 0.5f;
        public float stuckDespawnDelay = 1f;

        private Rigidbody rb;
        private Collider arrowCollider;
        private PlayerAudio playerAudio;

        private int enemyLayer;

        private bool canHit = false;
        private bool isDead = false;

        private float stuckTimer = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            arrowCollider = GetComponent<Collider>();

            rb.useGravity = false;

            enemyLayer = LayerMask.NameToLayer("Enemy");

            playerAudio = FindObjectOfType<PlayerAudio>();
        }

        private void Start()
        {
            if (shooterCollider != null)
            {
                Physics.IgnoreCollision(
                    arrowCollider,
                    shooterCollider
                );
            }

            rb.velocity = transform.forward * speed;

            StartCoroutine(EnableHitAfterDelay(0.15f));
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
            if (isDead)
                return;

            HandleHoming();
            HandleRotation();
            HandleStuckCheck();
        }

        private void HandleHoming()
        {
            if (target == null)
                return;

            Vector3 desiredDirection =
                (target.position - transform.position).normalized;

            Vector3 newDirection =
                Vector3.Lerp(
                    rb.velocity.normalized,
                    desiredDirection,
                    rotateSpeed * Time.fixedDeltaTime
                ).normalized;

            rb.velocity = newDirection * speed;
        }

        private void HandleRotation()
        {
            if (rb.velocity.sqrMagnitude > 0.01f)
            {
                transform.rotation =
                    Quaternion.LookRotation(rb.velocity);
            }
        }

        private void HandleStuckCheck()
        {
            if (rb.velocity.magnitude < stuckSpeedThreshold)
            {
                stuckTimer += Time.fixedDeltaTime;

                if (stuckTimer >= stuckDespawnDelay)
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
            if (isDead) return;

            if (shooterCollider != null &&
                other == shooterCollider)
                return;

            isDead = true;

            Debug.Log("Arrow hit : " + other.name);

            HandlePlayerHit(other);
            HandleEnemyHit(other);
            HandleTargetHit(other);

            SpawnImpactEffect();

            Destroy(gameObject);
        }

        private void HandlePlayerHit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            var health =
                other.GetComponent<Ilumisoft.HealthSystem.Health>();

            if (health != null)
            {
                health.ApplyDamage(10f);
            }
        }

        private void HandleEnemyHit(Collider other)
        {
            if (other.gameObject.layer != enemyLayer)
                return;

            if (playerAudio != null)
            {
                playerAudio.PlayExplode();
            }

            var health =
                other.GetComponentInParent<Ilumisoft.HealthSystem.Health>();

            if (health != null)
            {
                health.ApplyDamage(20f);

                Debug.Log("🏹 Enemy terkena panah!");
            }
        }

        private void HandleTargetHit(Collider other)
        {
            if (!other.CompareTag("Target"))
                return;

            if (playerAudio != null)
            {
                playerAudio.PlayExplode();
            }

            Spirit spirit = other.GetComponent<Spirit>();

            if (spirit != null)
            {
                spirit.DestroySpirit();
            }
            else
            {
                Destroy(other.gameObject);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.RegisterHit();
            }
        }

        private void SpawnImpactEffect()
        {
            if (explosionPrefab == null)
                return;

            GameObject effect = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );

            Destroy(effect, 1.5f);
        }
    }
}