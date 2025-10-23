using UnityEngine;
using System.Linq;

namespace AdventureGame
{
    public class PlayerBow : MonoBehaviour
    {
        [Header("Auto Aim Settings")]
        public float autoAimRadius = 10f;
        public string targetTag = "Enemy";

        [Header("Arrow Settings")]
        public GameObject arrowPrefab;
        public Transform arrowSpawnPoint;
        public float arrowSpeed = 10f;

        [Header("Lock Icon Settings")]
        public GameObject lockIconPrefab;      // prefab ikon lock (misalnya target indicator)
        private GameObject currentLockIcon;    // instance yang sedang aktif
        private Transform currentTarget;       // target terdekat saat ini
        public Vector3 lockIconOffset = Vector3.zero;

        void Update()
        {
            UpdateLockIcon();
        }

        public void ShootAutoAim()
        {
            // Cari target
            GameObject target = FindClosestTarget();
            Vector3 shootDirection;

            if (target != null)
            {
                shootDirection = (target.transform.position - arrowSpawnPoint.position).normalized;
                Debug.Log("🎯 Target ditemukan: " + target.name);
            }
            else
            {
                shootDirection = transform.forward;
                Debug.Log("❌ Tidak ada target dalam radius");
            }

            // Spawn arrow
            GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            Arrow arrow = arrowObj.GetComponent<Arrow>();
            Rigidbody rb = arrowObj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = shootDirection * arrowSpeed;
            }

            if (arrow != null && target != null)
            {
                arrow.target = target.transform; // 👉 berikan target ke arrow
                arrow.speed = arrowSpeed;        // sinkron dengan bow
            }
        }

        private GameObject FindClosestTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, autoAimRadius);
            GameObject closest = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag(targetTag))
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = hit.gameObject;
                    }
                }
            }

            return closest;
        }

        private void UpdateLockIcon()
        {
            // Temukan target terdekat setiap frame
            GameObject nearest = FindClosestTarget();

            // Jika tidak ada target dalam radius, hapus ikon
            if (nearest == null)
            {
                if (currentLockIcon != null)
                {
                    Destroy(currentLockIcon);
                    currentLockIcon = null;
                    currentTarget = null;
                }
                return;
            }

            // Jika target berubah, buat ulang ikon
            if (nearest.transform != currentTarget)
            {
                currentTarget = nearest.transform;

                // Hapus ikon lama
                if (currentLockIcon != null)
                    Destroy(currentLockIcon);

                // Buat ikon baru di atas target
                if (lockIconPrefab != null)
                {
                    currentLockIcon = Instantiate(
                        lockIconPrefab,
                        currentTarget.position + lockIconOffset,
                        Quaternion.identity,
                        currentTarget
                    );
                }
            }
            else if (currentLockIcon != null)
            {
                // Update posisi agar tetap di atas target
                currentLockIcon.transform.position = currentTarget.position + lockIconOffset;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, autoAimRadius);
        }
    }
}
