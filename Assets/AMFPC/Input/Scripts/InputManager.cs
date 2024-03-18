using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{

    public UnityEvent onJumpInputDown, onJumpInputUp; 
    [HideInInspector] public bool jump;

    public UnityEvent onCrouchInputDown, onCrouchInputUp; 
    [HideInInspector] public bool crouch;

    public UnityEvent onInteractInputDown, onInteractInputUp; 
    [HideInInspector] public bool interact;

    public UnityEvent onFireInputDown, onFireInputUp; 
    [HideInInspector] public bool fire;

    public UnityEvent onADSInputDown, onADSInputUp; 
    [HideInInspector] public bool aim;
    public UnityEvent toggleInventoryUI;

    public UnityEvent onReloadInputDown, onReloadInputUp;
    [HideInInspector] public bool reload;
    [HideInInspector] public Vector2 moveInput, cameraInput;
    
    public UnityEvent onAimAssistEnabled, onAimAssistDisabled;
    [HideInInspector] public bool aimAssist;

    public void ADSInputDown()
    { 
        onADSInputDown.Invoke();
        aim = true; 
    }
    public void ADSInputUp()
    { 
        onADSInputUp.Invoke(); 
        aim = false; 
    }
    public void ReloadInputDown()
    { 
        onReloadInputDown.Invoke(); 
        reload = true;
    }
    public void ReloadInputUp()
    { 
        onReloadInputUp.Invoke(); 
        reload = false; 
    }
    public void AimAssistEnabled()
    {
        onAimAssistEnabled.Invoke();
        aimAssist = true;
    }
    public void AimAssistDisabled()
    {
        onAimAssistDisabled.Invoke();
        aimAssist = false;
    }
    public void FireInputDown()
    { 
        onFireInputDown.Invoke(); 
        fire = true; 
    }
    public void FireInputUp()
    { 
        onFireInputUp.Invoke(); 
        fire = false; 
    }
    public void InteractInputDown()
    { 
        onInteractInputDown.Invoke(); 
        interact = true; 
    }
    public void InteracInputUp()
    {
        onInteractInputUp.Invoke(); 
        interact = false;  
    }
    public void CrouchInputDown()
    { 
        onCrouchInputDown.Invoke(); 
        crouch = true; 
    }
    public void CrouchInputUp()
    { 
        onCrouchInputUp.Invoke(); 
        crouch = false; 
    }
    public void JumpInputDown()
    { 
        onJumpInputDown.Invoke(); 
        jump = true; 
    }
    public void JumpInputUp()
    { 
        onJumpInputUp.Invoke(); 
        jump = false; 
    }
    public void ToggleInventoryUI()
    {
        toggleInventoryUI.Invoke();
    }

}
