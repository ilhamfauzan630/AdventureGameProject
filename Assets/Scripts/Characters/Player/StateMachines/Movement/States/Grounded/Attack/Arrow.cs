using UnityEngine;

namespace AdventureGame
{
    public class Arrow : MonoBehaviour
    {
        [HideInInspector] public Transform target;  // target panah
        public float speed = 30f;
        public float rotateSpeed = 10f;

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
            // Bisa juga pakai CompareTag(targetTag) jika mau
            Debug.Log($"💥 Arrow menabrak {other.name}");
            if (other.CompareTag("Enemy") || other.CompareTag("Target"))
            {
                Debug.Log("Target Hit!");
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}