using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [HideInInspector] public bool mechanicEnabled = true;
    private PlayerController _playerController;
    [Range(0,-100)] public float gravityForce;
    private Vector3 _gravityVector;
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

        _playerController.collisions.OnLand += OnLandResetGravity;
    }
    void Update()
    {
        ManageGravity();
    }
    private void ManageGravity()
    {
        if (!mechanicEnabled) 
        { 
            SetVelocityVector(Vector3.zero) ; 
            _gravityVector = Vector3.zero; 
            return; 
        }
        if (!_playerController.collisions.isGrounded)
        {
            _gravityVector += gravityForce * Time.deltaTime * Vector3.up;
            SetVelocityVector(_gravityVector);
        }
        else
        {
            SetVelocityVector(Vector3.zero);
        }
    }
    private void OnLandResetGravity()
    {
        _gravityVector = Vector3.zero;
        SetVelocityVector(_gravityVector);
    }
    public void SetVelocityVector(Vector3 vector)
    {
        _playerController.velocities[_mechanicVectorIndex] = vector;
    }
    public void ResetGravity()
    {
        _gravityVector = Vector3.zero;
    }
}
