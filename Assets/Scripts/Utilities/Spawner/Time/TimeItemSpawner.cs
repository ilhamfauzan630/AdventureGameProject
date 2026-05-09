using UnityEngine;

namespace AdventureGame
{
    public class TimeItemSpawner : MonoBehaviour
    {
        [Header("Item")]
        public GameObject timeItemPrefab;

        [Header("Spawn Points")]
        public Transform[] spawnPoints;

        [Header("Spawn Settings")]
        public int maxItems = 3;
        public float respawnDelay = 5f;

        private int currentItems;

        private void Start()
        {
            // Spawn awal
            for (int i = 0; i < maxItems; i++)
            {
                SpawnItem();
            }
        }

        public void SpawnItem()
        {
            // Cegah melebihi batas
            if (currentItems >= maxItems)
                return;

            // Pilih spawn point random
            int randomIndex = Random.Range(0, spawnPoints.Length);

            Transform spawnPoint = spawnPoints[randomIndex];

            // Spawn item
            GameObject item = Instantiate(
                timeItemPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            // Ambil script item
            TimeItem timeItem = item.GetComponent<TimeItem>();

            // Kirim referensi spawner
            timeItem.spawner = this;

            currentItems++;
        }

        public void ItemCollected()
        {
            currentItems--;

            Invoke(nameof(SpawnItem), respawnDelay);
        }
    }
}