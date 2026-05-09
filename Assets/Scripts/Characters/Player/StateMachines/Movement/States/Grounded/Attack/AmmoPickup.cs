using UnityEngine;

namespace AdventureGame
{
    public class AmmoPickup : MonoBehaviour
    {
        public int ammoAmount = 5;

        public float floatSpeed = 2f;
        public float floatHeight = 0.2f;

        private Vector3 startPos;

        [HideInInspector]
        public AmmoSpawner spawner;

        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            transform.position =
                startPos +
                new Vector3(
                    0,
                    Mathf.Sin(Time.time * floatSpeed) * floatHeight,
                    0
                );
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerBow bow = other.GetComponentInChildren<PlayerBow>();

                if (bow != null)
                {
                    // Tambah ammo
                    bow.currentArrows += ammoAmount;

                    bow.currentArrows = Mathf.Clamp(
                        bow.currentArrows,
                        0,
                        bow.maxArrows
                    );

                    bow.SendMessage(
                        "UpdateAmmoText",
                        SendMessageOptions.DontRequireReceiver
                    );

                    // Beri tahu spawner
                    if (spawner != null)
                    {
                        spawner.AmmoCollected();
                    }

                    // Hancurkan item
                    Destroy(gameObject);
                }
            }
        }
    }
}