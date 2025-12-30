using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class EnemyGolemHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        private int currentHP;

        private EnemyGolemAI ai;

        void Start()
        {
            currentHP = maxHealth;
            ai = GetComponent<EnemyGolemAI>();
        }

        public void TakeDamage(int amount)
        {
            currentHP -= amount;

            if (currentHP <= 0)
            {
                ai.Die();
            }
        }
    }
}
