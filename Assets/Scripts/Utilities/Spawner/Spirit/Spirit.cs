using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class Spirit : MonoBehaviour
    {
        [HideInInspector]
        public SpiritSpawner spawner;

        public void DestroySpirit()
        {
            // Beri tahu spawner
            spawner.SpiritDestroyed();

            // Hancurkan roh
            Destroy(gameObject);
        }
    }
}
