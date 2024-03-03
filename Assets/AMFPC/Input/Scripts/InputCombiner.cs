using UnityEngine;
// ReSharper disable CheckNamespace

namespace Fpslite.AMFPC.Inputs
{
    public class InputCombiner : MonoBehaviour
    {
        UnityEditorScreenSwipeCombineInput screenSwipe;
        JoystickCombine joystick;
        InputManager inputManager;

        void Awake()
        {
            screenSwipe = GameObject.FindGameObjectWithTag("ScreenSwipe").GetComponent<UnityEditorScreenSwipeCombineInput>();
            joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<JoystickCombine>();
            inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        }

        void Update()
        {
            // 更新移动
            Vector2 move = Vector2.zero;
            move.y = joystick.Vertical;
            inputManager.moveInput = move;
            
            // 更新相机
            Vector2 cam = Vector2.zero;
            cam.y = screenSwipe.Horizontal;
            inputManager.cameraInput = cam;
        }
    }
}
