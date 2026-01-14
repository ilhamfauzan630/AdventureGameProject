using UnityEngine;
using TMPro;

namespace AdventureGame
{
    public class PlayerBow : MonoBehaviour
    {
        [Header("Ammo Settings")]
        public int maxArrows = 15;
        public int currentArrows = 10;

        [Header("UI")]
        public TextMeshProUGUI ammoText;
        public TextMeshProUGUI noAmmoText;
        public float noAmmoDisplayTime = 1.5f;
        private float noAmmoTimer;

        [Header("Arrow")]
        public GameObject arrowPrefab;
        public Transform arrowSpawnPoint;
        public float arrowSpeed = 20f;

        [Header("Auto Aim")]
        public float autoAimRadius = 12f;
        public LayerMask targetLayer; // AimTarget
        public LayerMask enemyLayer;  // Enemy (fallback)

        [Header("Lock Icon")]
        public GameObject lockIconPrefab;
        public Vector3 lockIconOffset = Vector3.zero;

        private GameObject currentLockIcon;
        private Transform currentLockTarget;

        private void Start()
        {
            UpdateAmmoText();
            if (noAmmoText != null)
                noAmmoText.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateLockIcon();
            HandleNoAmmoText();
        }

        // ======================
        // SHOOT
        // ======================
        public void ShootAutoAim()
        {
            if (currentArrows <= 0)
            {
                ShowNoAmmoText();
                return;
            }

            currentArrows--;
            UpdateAmmoText();

            Transform aimTarget = GetFinalAimTarget();

            Vector3 direction = aimTarget != null
                ? (aimTarget.position - arrowSpawnPoint.position).normalized
                : transform.forward;

            GameObject arrow = Instantiate(
                arrowPrefab,
                arrowSpawnPoint.position,
                Quaternion.LookRotation(direction)
            );

            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * arrowSpeed;
            }
        }

        // ======================
        // AIM LOGIC
        // ======================
        private Transform GetFinalAimTarget()
        {
            // PRIORITAS 1 → Target
            Transform target = FindClosestByLayer(targetLayer);
            if (target != null)
                return target;

            // PRIORITAS 2 → Enemy
            return FindClosestByLayer(enemyLayer);
        }

        private Transform FindClosestByLayer(LayerMask layer)
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                autoAimRadius,
                layer
            );

            Transform closest = null;
            float minDist = Mathf.Infinity;

            foreach (var hit in hits)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = hit.transform;
                }
            }

            return closest;
        }

        // ======================
        // LOCK ICON (TARGET ONLY)
        // ======================
        private void UpdateLockIcon()
        {
            Transform target = FindClosestByLayer(targetLayer);

            if (target == null)
            {
                ClearLockIcon();
                return;
            }

            if (target != currentLockTarget)
            {
                ClearLockIcon();
                currentLockTarget = target;

                currentLockIcon = Instantiate(
                    lockIconPrefab,
                    currentLockTarget.position + lockIconOffset,
                    Quaternion.identity,
                    currentLockTarget
                );
            }
        }

        private void ClearLockIcon()
        {
            if (currentLockIcon != null)
                Destroy(currentLockIcon);

            currentLockIcon = null;
            currentLockTarget = null;
        }

        // ======================
        // UI
        // ======================
        private void UpdateAmmoText()
        {
            if (ammoText != null)
                ammoText.text = $"Arrows: {currentArrows} / {maxArrows}";
        }

        private void ShowNoAmmoText()
        {
            if (noAmmoText != null)
            {
                noAmmoText.gameObject.SetActive(true);
                noAmmoTimer = noAmmoDisplayTime;
            }
        }

        private void HandleNoAmmoText()
        {
            if (noAmmoText == null) return;

            if (noAmmoTimer > 0)
            {
                noAmmoTimer -= Time.deltaTime;
                if (noAmmoTimer <= 0)
                {
                    noAmmoText.gameObject.SetActive(false);
                }
            }
        }

        // ======================
        // DEBUG
        // ======================
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, autoAimRadius);
        }
    }
}
