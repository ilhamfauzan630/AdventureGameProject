using UnityEngine;

namespace AdventureGame
{
    public class FloatingItem : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 90f;

        [Header("Floating")]
        [SerializeField] private float floatHeight = 0.25f;
        [SerializeField] private float floatSpeed = 2f;

        private Vector3 startPosition;
        private float randomOffset;

        private void Start()
        {
            startPosition = transform.position;
            randomOffset = Random.Range(0f, Mathf.PI * 2f);
        }

        private void Update()
        {
            transform.Rotate(
                Vector3.up,
                rotationSpeed * Time.deltaTime,
                Space.World
            );

            float yOffset =
                Mathf.Sin(Time.time * floatSpeed + randomOffset) * floatHeight;

            transform.position = startPosition + Vector3.up * yOffset;
        }
    }
}