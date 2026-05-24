using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Attack }

    [Header("State Machine")]
    [SerializeField] private EnemyState currentState = EnemyState.Idle;

    [Header("Movement & Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float idleTimeAtPoint = 2f;
    private int currentPatrolIndex = 0;
    private float idleTimer = 0f;

    [Header("Detection & Combat")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float attackTimer = 0f;
    EntityStats entityStats;

    private NavMeshAgent agent;
    public Animator animator;

    // Cache Animator Hash IDs for performance optimization
    private readonly int moveXHash = Animator.StringToHash("MoveX");
    private readonly int moveYHash = Animator.StringToHash("MoveY");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int attackTriggerHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
        agent.updateUpAxis = false;
        playerTarget = FindObjectOfType<PlayerMovement>()?.transform;
    }

    private void Start()
    {
        entityStats = GetComponent<EntityStats>();
        if (patrolPoints.Length > 0)
        {
            SetState(EnemyState.Patrol);
            MoveToNextPatrolPoint();
        }
        else
        {
            SetState(EnemyState.Idle);
        }
    }

    private void Update()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

        float distanceToPlayer = playerTarget != null ? Vector3.Distance(transform.position, playerTarget.position) : Mathf.Infinity;

        if (distanceToPlayer <= attackRadius)
        {
            SetState(EnemyState.Attack);
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            SetState(EnemyState.Chase);
        }
        else if (currentState == EnemyState.Chase || currentState == EnemyState.Attack)
        {
            SetState(patrolPoints.Length > 0 ? EnemyState.Patrol : EnemyState.Idle);
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Patrol:
                HandlePatrolState();
                break;
            case EnemyState.Chase:
                HandleChaseState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
        }

        UpdateAnimationParameters();
    }

    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        
        if (newState == EnemyState.Idle) agent.ResetPath();
    }

    private void HandleIdleState()
    {
        if (patrolPoints.Length == 0) return;

        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTimeAtPoint)
        {
            idleTimer = 0f;
            SetState(EnemyState.Patrol);
            MoveToNextPatrolPoint();
        }
    }

    private void HandlePatrolState()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetState(EnemyState.Idle);
        }
    }

    private void HandleChaseState()
    {
        agent.SetDestination(playerTarget.position);
        playerTarget.GetComponent<PlayerMovement>().CamShake(0f, 0f);
    }

    private void HandleAttackState()
    {
        agent.ResetPath();

        if (playerTarget != null)
        {
            Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;
            animator.SetFloat(moveXHash, directionToPlayer.x);
            animator.SetFloat(moveYHash, directionToPlayer.y);
        }

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }

    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void UpdateAnimationParameters()
    {
        if (currentState == EnemyState.Attack)
        {
            animator.SetBool(isMovingHash, false);
            return;
        }

        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            animator.SetBool(isMovingHash, true);
            
            Vector3 moveDirection = velocity.normalized;
            animator.SetFloat(moveXHash, moveDirection.x);
            animator.SetFloat(moveYHash, moveDirection.y);
        }
        else
        {
            animator.SetBool(isMovingHash, false);
        }
    }

    private void Attack()
    {
        playerTarget.GetComponent<PlayerMovement>().CamShake(2f, 2f);
        animator.SetTrigger(attackTriggerHash);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private EnemySpawner mySpawner;

    public void AssignSpawner(EnemySpawner spawner)
    {
        mySpawner = spawner;
    }

    private void OnDestroy()
    {
        if (mySpawner != null)
        {
            mySpawner.DecrementEnemyCount();
        }
    }
}