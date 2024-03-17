using UnityEngine;
using UnityEngine.EventSystems;

public class UnityEditorScreenSwipeInput : MonoBehaviour
{
    private InputManager _inputManager;
    private Vector3 _lastPos, _deltaPos;
    [Range(0, 2)] public float sensitivity;
    private bool _mouseOverUI;
    AimAssist _aimAssist;
    private void Awake()
    {
        _inputManager = transform.parent.GetComponent<InputManager>();
        _aimAssist = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AimAssist>();
    }
    void Update()
    {
    #if (UNITY_EDITOR)
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                _mouseOverUI = true;
            }
            _lastPos = Input.mousePosition;
            _aimAssist.enabled = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseOverUI = false;
            _aimAssist.enabled = true;
        }

        if (!_mouseOverUI)
        {
            if (Input.GetMouseButton(0))
            {
                _deltaPos = _lastPos - Input.mousePosition;
                _inputManager.cameraInput.y = Mathf.Lerp(_inputManager.cameraInput.y, -_deltaPos.x * sensitivity,15*Time.deltaTime) ;
                _inputManager.cameraInput.x = Mathf.Lerp(_inputManager.cameraInput.x, -_deltaPos.y * sensitivity, 15 * Time.deltaTime);
            }
            else
            {
                _inputManager.cameraInput = Vector2.zero;
            }
            _lastPos = Input.mousePosition;
        }
    #endif

    }
}
