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
    private float lastAttackTime;

    [Header("Rotation")]
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = attackRange - 0.2f;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (isAttacking) return;

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= runRange)
        {
            Run();
        }
        else if (distance <= detectRange)
        {
            Walk();
        }
        else
        {
            Idle();
        }

        RotateToPlayer();
        UpdateAnimation();
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
    }

    void Walk()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        agent.SetDestination(player.position);

        animator.SetBool("IsWalk", true);
        animator.SetBool("IsRun", false);
    }

    void Run()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", true);
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        isAttacking = true;
        agent.isStopped = true;

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetTrigger("Punch");
        animator.SetBool("IsAttacking", true);

        lastAttackTime = Time.time;
    }

    public void EnablePunchHitbox()
    {
        if (punchHitbox) punchHitbox.SetActive(true);
    }

    public void DisablePunchHitbox()
    {
        if (punchHitbox) punchHitbox.SetActive(false);
    }
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    void RotateToPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    void UpdateAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
