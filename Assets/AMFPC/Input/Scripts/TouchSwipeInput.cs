using UnityEngine;

public class TouchSwipeInput : MonoBehaviour
{
    public float Horizontal { get { return input.y; } }
    public float Vertical { get { return input.x; } }
    private Vector2 input = Vector2.zero;
    private Touch mytouch;
    private Touch nullTouch;
    private int cameraControlAreaTouchId;
    private InputManager _inputManager;
    private void Awake()
    {
        _inputManager = transform.parent.GetComponent<InputManager>();
    }
    private void Start()
    {
        cameraControlAreaTouchId = -1;
        Input.multiTouchEnabled = true;
        nullTouch = new Touch();
    }
    void Update()
    {
        GetTouchInput();
        _inputManager.cameraInput = input;
    }
    private void GetTouchInput()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (cameraControlAreaTouchId == -1 && touch.position.x >= Screen.width / 2)
                { 
                    cameraControlAreaTouchId = touch.fingerId; mytouch = touch; 
                }
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.fingerId == cameraControlAreaTouchId)
                { 
                    mytouch = touch; 
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (touch.fingerId == cameraControlAreaTouchId)
                { 
                    cameraControlAreaTouchId = -1; mytouch = nullTouch; 
                }
            }
            if (touch.phase == TouchPhase.Stationary)
            {
                if (touch.position.x >= Screen.width / 2)
                { 
                    mytouch = nullTouch; 
                }
            }
        }
        input.y = Mathf.Lerp(input.y, mytouch.deltaPosition.x, 25 * Time.deltaTime);
        input.x = Mathf.Lerp(input.x, mytouch.deltaPosition.y, 25 * Time.deltaTime);
    }
}
