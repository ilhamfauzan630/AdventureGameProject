using UnityEngine;
using UnityEngine.AI;

public class EnemyGolemAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Hitbox")]
    public GameObject punchHitbox;

    [Header("Range")]
    public float detectRange = 15f;
    public float walkRange = 10f;
    public float runRange = 5f;
    public float attackRange = 2f;

    [Header("Speed")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;

    [Header("Attack")]
    public float attackCooldown = 2f;

    [Header("Rotation")]
    public float rotationSpeed = 5f;

    private float lastAttackTime;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttacking;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = attackRange * 0.8f;

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );
        Debug.Log($"Distance={distance} | Attacking={isAttacking} | Stopped={agent.isStopped} | Remaining={agent.remainingDistance}");

        // Selalu update tujuan selama player masih terdeteksi
        if (!isAttacking &&
            distance <= detectRange)
        {
            agent.SetDestination(
                player.position
            );
        }

        if (isAttacking)
        {
            RotateToPlayer();
            return;
        }

        // Attack
        if (distance <= attackRange + 0.3f)
        {
            Attack();
        }
        // Run
        else if (distance <= runRange)
        {
            Run();
        }
        // Walk
        else if (distance <= detectRange)
        {
            Walk();
        }
        // Idle
        else
        {
            Idle();
        }

        RotateToPlayer();
        UpdateAnimation();
    }

    private void Idle()
    {
        agent.isStopped = true;

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
    }

    private void Walk()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;

        animator.SetBool("IsWalk", true);
        animator.SetBool("IsRun", false);
    }

    private void Run()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", true);
    }

    private void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        isAttacking = true;

        agent.isStopped = true;

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);

        animator.SetTrigger("Punch");
        animator.SetBool("IsAttacking", true);

        lastAttackTime = Time.time;

        Debug.Log("👊 Golem Attack!");
    }

    // Animation Event
    public void EnablePunchHitbox()
    {
        if (punchHitbox != null)
        {
            punchHitbox.SetActive(true);
        }
    }

    // Animation Event
    public void DisablePunchHitbox()
    {
        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);
        }
    }

    // Animation Event
    public void EndAttack()
    {
        isAttacking = false;

        agent.isStopped = false;

        // Paksa update tujuan lagi
        if (player != null)
        {
            agent.SetDestination(
                player.position
            );
        }

        animator.SetBool(
            "IsAttacking",
            false
        );

        Debug.Log("✅ End Attack");
    }

    private void RotateToPlayer()
    {
        Vector3 direction =
            (player.position - transform.position).normalized;

        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion lookRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );
    }

    private void UpdateAnimation()
    {
        animator.SetFloat(
            "Speed",
            agent.velocity.magnitude
        );
    }
}