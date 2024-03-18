using UnityEngine;
using UnityEngine.EventSystems;
// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
namespace Fpslite.AMFPC.Inputs
{
    public class UnityEditorScreenSwipeCombineInput : MonoBehaviour
    {
        public KeyCode hold;
        public float Horizontal => input.y;
        public float Vertical => input.x;
        public bool InputUp { get; private set; }
        public bool InputDown => !InputUp;
        public bool Enabled { get; private set; }
        public bool Hold { get; private set; }

        Vector2 input = Vector2.zero;
        // InputManager inputManager;
        Vector3 lastPos, deltaPos;
        [Range(0, 2)] public float sensitivity;
        bool mouseOverUI;
        
        void Awake()
        {
#if (UNITY_EDITOR)
            Enabled = true;
#endif
        }
        
        void Update()
        {
#if (UNITY_EDITOR)
            InputUp = false;
            Hold = Input.GetKey(hold);
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    // mouseOverUI = true;
                }
                lastPos = Input.mousePosition;
                InputUp = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                mouseOverUI = false;
                InputUp = true;
            }

            if (!mouseOverUI)
            {
                if (Input.GetMouseButton(0))
                {
                    deltaPos = lastPos - Input.mousePosition;
                    input.y = Mathf.Lerp(input.y, -deltaPos.x * sensitivity,15 * Time.deltaTime);
                    input.x = Mathf.Lerp(input.x, -deltaPos.y * sensitivity, 15 * Time.deltaTime);
                }
                else
                {
                    input = Vector2.zero;
                }
                lastPos = Input.mousePosition;
            }
#endif

        }
    }
}
