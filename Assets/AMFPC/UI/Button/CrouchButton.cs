using UnityEngine;

public class CrouchButton : MonoBehaviour
{
    private UIButton _button;
    private InputManager _inputManager;
    private void Awake()
    {
        _inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        _button = GetComponent<UIButton>();
    }
    void Start()
    {
        _button.OnButtonDown.AddListener(_inputManager.CrouchInputDown);
        _button.OnButtonUp.AddListener(_inputManager.CrouchInputUp);
    }
}
