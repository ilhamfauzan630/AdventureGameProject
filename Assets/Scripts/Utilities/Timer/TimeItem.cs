using UnityEngine;

namespace AdventureGame
{
    public class TimeItem : MonoBehaviour
    {
        [Header("Time Bonus")]
        public float extraTime = 10f;

        [HideInInspector]
        public TimeItemSpawner spawner;

        [HideInInspector]
        public Transform spawnPoint;

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

                // Hancurkan item
                Destroy(gameObject);
            }
        }
    }
}