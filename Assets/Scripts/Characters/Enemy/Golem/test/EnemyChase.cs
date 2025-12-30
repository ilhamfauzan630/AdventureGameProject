using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class EnemyChase : MonoBehaviour
    {
        public Transform player;       // Target pemain
        public float moveSpeed = 3f;   // Kecepatan jalan
        public float detectRange = 10f; // Jarak deteksi pemain

        void Update()
        {
            if (player == null) return;

            float distance = Vector3.Distance(transform.position, player.position);

            // hanya kejar kalau player dalam jarak deteksi
            if (distance < detectRange)
            {
                // arahkan enemy ke player
                Vector3 direction = (player.position - transform.position).normalized;

                // bergerak menuju player
                transform.position += direction * moveSpeed * Time.deltaTime;

                // rotasi menghadap player
                transform.LookAt(player);
            }
        }
    }
}
