using UnityEngine;
using System.Linq;

namespace AdventureGame
{
    public class TargetLockSystem : MonoBehaviour
    {
        public float lockRange = 20f;
        public LayerMask targetMask;
        public GameObject lockIndicatorPrefab;

        private Transform currentTarget;
        private GameObject indicatorInstance;

        void Update()
        {
            // Tekan L untuk lock/unlock
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (currentTarget == null)
                    LockNearestTarget();
                else
                    UnlockTarget();
            }

            // Update posisi indikator jika terkunci
            if (currentTarget != null && indicatorInstance != null)
            {
                indicatorInstance.transform.position = currentTarget.position + Vector3.up * 2f;
            }
        }

        void LockNearestTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, lockRange, targetMask);
            if (hits.Length == 0) return;

            var nearest = hits
                .OrderBy(h => Vector3.Distance(transform.position, h.transform.position))
                .First().transform;

            currentTarget = nearest;

            if (lockIndicatorPrefab != null)
            {
                indicatorInstance = Instantiate(lockIndicatorPrefab, currentTarget.position + Vector3.up * 2f, Quaternion.identity);
                indicatorInstance.transform.SetParent(currentTarget);
                Debug.Log("Indicator instantiated: " + indicatorInstance.name);

            }
            else
            {
                    Debug.LogWarning("No lockIndicatorPrefab assigned!");

            }

            Debug.Log("Locked on " + currentTarget.name);
        }

        void UnlockTarget()
        {
            if (indicatorInstance != null)
                Destroy(indicatorInstance);

            currentTarget = null;
            Debug.Log("Target unlocked");
        }

        public Transform GetCurrentTarget()
        {
            return currentTarget;
        }
    }
}
