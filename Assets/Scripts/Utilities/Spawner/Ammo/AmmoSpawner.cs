using UnityEngine;

namespace AdventureGame
{
    public class AmmoSpawner : MonoBehaviour
    {
        [Header("Ammo")]
        public GameObject ammoPrefab;

        [Header("Spawn Points")]
        public Transform[] spawnPoints;

        [Header("Spawn Settings")]
        public int maxAmmoItems = 3;
        public float respawnDelay = 5f;

        private int currentAmmoItems;

        private void Start()
        {
            // Spawn awal
            for (int i = 0; i < maxAmmoItems; i++)
            {
                SpawnAmmo();
            }
        }

        public void SpawnAmmo()
        {
            // Batasi jumlah ammo aktif
            if (currentAmmoItems >= maxAmmoItems)
                return;

            // Ambil spawn point random
            int randomIndex = Random.Range(0, spawnPoints.Length);

            Transform spawnPoint = spawnPoints[randomIndex];

            // Spawn ammo
            GameObject ammo = Instantiate(
                ammoPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            // Ambil script pickup
            AmmoPickup pickup = ammo.GetComponent<AmmoPickup>();

            // Kirim referensi spawner
            pickup.spawner = this;

            currentAmmoItems++;
        }

        public void AmmoCollected()
        {
            currentAmmoItems--;

            Invoke(nameof(SpawnAmmo), respawnDelay);
        }
    }
}