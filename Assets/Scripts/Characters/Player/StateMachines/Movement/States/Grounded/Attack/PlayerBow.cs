using UnityEngine;
using TMPro;

namespace AdventureGame
{
    public class PlayerBow : MonoBehaviour
    {
        [Header("Ammo Settings")]
        public int maxArrows = 15;
        public int currentArrows = 10;

        [Header("UI References")]
        public TextMeshProUGUI ammoText;         // tampilkan jumlah panah
        public TextMeshProUGUI noAmmoText;       // tulisan "Panah Habis!"
        public float noAmmoDisplayTime = 1.5f;   // durasi muncul teks panah habis
        private float noAmmoTimer;

        [Header("Arrow Settings")]
        public GameObject arrowPrefab;
        public Transform arrowSpawnPoint;
        public float arrowSpeed = 15f;

        [Header("Auto Aim Settings")]
        public float autoAimRadius = 10f;
        public string targetTag = "Enemy";

        [Header("Lock Icon Settings")]
        public GameObject lockIconPrefab;
        private GameObject currentLockIcon;
        private Transform currentTarget;
        public Vector3 lockIconOffset = Vector3.zero;

        void Start()
        {
            UpdateAmmoText();
            if (noAmmoText != null)
                noAmmoText.gameObject.SetActive(false);
        }

        void Update()
        {
            UpdateLockIcon();
            HandleNoAmmoText();
        }

        public void ShootAutoAim()
        {
            if (currentArrows <= 0)
            {
                ShowNoAmmoText();
                return;
            }

            currentArrows--;
            UpdateAmmoText();

            GameObject target = FindClosestTarget();
            Vector3 direction = target != null ?
                (target.transform.position - arrowSpawnPoint.position).normalized :
                transform.forward;

            GameObject arrowObj = Instantiate(
                arrowPrefab,
                arrowSpawnPoint.position,
                Quaternion.LookRotation(direction)
            );

            Rigidbody rb = arrowObj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = direction * arrowSpeed;
        }

        private void UpdateAmmoText()
        {
            if (ammoText != null)
            {
                ammoText.text = $"Arrows: {currentArrows} / {maxArrows}";
            }
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

        private GameObject FindClosestTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, autoAimRadius);
            GameObject closest = null;
            float minDist = Mathf.Infinity;

            foreach (var hit in hits)
            {
                if (hit.CompareTag(targetTag))
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = hit.gameObject;
                    }
                }
            }
            return closest;
        }

        private void UpdateLockIcon()
        {
            GameObject nearest = FindClosestTarget();

            if (nearest == null)
            {
                if (currentLockIcon != null)
                    Destroy(currentLockIcon);

                currentLockIcon = null;
                currentTarget = null;
                return;
            }

            if (nearest.transform != currentTarget)
            {
                currentTarget = nearest.transform;

                if (currentLockIcon != null)
                    Destroy(currentLockIcon);

                currentLockIcon = Instantiate(
                    lockIconPrefab,
                    currentTarget.position + lockIconOffset,
                    Quaternion.identity,
                    currentTarget
                );
            }
            else if (currentLockIcon)
            {
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
