using UnityEngine;

public class FireButton : MonoBehaviour
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
        _button.OnButtonDown.AddListener(_inputManager.FireInputDown);
        _button.OnButtonUp.AddListener(_inputManager.FireInputUp);
    }
}
