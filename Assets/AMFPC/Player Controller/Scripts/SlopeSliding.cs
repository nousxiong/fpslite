using UnityEngine;

public class SlopeSliding : MonoBehaviour
{
    [HideInInspector] public bool mechanicEnabled = true;
    private PlayerController _playerController;

    private Vector3 _SlopeMoveVector;
    private int _mechanicVectorIndex;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        mechanicEnabled = true;

        _mechanicVectorIndex = _playerController.velocityVectorsCount;
        _playerController.velocityVectorsCount++;
    }

    void Update()
    {
        SetSlopeMoveVector();
    }
    private void SetSlopeMoveVector()
    {
        if (!mechanicEnabled) { SetVelocityVector(Vector3.zero); return; }

        if ((_playerController.collisions.slopeAngle > _playerController.characterController.slopeLimit ))
        {
            Vector3 _slopeNormal = _playerController.collisions.slopeNormal;
            Vector3 _targetSlopeMoveVector = Vector3.Cross((Vector3.Cross(Vector3.up, _slopeNormal)), _slopeNormal);
            _SlopeMoveVector = Vector3.Lerp(_SlopeMoveVector, _targetSlopeMoveVector*20, 2 * Time.deltaTime);
            _playerController.playerState.slopeSliding = true;
        }
        else
        {
            _SlopeMoveVector = Vector3.zero;
            _playerController.playerState.slopeSliding = false;
        }
        SetVelocityVector(_SlopeMoveVector);
    }
    public void EnableMovementMechanic(bool value)
    {
        mechanicEnabled = value;
    }
    public void SetVelocityVector(Vector3 vector)
    {
        _playerController.velocities[_mechanicVectorIndex] = vector;
    }
}
