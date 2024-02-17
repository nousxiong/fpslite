using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private InputManager _inputManager;
    private Vector2 _moveInput;
    public KeyCode jump, crouch, reload, interact;
    private void Awake()
    {
        _inputManager = transform.parent.GetComponent<InputManager>();
    }
    void Update()
    {
       
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        _inputManager.moveInput = _moveInput;
        // Jumping
        if (Input.GetKeyDown(jump))
        {
            _inputManager.JumpInputDown();
        }
        if (Input.GetKeyUp(jump))
        {
            _inputManager.JumpInputUp();
        }
        // Crouching
        if (Input.GetKeyDown(crouch))
        {
            _inputManager.CrouchInputDown();
        }
        if (Input.GetKeyUp(crouch))
        {
            _inputManager.CrouchInputUp();
        }
        //Reloading 
        if (Input.GetKeyDown(reload))
        {
            _inputManager.ReloadInputDown();
        }
        if (Input.GetKeyUp(reload))
        {
            _inputManager.ReloadInputUp();
        }
        // Interacting
        if (Input.GetKeyDown(interact))
        {
            _inputManager.InteractInputDown();
        }
        if (Input.GetKeyUp(interact))
        {
            _inputManager.InteracInputUp();
        }
    }
}
