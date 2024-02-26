using UnityEngine;

namespace Common.Launch
{
    public class Launcher : MonoBehaviour
    {
        static Resolution resolution;

        void Awake()
        {
            Time.timeScale = 1f;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            resolution = Screen.currentResolution;
            
            if (Application.isMobilePlatform)
            {
                var memorySize = SystemInfo.systemMemorySize / 1024f;
                Debug.Log($"Device memorySize:{memorySize:F}GB, resolution:{resolution.width}*{resolution.height}");

                SetResolution();
            }
        }

        void Update()
        {
            SetResolution();
        }

        void SetResolution()
        {
            if (Screen.width < Screen.height)
            {
                // 竖屏
                resolution = Screen.currentResolution;
                var targetWidth = resolution.width > 720 ? 720 : 640;
                if (targetWidth != Screen.currentResolution.width)
                {
                    var targetHeight = Mathf.RoundToInt((float)targetWidth * Screen.height / Screen.width);
                    Screen.SetResolution(targetWidth, targetHeight, true);
                    Debug.Log($"Set device resolution to {targetWidth}*{targetHeight}");
                }
            }
            else
            {
                // 横屏
                resolution = Screen.currentResolution;
                var targetHeight = resolution.height > 720 ? 720 : 640;
                if (targetHeight != Screen.currentResolution.height)
                {
                    var targetWidth = Mathf.RoundToInt((float)targetHeight * Screen.width / Screen.height);
                    Screen.SetResolution(targetWidth, targetHeight, true);
                    Debug.Log($"Set device resolution to {targetWidth}*{targetHeight}");
                }
            }
        }
    }
}
