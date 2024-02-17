using UnityEngine;

public class Jumping : MonoBehaviour
{
    [HideInInspector] public bool mechanicEnabled = true;
    private PlayerController _playerController;
    [Range(0,25)] public int airJumps;
    [Range(0, 50)] public float jumpVelocity;
    public bool jumpWhileCrouching;
    [HideInInspector] public  Vector3 jumpDirection;
    [HideInInspector] public bool forceJump;

    private int _mechanicIndex;
    private Vector3 _jumpVector;
    private int _airJumps;

    private CharacterController _CC;
    public delegate void PlayerJump();
    public event PlayerJump OnJump;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _CC = _playerController.characterController;
    }
    void Start()
    {
        jumpDirection = Vector3.up;
        mechanicEnabled = true;
        _playerController.inputManager.onJumpInputDown.AddListener(Jump);
        _mechanicIndex = _playerController.velocityVectorsCount;
        _playerController.velocityVectorsCount++;
        _airJumps = airJumps;
        GetComponent<HealthManager>().OnDeath.AddListener(() => SetMechanic(false));
        _playerController.collisions.OnLand += ResetJumping;
    }
    public void Jump()
    {
        if (!mechanicEnabled) 
        {
            SetVelocityVector(_jumpVector);
            return;
        }
        float _Threshhold = (_playerController.CCInitialHeight / 2 + _CC.skinWidth);
        bool _spaceAboveClear = _playerController.collisions.aboveDistance >= _Threshhold;
        if (((jumpWhileCrouching && _playerController.playerState.crouching) || !_playerController.playerState.crouching))
        {
            if ((_playerController.collisions.isGrounded && _spaceAboveClear) || forceJump)
            {
                _jumpVector = jumpVelocity * jumpDirection;
                SetVelocityVector(_jumpVector);
                OnJump();
                return;
            }
            else if (_airJumps > 0 && _spaceAboveClear)
            {
                _playerController.gravity.ResetGravity();
                _jumpVector = jumpVelocity * jumpDirection;
                SetVelocityVector(_jumpVector);
                _airJumps--;
                OnJump();
                return;
            }
        }
    }
    public void ResetJumping()
    {
        _jumpVector = Vector3.zero;
        _airJumps = airJumps;
        SetVelocityVector(_jumpVector);
    }
    public void SetVelocityVector(Vector3 vector)
    {
        _playerController.velocities[_mechanicIndex] = vector;
    }
    private void SetMechanic(bool value)
    {
        mechanicEnabled = value;
    }
}
