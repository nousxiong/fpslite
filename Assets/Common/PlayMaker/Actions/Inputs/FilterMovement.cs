using HutongGames.PlayMaker;
// ReSharper disable UnassignedField.Global

namespace Common.PlayMaker.Actions.Inputs
{
    public class FilterMovement : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The input movement of X")]
        public FsmFloat moveX;
        
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The input movement of Y")]
        public FsmFloat moveY;
        
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the input movement of X")]
        public FsmFloat storeMoveX;
        
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the input movement of Y")]
        public FsmFloat storeMoveY;
        
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public override void Reset()
        {
            everyFrame = false;
        }

        public override void OnEnter()
        {
            if (!everyFrame)
            {
                DoFilterMovement();
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoFilterMovement();
        }
        
        void DoFilterMovement()
        {
            if (moveX.Value >= 0f)
            {
                storeMoveX.Value = moveX.Value is > 0.3f and < 0.7f ? 1f : 0f;
            }
            else
            {
                storeMoveX.Value = moveX.Value is > -0.7f and < -0.3f ? -1f : -0f;
            }
            
            if (moveY.Value >= 0f)
            {
                storeMoveY.Value = moveY.Value > 0.5f ? 1f : 0f;
            }
            else
            {
                storeMoveY.Value = moveY.Value < -0.5f ? -1f : 0f;
            }
        }
    }
}
