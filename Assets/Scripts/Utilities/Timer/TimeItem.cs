using UnityEngine;

namespace AdventureGame
{
    public class TimeItem : MonoBehaviour
    {
        [Header("Time Bonus")]
        public float extraTime = 10f;

        [HideInInspector] public TimeItemSpawner spawner;

        [HideInInspector] public Transform spawnPoint;

        [SerializeField] private GameObject collectEffectPrefab;

        [SerializeField] private AudioClip collectSound;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Tambah waktu
                CountdownTimer.Instance.AddTime(extraTime);

                // Beri tahu spawner
                if (spawner != null)
                {
                    spawner.ItemCollected(spawnPoint);
                }

                AudioSource.PlayClipAtPoint(
                    collectSound,
                    transform.position
                );

                GameObject effect =
                Instantiate(
                    collectEffectPrefab,
                    transform.position,
                    Quaternion.identity
                );

                Destroy(effect, 2f);

                // Hancurkan item
                Destroy(gameObject);
            }
        }
    }
}