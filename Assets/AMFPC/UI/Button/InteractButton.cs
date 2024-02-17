using UnityEngine;

public class InteractButton : MonoBehaviour
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
        _button.OnButtonDown.AddListener(_inputManager.InteractInputDown);
        _button.OnButtonUp.AddListener(_inputManager.InteracInputUp);
    }

}
