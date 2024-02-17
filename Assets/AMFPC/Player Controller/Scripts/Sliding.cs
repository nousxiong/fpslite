using UnityEngine;
using UnityEngine.Events;

public class Sliding : MonoBehaviour
{
    [HideInInspector] public bool mechanicEnabled = true;

    private PlayerController _playerController;
    private InputManager _inputManager;
    [Range(0,35)] public float slidingVelocity, slideDuration,slidingSpeedThreshold,timeBetweenSlides;
    public bool endOfSlideCancelsCrouch;

    private PlayerState _playerState;
    private Collisions _collisions;
    private bool _slide, _reset;
    private float _slideDuration,_slideTimer;
    private Vector3 _slideDirection;
    private int _mechanicIndex;
    public UnityEvent OnSlide;


    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerState = _playerController.playerState;
        _collisions = _playerController.collisions;
        _playerController.collisions.OnLand += CancelSlide;
    }
    void Start()
    {
        mechanicEnabled = true;
        _inputManager = _playerController.inputManager;
        _slideDuration = slideDuration; 
        _inputManager.onCrouchInputDown.AddListener(CheckForSlide);
        _mechanicIndex = _playerController.velocityVectorsCount;
        _playerController.velocityVectorsCount++;
    }
    void Update()
    {
        if (!mechanicEnabled) 
        {
            SetVelocityVector(Vector3.zero);
            return;
        }
        GetSlidingDirection();
        SlidingTimer();
        ManageDefaultMovement();
        CheckFrontCollisionWhileSliding();
        Slide();
    }
    private void Slide()
    {
        if (_slideTimer < timeBetweenSlides)
        {
            _slideTimer += Time.deltaTime;
        }
        if (_slide)
        {
            Vector3 _velocity = (_slideDirection * slidingVelocity) * (_slideDuration / slideDuration);
            SetVelocityVector(_velocity);
        }
    }
    private void SlidingTimer()
    {
        if (!_slide)
        {
            _playerState.sliding = false;
            return;
        }
        if (!_collisions.isGrounded)
        {
            return;
        }
        if (_playerState.crouching)
        {
            _slideDuration -= Time.deltaTime;
            _playerState.sliding = true;
        }
        else
        {
            _slideDuration = 0;
        }
        if (_slideDuration <= 0)
        {
            CancelSlide();
        }
    }
    private void CheckForSlide()
    {
        if (!mechanicEnabled)
        {
            return;
        }
        if (!_slide && !_collisions.frontCollision && _slideTimer >= timeBetweenSlides)
        {
            _slideTimer = 0;
            Vector3 _xzVel = Vector3.ProjectOnPlane(_playerController.playerVelocity,Vector3.up);
            _slide = (_xzVel.magnitude > slidingSpeedThreshold && _collisions.isGrounded);
            if (!_slide) return;
            OnSlide.Invoke();
        }
    }
    private void GetSlidingDirection()
    {
        if (_inputManager.moveInput!= Vector2.zero && !_slide)
        {
            Vector2 _input = _playerController.inputManager.moveInput;
            Vector3 _moveDir = (transform.right * _input.x + transform.forward * _input.y);
            _slideDirection = _moveDir.normalized; 
        }
    }
    private void ManageDefaultMovement()
    {
        if (_slide)
        { 
            _reset = false;
            _playerController.defaultMovement.mechanicEnabled = false;
        }
        else if (!_reset)
        { 
            _reset = true;
            _playerController.defaultMovement.mechanicEnabled = true;
        }
    }
    private void CheckFrontCollisionWhileSliding()
    { 
        if (_collisions.frontCollision && _slide) 
        { 
            CancelSlide(); 
        } 
    }
    public void CancelSlide()
    { 
        _slide = false;
        _playerState.sliding = false;
        _slideDuration = slideDuration;
        if (_playerState.crouching && endOfSlideCancelsCrouch)
        {
            _playerController.crouching.Crouch(false);
        }
        SetVelocityVector(Vector3.zero);
    }
    public void EnableMovementMechanic(bool value)
    {
        mechanicEnabled = value;
    }
    public void SetVelocityVector(Vector3 vector)
    {
        _playerController.velocities[_mechanicIndex] = vector;
    }
}
