using UnityEngine;

namespace AdventureGame
{
    public class AmmoPickup : MonoBehaviour
    {
        public int ammoAmount = 5;

        [HideInInspector]
        public AmmoSpawner spawner;

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