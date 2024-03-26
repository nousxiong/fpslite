using UnityEngine;

namespace AMFPC.Camera.Scripts
{
    /// <summary>
    /// AimAssist off时、玩家转动视角或者移动时，相机X恢复到原始水平位置
    /// </summary>
    public class RecoverVerticalRotation : MonoBehaviour
    {
        public float sensitivity = 10f;
        CameraLook _cameraLook;
        InputManager _inputManager;
        bool _off;
        
        void Awake()
        {
            _inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        }
        
        void Start()
        {
            _cameraLook = GetComponent<CameraLook>();
            _inputManager.onAimAssistEnabled.AddListener(AimAssistEnable);
            _inputManager.onAimAssistDisabled.AddListener(AimAssistDisable);
        }
        
        void AimAssistEnable()
        {
            _off = true;
            _cameraLook.additionalRot = Vector2.zero;
        }
        
        void AimAssistDisable()
        {
            _off = false;
        }

        void Update()
        {
            if (_off)
            {
                return;
            }

            if (!_cameraLook.IsVerticalRotating)
            {
                // 如果没有转动，不需要恢复
                _cameraLook.additionalRot = Vector2.zero;
                return;
            }


            if (Mathf.Abs(_cameraLook.transform.localRotation.x) < 0.01f)
            {
                // 移动到0度，结束
                _cameraLook.additionalRot = Vector2.zero;
                return;
            }

            // 根据相机Y轴（左右）旋转输入，来确定恢复的速度
            var rot = Mathf.Abs(_inputManager.cameraInput.y * sensitivity * Time.fixedDeltaTime);
            if (_cameraLook.transform.localRotation.x > 0f)
            {
                rot = -rot;
            }
            _cameraLook.additionalRot.x = rot;
        }
    }
}
