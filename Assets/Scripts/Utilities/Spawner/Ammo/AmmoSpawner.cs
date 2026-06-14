using System.Collections.Generic;
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

        private List<Transform> availableSpawnPoints;

        private void Start()
        {
            availableSpawnPoints = new List<Transform>(spawnPoints);

            int spawnCount = Mathf.Min(
                maxAmmoItems,
                spawnPoints.Length
            );

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnAmmo();
            }
        }

        public void SpawnAmmo()
        {
            if (currentAmmoItems >= maxAmmoItems)
                return;

            if (availableSpawnPoints.Count == 0)
                return;

            int randomIndex = Random.Range(
                0,
                availableSpawnPoints.Count
            );

            Transform selectedPoint =
                availableSpawnPoints[randomIndex];

            availableSpawnPoints.RemoveAt(randomIndex);

            GameObject ammo = Instantiate(
                ammoPrefab,
                selectedPoint.position,
                selectedPoint.rotation
            );

            AmmoPickup pickup =
                ammo.GetComponent<AmmoPickup>();

            if (pickup != null)
            {
                pickup.spawner = this;
                pickup.spawnPoint = selectedPoint;
            }

            currentAmmoItems++;
        }

        public void AmmoCollected(Transform spawnPoint)
        {
            if (!availableSpawnPoints.Contains(spawnPoint))
            {
                availableSpawnPoints.Add(spawnPoint);
            }

            currentAmmoItems--;

            Invoke(nameof(SpawnAmmo), respawnDelay);
        }
    }
}