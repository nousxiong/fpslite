using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private PlayerController _playerController;
    [HideInInspector] public bool idle, walking, running, slopeSliding, sliding, climbing, crouching, inAir;
    private void Awake()
    { 
        _playerController = GetComponent<PlayerController>(); 
    }
    void Update()
    {
        if (sliding || climbing || slopeSliding)
        {
            inAir = false;
            idle = false;
            running = false;
            walking = false;
            return;
        }
        Vector2 _xzVelocity = Vector2.zero;
        _xzVelocity.x = Mathf.Abs(_playerController.playerVelocity.x) ;
        _xzVelocity.y = Mathf.Abs(_playerController.playerVelocity.z);
        float _speed = _playerController.defaultMovement.speed;
        float _walkSpeed = _playerController.defaultMovement.walkSpeed;
        float _runSpeed = _playerController.defaultMovement.runSpeed;
        if (_playerController.collisions.isGrounded && Mathf.Round(_xzVelocity.magnitude) >= _playerController.defaultMovement.walkSpeed)
        {
            idle = false; 
            inAir = false;
            if (_speed == _walkSpeed)
            { 
                walking = true; 
                running = false; 
            }

            else if (_speed == _runSpeed)
            { 
                running = true; 
                walking = false; 
            }
        }
        else
        {
            if(!_playerController.collisions.isGrounded)
            { 
                idle = false; 
                inAir = true; 
            }
            else
            { 
                idle = true; 
                inAir = false; 
            }

            running = false; 
            walking = false;
        }
    }
}
