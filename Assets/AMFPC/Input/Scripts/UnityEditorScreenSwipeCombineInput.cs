﻿using UnityEngine;
using UnityEngine.EventSystems;
// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
namespace Fpslite.AMFPC.Inputs
{
    public class UnityEditorScreenSwipeCombineInput : MonoBehaviour
    {
        public float Horizontal => input.y;
        public float Vertical => input.x;

        Vector2 input = Vector2.zero;
        // InputManager inputManager;
        Vector3 lastPos, deltaPos;
        [Range(0, 2)] public float sensitivity;
        bool mouseOverUI;
        // void Awake()
        // {
        //     // inputManager = transform.parent.GetComponent<InputManager>();
        // }
        void Update()
        {
#if (UNITY_EDITOR)
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    // mouseOverUI = true;
                }
                lastPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                mouseOverUI = false;
            }

            if (!mouseOverUI)
            {
                if (Input.GetMouseButton(0))
                {
                    deltaPos = lastPos - Input.mousePosition;
                    // inputManager.cameraInput.y = Mathf.Lerp(inputManager.cameraInput.y, -deltaPos.x * sensitivity,15*Time.deltaTime) ;
                    // inputManager.cameraInput.x = Mathf.Lerp(inputManager.cameraInput.x, -deltaPos.y * sensitivity, 15 * Time.deltaTime);
                    input.y = Mathf.Lerp(input.y, -deltaPos.x * sensitivity,15 * Time.deltaTime);
                    input.x = Mathf.Lerp(input.x, -deltaPos.y * sensitivity, 15 * Time.deltaTime);
                }
                else
                {
                    // inputManager.cameraInput = Vector2.zero;
                    input = Vector2.zero;
                }
                lastPos = Input.mousePosition;
            }
#endif

        }
    }
}
