using UnityEngine;

public class MouseCameraInput : MonoBehaviour
{
    private InputManager _inputManager;
    private bool _cursorLocked;
    public KeyCode fire,ADS,inventory;
    private void Start()
    {
        _cursorLocked = false;
        ToggleLockCursor();
    }
    private void Awake()
    {
        _inputManager = transform.parent.GetComponent<InputManager>();
        _inputManager.toggleInventoryUI.AddListener(ToggleLockCursor);
    }
    void Update()
    {
        // Camera input
        _inputManager.cameraInput.x = Input.GetAxisRaw("Mouse Y");
        _inputManager.cameraInput.y = Input.GetAxisRaw("Mouse X");
        // Aim Down Sight
        if (Input.GetKeyDown(ADS))
        {
            _inputManager.AdsInputDown();
        }
        if (Input.GetKeyUp(ADS))
        {
            _inputManager.AdsInputUp();
        }
        // Firing 
        if (Input.GetKeyDown(fire))
        {
            _inputManager.FireInputDown();
        }
        if (Input.GetKeyUp(fire))
        {
            _inputManager.FireInputUp();
        }
        // Inventory UI
        if (Input.GetKeyDown(inventory))
        {
            _inputManager.ToggleInventoryUI();
        }
    }
    public void ToggleLockCursor()
    {
        _cursorLocked = !_cursorLocked;
        if (_cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}
