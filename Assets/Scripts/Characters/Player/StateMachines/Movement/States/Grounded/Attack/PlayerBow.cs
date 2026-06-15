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
        public LayerMask targetLayer;
        public LayerMask enemyLayer;

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

            Transform aimTarget = GetNearestTarget();

            Vector3 direction =
                aimTarget != null
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

            Arrow arrowScript = arrow.GetComponent<Arrow>();

            if (arrowScript != null)
            {
                arrowScript.target = aimTarget;
            }
        }

        // ======================
        // TARGET SEARCH
        // ======================

        private Transform GetNearestTarget()
        {
            Transform nearestTarget =
                FindClosestByLayer(targetLayer);

            Transform nearestEnemy =
                FindClosestByLayer(enemyLayer);

            if (nearestTarget == null)
                return nearestEnemy;

            if (nearestEnemy == null)
                return nearestTarget;

            float targetDistance =
                Vector3.Distance(
                    transform.position,
                    nearestTarget.position
                );

            float enemyDistance =
                Vector3.Distance(
                    transform.position,
                    nearestEnemy.position
                );

            return targetDistance < enemyDistance
                ? nearestTarget
                : nearestEnemy;
        }

        private Transform FindClosestByLayer(LayerMask layer)
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                autoAimRadius,
                layer
            );

            Transform closest = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider hit in hits)
            {
                float distance =
                    Vector3.Distance(
                        transform.position,
                        hit.transform.position
                    );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = hit.transform;
                }
            }

            return closest;
        }

        // ======================
        // LOCK ICON
        // Tetap hanya untuk Target
        // ======================

        private void UpdateLockIcon()
        {
            Transform target =
                FindClosestByLayer(targetLayer);

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
            {
                Destroy(currentLockIcon);
            }

            currentLockIcon = null;
            currentLockTarget = null;
        }

        // ======================
        // UI
        // ======================

        private void UpdateAmmoText()
        {
            if (ammoText != null)
            {
                ammoText.text =
                    $"{currentArrows}/{maxArrows}";
            }
        }

        private void ShowNoAmmoText()
        {
            if (noAmmoText == null)
                return;

            noAmmoText.gameObject.SetActive(true);
            noAmmoTimer = noAmmoDisplayTime;
        }

        private void HandleNoAmmoText()
        {
            if (noAmmoText == null)
                return;

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
        // PUBLIC
        // ======================

        public void AddArrows(int amount)
        {
            currentArrows =
                Mathf.Clamp(
                    currentArrows + amount,
                    0,
                    maxArrows
                );

            UpdateAmmoText();
        }

        // ======================
        // DEBUG
        // ======================

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                transform.position,
                autoAimRadius
            );
        }
    }
}