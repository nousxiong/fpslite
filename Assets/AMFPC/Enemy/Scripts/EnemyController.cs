using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public Animator animator;
    public Transform[] wanderPoints;
    public float respawnTime;
    private float _currentRespawnTimer;
    private NavMeshAgent _agent;
    private Vector3 _distination;
    private bool generatedPoint, _distinationSet;
    private float _targetDistance;
    [HideInInspector] public bool dead;
    private EnemyStats enemyStats;
    private HealthManager healthManager;
    private Rigidbody[] rigidBodies;
    private Collider[] colliders;
    private void Awake()
    {
        rigidBodies = animator.gameObject.GetComponentsInChildren<Rigidbody>();
        colliders = animator.gameObject.GetComponentsInChildren<Collider>();
        SetRagdoll(false);
    }
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        healthManager = GetComponent<HealthManager>();
        _agent = GetComponent<NavMeshAgent>();
        _currentRespawnTimer = respawnTime;

        
    }
    void Update()
    {
        if(!dead)
        {
            Chase();
        }

        RespawnTimer();
    }
    public void Chase()
    {
        if(generatedPoint)
        {
            _targetDistance = Vector3.Distance(_distination, transform.position);
            if (!_distinationSet)
            {
                _agent.destination = _distination;
                _distinationSet = true;
            }
            if (_targetDistance < 2)
            {
                generatedPoint = false;
            }
        }
        else
        {
            GenerateDestinationPoint();
        }

    }
    public void GenerateDestinationPoint()
    {
        if(!generatedPoint)
        {
            int random = Random.Range(0, wanderPoints.Length-1);
            _distination = wanderPoints[random].position;
            generatedPoint = true;
            _distinationSet = false;
        }

    }
    public void KillEnemy()
    {
        _agent.destination = transform.position;
        dead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        SetRagdoll(true);
        animator.enabled = false;
    }
    public void RespawnEnemy()
    {
        animator.enabled = true;
        SetRagdoll(false);
        dead = false;
        healthManager.RestoreHealth();
        generatedPoint = false;
        transform.position = wanderPoints[Random.Range(0, wanderPoints.Length)].position;
        GetComponent<CapsuleCollider>().enabled = true;
        healthManager.RestoreHealth();
        enemyStats.UpdateEnemyHealthUI();
    }
    private void RespawnTimer()
    {
        if(dead)
        {
            _currentRespawnTimer -= Time.deltaTime;
            if (_currentRespawnTimer <= 0)
            {
                RespawnEnemy();
                _currentRespawnTimer = respawnTime;
            }
        }
    }
    public void Damage(int value)
    {
        healthManager.Damage(value);
    }
    private void SetRagdoll(bool value)
    {
        foreach (Rigidbody rb in rigidBodies)
        {
            if (value)
            {
                rb.isKinematic = false;
            }
            else
            {
                rb.isKinematic = true;
            }
        }
        foreach (Collider col in colliders)
        {
            col.enabled = value;
        }
    }
}
