using UnityEngine;

// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
namespace Fpslite.AMFPC.Inputs
{
    public class TouchSwipeCombineInput : MonoBehaviour
    {
        public float Horizontal => input.y;
        public float Vertical => input.x;
        public bool InputUp { get; private set; }
        public bool Hold { get; private set; }
        
        Vector2 input = Vector2.zero;
        Touch mytouch;
        Touch nullTouch;
        int cameraControlAreaTouchId;
        int holdTouchId;
        
        // void Awake()
        // {
        //     _inputManager = transform.parent.GetComponent<InputManager>();
        // }
        void Start()
        {
            cameraControlAreaTouchId = -1;
            holdTouchId = -1;
            Input.multiTouchEnabled = true;
            nullTouch = new Touch();
        }
        
        void Update()
        {
            GetTouchInput();
            // _inputManager.cameraInput = input;
        }
        
        void GetTouchInput()
        {
            InputUp = false;
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            InputUp = false;
                            if (cameraControlAreaTouchId == -1 /*&& touch.position.x >= Screen.width / 2*/)
                            {
                                cameraControlAreaTouchId = touch.fingerId;
                                mytouch = touch;
                            }
                            else if (holdTouchId == -1)
                            {
                                holdTouchId = touch.fingerId;
                            }
                            break;
                        }
                    case TouchPhase.Moved:
                        {
                            if (touch.fingerId == cameraControlAreaTouchId)
                            {
                                mytouch = touch;
                            }
                            break;
                        }
                    case TouchPhase.Ended:
                        {
                            InputUp = true;
                            if (touch.fingerId == cameraControlAreaTouchId)
                            {
                                cameraControlAreaTouchId = -1;
                                mytouch = nullTouch;
                            }
                            else if (touch.fingerId == holdTouchId)
                            {
                                holdTouchId = -1;
                            }
                            break;
                        }
                    case TouchPhase.Stationary:
                        {
                            // if (touch.position.x >= Screen.width / 2)
                            // {
                            //     mytouch = nullTouch;
                            // }
                            mytouch = nullTouch;
                            break;
                        }
                    case TouchPhase.Canceled:
                        break;
                    // ReSharper disable once RedundantEmptySwitchSection
                    default:
                        break;
                }
            }
            input.y = Mathf.Lerp(input.y, mytouch.deltaPosition.x, 25 * Time.deltaTime);
            input.x = Mathf.Lerp(input.x, mytouch.deltaPosition.y, 25 * Time.deltaTime);
            Hold = holdTouchId != -1;
        }

    }
}
