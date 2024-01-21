using HutongGames.PlayMaker;

namespace Common.PlayMaker.Actions.Input
{
    [ActionCategory("Input")]
    [Tooltip("Debug查看Mouse输入信息")]
    public class DebugMouseInput : FsmStateAction
    {
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the mouse input x")]
        public FsmFloat mouseX;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the mouse input y")]
        public FsmFloat mouseY;

        public override void Reset()
        {
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoDebugMouseInput();
            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoDebugMouseInput();
        }

        void DoDebugMouseInput()
        {
            mouseX.Value = UnityEngine.Input.GetAxis("Mouse X");
            mouseY.Value = UnityEngine.Input.GetAxis("Mouse Y");
        }
    }
}
