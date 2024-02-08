using HutongGames.PlayMaker;
// ReSharper disable UnusedType.Global

// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Common.PlayMaker.Actions.Inputs
{
    public class TransformInput : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("The input movement of X")]
        public FsmFloat moveX;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("The input movement of Y")]
        public FsmFloat moveY;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("The input touch of X")]
        public FsmFloat touchX;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("The input touch of Y")]
        public FsmFloat touchY;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("The input whether or not up, e.g. mouse up or touch up")]
        public FsmBool inputUp;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the input whether or not push down")]
        public FsmBool holdDown;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the input movement of X")]
        public FsmFloat storeMoveX;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the input movement of Y")]
        public FsmFloat storeMoveY;
        
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        // 是否可以平移
        bool canSidesway;

        public override void Reset()
        {
            everyFrame = false;
        }

        public override void OnEnter()
        {
            if (!everyFrame)
            {
                DoTransform();
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoTransform();
        }
        
        void DoTransform()
        {
            var mvX = moveX.Value;
            var mvY = moveY.Value;
            var tchX = touchX.Value;
            var tchY = touchY.Value;
            // X值在范围内，触发左右平移
            // if (mvX >= 0f)
            // {
            //     storeMoveX.Value = mvX is > 0.1f and < 0.7f ? 1f : 0f;
            // }
            // else
            // {
            //     storeMoveX.Value = mvX is > -0.7f and < -0.1f ? -1f : -0f;
            // }
            storeMoveX.Value = 0f;
            
            // Y大于一定值才会触发前后移动
            var isMvFwd = false;
            if (mvY >= 0f)
            {
                storeMoveY.Value = mvY > 0.4f ? 1f : 0f;
                isMvFwd = storeMoveY.Value != 0f;
            }
            else
            {
                storeMoveY.Value = mvY < -0.4f ? -1f : 0f;
            }

            // 只要曾有过前移，就可以平移
            if (isMvFwd)
            {
                canSidesway = true;
            }

            // 抬起输入时重置平移
            if (inputUp.Value)
            {
                canSidesway = false;
            }
            
            // 查看touch的值，如果touchX的abs值>一定值、touchY的abs值在一定范围内，触发与touch的值相反的左右平移
            if (canSidesway)
            {
                if (tchX > 0.01f)
                {
                    storeMoveX.Value = -1f;
                }
                else if (tchX < -0.01f)
                {
                    storeMoveX.Value = 1f;
                }
            }
            
        }
    }
}
