using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static JoystickHandle Instance;
    
    public Transform JoystickTransform;
    
    public Transform HandleTransform;

    Vector2 _pointDownPos;

    public float MaxRadius;

    public Vector2 InputVector2;

    Vector3 _originJoystickPos;

    RectTransform _rectTransform;

    int _touchId;

    void Awake()
    {
        Instance = this;
        _rectTransform = transform as RectTransform;
    }

    void Start()
    {
        if (JoystickTransform)
        {
            _originJoystickPos = JoystickTransform.localPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_touchId != 0)
        {
            return;
        }
        _touchId = eventData.pointerId;
        
        _pointDownPos = eventData.position;
        if (_rectTransform)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, _pointDownPos, null, out _pointDownPos);
        }

        if (JoystickTransform)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickTransform.parent as RectTransform, eventData.position, null, out var localPos);
            JoystickTransform.localPosition = localPos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_touchId != eventData.pointerId)
        {
            return;
        }
        
        var touchPoint = eventData.position;
        if (_rectTransform)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, touchPoint, null, out touchPoint);
        }
        var dis = touchPoint - _pointDownPos;
        var magnitude = dis.magnitude;
        magnitude = Mathf.Clamp(magnitude, 0f, MaxRadius);
        var normalized = magnitude * dis.normalized;
        HandleTransform.localPosition = normalized;
        InputVector2 = normalized.normalized * magnitude / MaxRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_touchId != eventData.pointerId)
        {
            return;
        }

        _touchId = 0;
        HandleTransform.localPosition = Vector3.zero;
        InputVector2 = Vector2.zero;

        if (JoystickTransform)
        {
            JoystickTransform.localPosition = _originJoystickPos;
        }
    }

}
