using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FallEvent : UnityEvent<float> { }

public class Collisions : MonoBehaviour
{
    private PlayerController _playerController;
    public LayerMask groundLayer, ignoredLayers;

    [HideInInspector] public bool isGrounded, frontCollision, aboveCollision, DownCollision;
    [HideInInspector] public float slopeAngle, slopeAngle1, aboveDistance;
    [HideInInspector] public Vector3 slopeNormal;
    public RaycastHit frontHit, aboveHit, downHit, downLineHit, groundHit;
    private float _lastYPos, _initialHeight;
    private bool _landed, _fallingStarted;
    public FallEvent onFall;
    private Vector3 _castPos;
    private CharacterController _CC;

    public delegate void landing();
    public event landing OnLand;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    void Start()
    {
        _CC = _playerController.characterController;
        _initialHeight = _CC.height;
    }

    void Update()
    {
        SetSphereCastPosition();
        CheckIfGrounded();
        GetGroundSlopeAngle();
        CheckFrontCollision();
        SphereCastAbove();
        CheckIfLanded();
        LineCastDown();
    }
    private void CheckIfGrounded()
    {
        float _rayDistance = _CC.height/2 - _CC.radius + _CC.skinWidth*2;
        isGrounded = Physics.SphereCast(_castPos, _CC.radius, Vector3.down, out groundHit, _rayDistance, groundLayer);
    }
    private void GetGroundSlopeAngle()
    {
        float _distance = _CC.skinWidth * 2 + _CC.height;
        Physics.SphereCast(_castPos, _CC.radius, Vector3.down, out downHit, _distance, groundLayer);
        slopeAngle = Vector3.Angle(Vector3.up, downHit.normal);
        slopeNormal = downHit.normal;
    }
    private void SetSphereCastPosition()
    {
        _castPos = transform.position;
    }
    private void CheckFrontCollision()
    {
        Vector3 _castpos = _castPos - (_CC.height/2- _CC.radius) * Vector3.up;
        float _distance = _CC.radius / 2;
        frontCollision = Physics.SphereCast(_castpos, _CC.radius, transform.forward, out frontHit, _distance, ~ignoredLayers);
    }
    private void SphereCastAbove()
    {
        float _distance = (_initialHeight + _CC.skinWidth) * 2;
        Physics.SphereCast(_castPos, _CC.radius, Vector3.up, out aboveHit, _distance, ~ignoredLayers);
        aboveDistance = (aboveHit.point - transform.position).magnitude - (_CC.height / 2 + _CC.skinWidth);
    }
    private void LineCastDown()
    {
        float _distance = (_initialHeight + _CC.skinWidth) * 2;
        Physics.Raycast(_castPos, Vector3.down, out downLineHit, _distance, ~ignoredLayers);
        slopeAngle1 = Vector3.Angle(Vector3.up, downLineHit.normal);
    }
    private void CheckIfLanded()
    {
        if (!_fallingStarted && !isGrounded && _playerController.playerVelocity.y < 0)
        {
            _lastYPos = transform.position.y;
            _fallingStarted = true;
        }
        if (isGrounded && !_landed)
        {
            _fallingStarted = false;
            _landed = true;
            if (transform.position.y < _lastYPos)
            {
                float _fallDistance = Mathf.Abs(transform.position.y - _lastYPos);
                OnLand();
                onFall?.Invoke(_fallDistance);
            }
            _lastYPos = transform.position.y;
        }
        else if (!isGrounded)
        {
            _landed = false;
        }
    }
}
