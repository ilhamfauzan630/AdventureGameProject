using UnityEngine;

namespace AdventureGame
{
    public class Arrow : MonoBehaviour
    {
        [HideInInspector] public Transform target;  // target panah
        public float speed = 30f;
        public float rotateSpeed = 10f;

        [Header("Efek Ledakan")]
        public GameObject explosionPrefab; 

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false; // panah melesat lurus (ala The Pathless), bukan parabola
        }

        private void Start()
        {
            // Panah langsung mengarah ke depan saat muncul
            rb.velocity = transform.forward * speed;
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            // Arah ke target
            Vector3 direction = (target.position - transform.position).normalized;

            // Smooth belok ke arah target
            Vector3 newVelocity = Vector3.RotateTowards(rb.velocity, direction * speed, rotateSpeed * Time.fixedDeltaTime, 0f);
            rb.velocity = newVelocity;

            // Rotasi panah mengikuti arah terbang
            if (rb.velocity.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Arrow hit: " + other.name);

            if (explosionPrefab != null)
            {
                // Tentukan posisi spawn (bisa pakai titik kontak atau transform panah)
                Vector3 spawnPos = transform.position;

                // Buat efek ledakan
                GameObject explosion = Instantiate(explosionPrefab, spawnPos, Quaternion.identity);

                // Jadikan child dari target
                explosion.transform.SetParent(other.transform);
            }

            // Hancurkan panah setelah mengenai target
            Destroy(gameObject);
        }
    }
}