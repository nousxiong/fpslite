using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickCombine : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float Horizontal { get { return _input.x; } }
    public float Vertical { get { return _input.y; } }
    private InputManager _inputManager;

    public float HandleRange
    {
        get { return _handleRange; }
        set { _handleRange = Mathf.Abs(value); }
    }

    [SerializeField] private float _handleRange = 1;
    private RectTransform _background = null, _handle = null;
    private Canvas _canvas;
    private Camera _camera;

    private Vector2 _input = Vector2.zero;
    private void Awake()
    {
        _inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }
    void Start()
    {
        _background = GetComponent<RectTransform>();
        _handle = transform.GetChild(0).GetComponent<RectTransform>();
        HandleRange = _handleRange;
        _canvas = GetComponentInParent<Canvas>();

        Vector2 center = new Vector2(0.5f, 0.5f);
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;
    }
    private void Update()
    {
        Vector2 _input = Vector2.zero;
        // _input.x = Horizontal;
        _input.y = Vertical;
        _inputManager.moveInput = _input;
    }
    public void OnPointerDown(PointerEventData eventData)
    { 
        OnDrag(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    { 
        _input = Vector2.zero; 
        _handle.anchoredPosition = Vector2.zero; 
    }
    public void OnDrag(PointerEventData eventData)
    {
        _camera = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera) { _camera = _canvas.worldCamera;}

        Vector2 position = RectTransformUtility.WorldToScreenPoint(_camera, _background.position);
        Vector2 radius = _background.sizeDelta / 2;
        _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        HandleInput(_input.magnitude, _input.normalized);
        _handle.anchoredPosition = _input * radius * _handleRange;
    }
    private void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > 1) { _input = normalised; } 
    }
}
