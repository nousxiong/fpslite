using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
public class JoystickCombine : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float Horizontal => input.x;
    public float Vertical => input.y;
    // InputManager _inputManager;

    public float HandleRange
    {
        get => handleRange;
        set => handleRange = Mathf.Abs(value);
    }

    [FormerlySerializedAs("_handleRange"),SerializeField] float handleRange = 1;
    RectTransform background, handle;
    Canvas canvas;
    // Camera _camera;

    Vector2 input = Vector2.zero;
    // void Awake()
    // {
    //     // _inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    // }
    void Start()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
        HandleRange = handleRange;
        canvas = GetComponentInParent<Canvas>();

        var center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
    }
    // void Update()
    // {
    //     // Vector2 _input = Vector2.zero;
    //     // _input.x = Horizontal;
    //     // _input.y = Vertical;
    //     // _inputManager.moveInput = _input;
    // }
    public void OnPointerDown(PointerEventData eventData)
    { 
        OnDrag(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    { 
        input = Vector2.zero; 
        handle.anchoredPosition = Vector2.zero; 
    }
    public void OnDrag(PointerEventData eventData)
    {
        Camera cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera) { cam = canvas.worldCamera;}

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        HandleInput(input.magnitude, input.normalized);
        handle.anchoredPosition = input * radius * handleRange;
    }
    void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > 1) { input = normalised; } 
    }
}
