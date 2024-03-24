using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedVariable
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local

namespace Fpslite.AMFPC.Inputs
{
    public class InputCombiner : MonoBehaviour
    {
        public float fwdSpeed = 1f;
        public float bwdSpeed = 0.5f;
        [Tooltip("Sideways speed")]
        public float swSpeed = 1f;
        
        UnityEditorScreenSwipeCombineInput _screenSwipe;
        TouchSwipeCombineInput _touchSwipe;
        JoystickCombine _joystick;
        InputManager _inputManager;
        
        float SwipeHorizontal => _screenSwipe.Enabled ? _screenSwipe.Horizontal : _touchSwipe.Horizontal;
        float SwipeVertical => _screenSwipe.Enabled ? _screenSwipe.Vertical : _touchSwipe.Vertical;
        bool SwipeInputUp => _screenSwipe.Enabled ? _screenSwipe.InputUp : _touchSwipe.InputUp;
        bool SwipeHold => _screenSwipe.Enabled ? _screenSwipe.Hold : _touchSwipe.Hold;

        // 是否可以平移
        bool _canSidesway;
        // 是否曾有过前移
        bool _hadFwd;
        // 当前是否抬起输入
        bool _inputUp;

        void Awake()
        {
            _screenSwipe = GameObject.FindGameObjectWithTag("ScreenSwipe").GetComponent<UnityEditorScreenSwipeCombineInput>();
            _touchSwipe = GameObject.FindGameObjectWithTag("TouchSwipe").GetComponent<TouchSwipeCombineInput>();
            _joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<JoystickCombine>();
            _inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        }

        // void Reset()
        // {
        //     Awake();
        //     canSidesway = false;
        //     hadFwd = false;
        // }

        float TransformVertical(float mvY, out bool isMvFwd, out bool isMvBwd, out bool isBwd)
        {
            // Y大于一定值才会触发前后移动
            isMvFwd = false;
            isMvBwd = false;
            isBwd = false;
            float result;
            if (mvY >= 0f)
            {
                // 如果当前hadFwd，则也触发前移
                result = mvY > 0.5f || _hadFwd ? fwdSpeed : 0f;
                isMvFwd = result != 0f;
            }
            else
            {
                isBwd = mvY is < 0f and > -0.5f;
                result = mvY < -0.5f ? -bwdSpeed : 0f;
                isMvBwd = result != 0f;
            }
            
            TransformForward(isMvFwd, isMvBwd);
            return result;
        }

        void TransformForward(bool isMvFwd, bool isMvBwd)
        {
            if (isMvFwd)
            {
                _hadFwd = true;
            }
            
            // 抬起输入或者后移时重置
            if (_inputUp || isMvBwd)
            {
                _hadFwd = false;
            }
        }
        
        float TransformSidesway(float camY, float mvX, bool isMvFwd, bool isMvBwd, bool isBwd)
        {
            // 只要曾有过前移动，就可以平移
            if (isMvFwd)
            {
                _canSidesway = true;
            }

            // 抬起输入或者向后时重置平移
            if (_inputUp || isMvBwd)
            {
                _canSidesway = false;
            }
            
            // 如果在左右旋转视角，触发与camY的值相反的左右平移
            var result = 0f;
            if (SwipeHold && _canSidesway)
            {
                result = camY switch
                {
                    > 0.01f => -swSpeed,
                    < -0.01f => swSpeed,
                    _ => mvX
                };
            }
            return result;
        }
        
        void Update()
        {
            _inputUp = SwipeInputUp;
            
            // 更新移动
            var mvX = _joystick.Horizontal;
            var mvY = _joystick.Vertical;
            var camY = SwipeHorizontal;
            Vector2 move = Vector2.zero;
            
            move.y = TransformVertical(mvY, out var isMvFwd, out var isMvBwd, out var isBwd);
            move.x = TransformSidesway(camY, mvX, isMvFwd, isMvBwd, isBwd);
            
            _inputManager.moveInput = move;
            
            // 更新视角
            Vector2 cam = Vector2.zero;
            cam.y = SwipeHorizontal;
            _inputManager.cameraInput = cam;
            
            // 瞄准辅助
            if (_inputUp)
            {
                _inputManager.AimAssistEnabled();
            }
            else
            {
                _inputManager.AimAssistDisabled();
            }
        }
    }
}
