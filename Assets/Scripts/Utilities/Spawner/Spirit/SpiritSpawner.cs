using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class SpiritSpawner : MonoBehaviour
    {
        [Header("Spirit")]
        public GameObject spiritPrefab;

        [Header("Spawn Points")]
        public Transform[] spawnPoints;

        [Header("Spawn Settings")]
        public int maxSpirit = 5;
        public float spawnDelay = 3f;

        private int currentSpirit;

        void Start()
        {
            // Spawn awal
            for (int i = 0; i < maxSpirit; i++)
            {
                SpawnSpirit();
            }
        }

        void SpawnSpirit()
        {
            // Cegah melebihi batas
            if (currentSpirit >= maxSpirit)
                return;

            // Ambil spawn point random
            int randomIndex = Random.Range(0, spawnPoints.Length);

            Transform spawnPoint = spawnPoints[randomIndex];

            // Spawn roh
            GameObject newSpirit = Instantiate(
                spiritPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            // Ambil script roh
            Spirit spiritScript = newSpirit.GetComponent<Spirit>();

            // Kirim referensi spawner
            spiritScript.spawner = this;

            currentSpirit++;
        }

        public void SpiritDestroyed()
        {
            currentSpirit--;

            // Spawn lagi setelah beberapa detik
            Invoke(nameof(SpawnSpirit), spawnDelay);
        }
    }
}
