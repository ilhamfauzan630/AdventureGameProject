using Ilumisoft.HealthSystem;
using UnityEngine;
using UnityEngine.AI;

namespace AdventureGame
{
    public class EnemyGolemAI : MonoBehaviour
    {
        [Header("References")]
        public Transform player;
        public NavMeshAgent agent;
        private Animator anim;

        [Header("Distances")]
        public float detectRange = 15f;
        public float walkRange = 10f;
        public float runRange = 5f;
        public float attackRange = 2.2f;

        [Header("Attack Settings")]
        public float attackCooldown = 2f;
        private float attackTimer;

        public float jumpAttackChance = 0.25f;  // 25% chance
        public float jumpAttackCooldown = 7f;
        private float jumpTimer;

        private bool isDead = false;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isDead) return;

            float dist = Vector3.Distance(transform.position, player.position);
            attackTimer -= Time.deltaTime;
            jumpTimer -= Time.deltaTime;

            // Set speed parameter for animation
            anim.SetFloat("speed", agent.velocity.magnitude);

            if (dist > detectRange)
            {
                Idle();
            }
            else if (dist > walkRange)
            {
                WalkToPlayer();
            }
            else if (dist > runRange)
            {
                RunToPlayer();
            }
            else if (dist <= attackRange)
            {
                AttackPlayer();
            }
        }

        private void Idle()
        {
            agent.isStopped = true;
        }

        private void WalkToPlayer()
        {
            agent.isStopped = false;
            agent.speed = 1.5f;   // jalan
            agent.SetDestination(player.position);
        }

        private void RunToPlayer()
        {
            agent.isStopped = false;
            agent.speed = 3.5f;   // lari
            agent.SetDestination(player.position);
        }

        private void AttackPlayer()
        {
            agent.isStopped = true;
            transform.LookAt(player);

            if (attackTimer <= 0f)
            {
                // Randomly choose between normal attack or jump attack
                if (jumpTimer <= 0f && Random.value < jumpAttackChance)
                {
                    anim.SetTrigger("jumpAttack");
                    jumpTimer = jumpAttackCooldown;
                }
                else
                {
                    anim.SetTrigger("attack");
                }

                attackTimer = attackCooldown;
            }
        }

        // Dipanggil dari Animation Event
        public void DealDamage()
        {
            if (Vector3.Distance(transform.position, player.position) < attackRange + 0.5f)
            {
                var hp = player.GetComponent<Health>();
                if (hp != null)
                    hp.ApplyDamage(10);
            }
        }

        public void Die()
        {
            if (isDead) return;

            isDead = true;
            agent.isStopped = true;
            anim.SetTrigger("die");

            Destroy(gameObject, 4f);
        }
    }
}
