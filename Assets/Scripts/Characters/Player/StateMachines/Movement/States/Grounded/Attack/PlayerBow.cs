using UnityEngine;

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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, autoAimRadius);
        }
    }
}
