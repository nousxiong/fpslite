using UnityEngine;

namespace AMFPC.Camera.Scripts
{
    /// <summary>
    /// AimAssist off时、玩家转动视角或者移动时，相机X恢复到原始水平位置
    /// </summary>
    public class RecoverVerticalRotation : MonoBehaviour
    {
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
                return;
            }


            if (Mathf.Abs(_cameraLook.transform.localRotation.x) < 0.01f)
            {
                return;
            }

            var speed = 30f;
            if (_cameraLook.transform.localRotation.x > 0f)
            {
                speed = -speed;
            }
            _cameraLook.additionalRot.x = speed * Time.fixedDeltaTime;
        }
    }
}
