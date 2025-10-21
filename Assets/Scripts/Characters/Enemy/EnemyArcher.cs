using UnityEngine;

namespace AdventureGame
{
    public class EnemyArcher : MonoBehaviour
    {
        [Header("Detection Settings")]
        public float detectionRadius = 15f;
        public LayerMask playerLayer;

        [Header("Attack Settings")]
        public GameObject arrowPrefab;
        public Transform shootPoint;
        public float arrowSpeed = 15f;
        public float shootInterval = 0.5f; // ⏱️ waktu jeda antar tembakan

        private Transform targetPlayer;
        private float shootTimer;

        // 🧠 Animator
        private Animator animator;
        private bool isShooting;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            DetectPlayer();

            if (targetPlayer != null)
            {
                AimAtPlayer();
                HandleShooting();
            }
            else
            {
                animator.SetBool("IsShooting", false);
                isShooting = false;
            }

            // ⏱️ hitung mundur timer
            if (shootTimer > 0f)
            {
                shootTimer -= Time.deltaTime;
            }
        }

        private void DetectPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
            if (hits.Length > 0)
            {
                targetPlayer = hits[0].transform;
            }
            else
            {
                targetPlayer = null;
            }
        }

        private void AimAtPlayer()
        {
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
            }
        }

        private void HandleShooting()
        {
            // 🟡 Pastikan musuh tidak langsung spam shoot, tunggu jeda
            if (shootTimer <= 0f && !isShooting)
            {
                isShooting = true;
                animator.SetBool("IsShooting", true);
                // Animasi shoot akan memanggil event ShootArrowEvent dan EndShootEvent
            }
        }

        // 📌 Dipanggil dari Animation Event di tengah animasi shoot
        public void ShootArrowEvent()
        {
            if (targetPlayer == null) return;

            Debug.Log("🎯 EnemyArcher: ShootArrowEvent triggered!");

            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = arrow.GetComponent<Rigidbody>();

            Vector3 dir = (targetPlayer.position - shootPoint.position).normalized;
            rb.velocity = dir * arrowSpeed;
        }

        // 📌 Dipanggil dari akhir animasi shoot
        public void EndShootEvent()
        {
            isShooting = false;
            animator.SetBool("IsShooting", false);

            // Reset timer agar musuh tidak langsung menembak lagi
            shootTimer = shootInterval;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
