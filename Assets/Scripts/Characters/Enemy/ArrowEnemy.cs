using UnityEngine;

namespace AdventureGame
{
    public class ArrowEnemy : MonoBehaviour
    {
        public float lifetime = 5f;
        public int damage = 10;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("🏹 Player terkena panah musuh!");
                // Bisa ditambahkan sistem health player di sini
                Destroy(gameObject);
            }
            else if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
