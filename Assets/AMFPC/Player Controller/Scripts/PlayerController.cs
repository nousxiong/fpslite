using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public PlayerState playerState;
    [HideInInspector] public Collisions collisions;
    [HideInInspector] public DefaultMovement defaultMovement;
    [HideInInspector] public Jumping jumping;
    [HideInInspector] public Crouching crouching;
    [HideInInspector] public SlopeSliding slopeSliding;
    [HideInInspector] public Sliding sliding;
    [HideInInspector] public Gravity gravity;
    [HideInInspector] public HealthManager healthManager;
    [HideInInspector] public PlayerRespawn playerRespawn;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public float CCInitialHeight;

    [HideInInspector] public Vector3[] velocities = new Vector3[128];
    [HideInInspector] public int velocityVectorsCount = 0;
    [HideInInspector] public Vector3 velocity;

    private Vector3 _lastPlayerPos;
    [HideInInspector] public Vector3 playerVelocity;

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        healthManager = GetComponent<HealthManager>();
        playerRespawn = GetComponent<PlayerRespawn>();
        playerState = GetComponent<PlayerState>();
        collisions = GetComponent<Collisions>();
        defaultMovement = GetComponent<DefaultMovement>();
        jumping = GetComponent<Jumping>();
        crouching = GetComponent<Crouching>();
        slopeSliding = GetComponent<SlopeSliding>();
        sliding = GetComponent <Sliding>();
        gravity = GetComponent<Gravity>();
        characterController = GetComponent<CharacterController>();
        CCInitialHeight = characterController.height;

    }
    void Start()
    {
        _lastPlayerPos = this.transform.position;
    }
    void Update()
    {
        SetVelocity(); // add velocity vectors to the main velocity vector
        Move(velocity); // move character controller
        CalculateVelocity(); // calculate actual velocity
    }
    private void SetVelocity()
    {
        velocity = Vector3.zero;
        for (int i = 0; i < velocityVectorsCount; i++)
        {
            velocity += velocities[i];
        }
    }
    private void CalculateVelocity()
    {
        playerVelocity = (transform.position - _lastPlayerPos) / Time.deltaTime;
        _lastPlayerPos = transform.position;
    }
    private void Move(Vector3 _velocity)
    {
        if (characterController.enabled)
        {
            characterController.Move(_velocity * Time.deltaTime);
        }
        
    }
}
