using System.Collections.Generic;
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

        private List<Transform> availableSpawnPoints;

        private void Start()
        {
            availableSpawnPoints = new List<Transform>(spawnPoints);

            int spawnCount = Mathf.Min(maxItems, spawnPoints.Length);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnItem();
            }
        }

        public void SpawnItem()
        {
            if (currentItems >= maxItems)
                return;

            if (availableSpawnPoints.Count == 0)
                return;

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);

            Transform selectedPoint =
                availableSpawnPoints[randomIndex];

            availableSpawnPoints.RemoveAt(randomIndex);

            GameObject item = Instantiate(
                timeItemPrefab,
                selectedPoint.position,
                selectedPoint.rotation
            );

            TimeItem timeItem = item.GetComponent<TimeItem>();

            if (timeItem != null)
            {
                timeItem.spawner = this;
                timeItem.spawnPoint = selectedPoint;
            }

            currentItems++;
        }

        public void ItemCollected(Transform spawnPoint)
        {
            if (!availableSpawnPoints.Contains(spawnPoint))
            {
                availableSpawnPoints.Add(spawnPoint);
            }

            currentItems--;

            Invoke(nameof(SpawnItem), respawnDelay);
        }
    }
}