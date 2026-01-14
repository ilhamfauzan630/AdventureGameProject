using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class GolemJumpHitBox : MonoBehaviour
    {
        public int damage = 10;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player kena Hantam!");
            }
        }
    }
}
