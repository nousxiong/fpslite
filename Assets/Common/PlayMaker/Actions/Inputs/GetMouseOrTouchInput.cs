#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
#define NEW_INPUT_SYSTEM_ONLY
#endif

using UnityEngine;

#if NEW_INPUT_SYSTEM_ONLY
using UnityEngine.InputSystem;
#endif

using HutongGames.PlayMaker;
// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Common.PlayMaker.Actions.Inputs
{
    public class GetMouseOrTouchInput : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the input movement of X")]
        public FsmFloat moveX;
        
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the input movement of Y")]
        public FsmFloat moveY;

        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the input whether or not up, e.g. mouse up or touch up")]
        public FsmBool inputUp;

        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the input whether or not push down")]
        public FsmBool holdDown;
        
        [HutongGames.PlayMaker.Tooltip("是否需要鼠标按下才获取鼠标移动.")]
        public bool mouseDown = true;
        
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        
        bool curInputDown;
        
        public override void Reset()
        {
            mouseDown = true;
            everyFrame = false;
            curInputDown = false;
        }

        public override void OnEnter()
        {
            if (!everyFrame)
            {
                DoGetMouseOrTouchInput();
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoGetMouseOrTouchInput();
        }
        
        void DoGetMouseOrTouchInput()
        {
            if (Application.isMobilePlatform)
            {
                GetTouchInput();
            }
            else
            {
                GetMouseInput();
            }
        }

        void GetTouchInput()
        {
            moveX.Value = 0f;
            moveY.Value = 0f;
            inputUp.Value = false;
            holdDown.Value = false;
#if NEW_INPUT_SYSTEM_ONLY
            var touchCount = Touchscreen.current != null ? Touchscreen.current.touches.Count : 0;
            if (touchCount > 0)
            {
                curInputDown = true;
                holdDown.Value = true;
                var touch = Touchscreen.current.touches[0];
                if (touch.phase == TouchPhase.Moved)
                {
                    var deltaPosition = touch.delta.ReadValue();
                    moveX.Value = deltaPosition.x;
                    moveY.Value = deltaPosition.y;
                }
                return;
            }
#else
            if (Input.touchCount > 0)
            {
                curInputDown = true;
                holdDown.Value = true;
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 deltaPosition = touch.deltaPosition;
                    moveX.Value = deltaPosition.x;
                    moveY.Value = deltaPosition.y;
                }
                return;
            }
#endif
            // input up
            if (curInputDown)
            {
                inputUp.Value = true;
                curInputDown = false;
            }
        }

        void GetMouseInput()
        {
            moveX.Value = 0f;
            moveY.Value = 0f;
            inputUp.Value = false;
            holdDown.Value = false;
#if NEW_INPUT_SYSTEM_ONLY
            if (Mouse.current != null) 
            {
                curInputDown = true;
                holdDown.Value = true;
                // fudge factor accounts for sensitivity of old input system
                var deltaRo = Mouse.current.delta.ReadValue();
                moveX.Value = deltaRo.x * 0.05f;
                moveY.Value = deltaRo.y * 0.05f;
                return;
            }
			
#else
            if (!mouseDown || Input.GetMouseButton(0))
            {
                curInputDown = true;
                holdDown.Value = true;
                moveX.Value = Input.GetAxis("Mouse X");
                moveY.Value = Input.GetAxis("Mouse Y");
                return;
            }
#endif
            // input up
            if (curInputDown)
            {
                inputUp.Value = true;
                curInputDown = false;
            }
        }
    }
}
