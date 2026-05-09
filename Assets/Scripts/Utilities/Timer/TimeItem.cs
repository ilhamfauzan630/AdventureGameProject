using UnityEngine;

namespace AdventureGame
{
    public class TimeItem : MonoBehaviour
    {
        [Header("Time Bonus")]
        public float extraTime = 10f;

        private void OnTriggerEnter(Collider other)
        {
            // cek apakah yang menyentuh player
            if (other.CompareTag("Player"))
            {
                // tambah waktu
                CountdownTimer.Instance.AddTime(extraTime);

                // hancurkan item
                Destroy(gameObject);
            }
        }
    }
}