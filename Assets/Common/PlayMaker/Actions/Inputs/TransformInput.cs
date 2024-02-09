using System.Diagnostics.CodeAnalysis;
using HutongGames.PlayMaker;
// ReSharper disable UnusedType.Global

// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Common.PlayMaker.Actions.Inputs
{
    [SuppressMessage("ReSharper", "UnusedVariable")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
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
        public FsmFloat tranMoveX;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the input movement of Y")]
        public FsmFloat tranMoveY;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the input touch of X")]
        public FsmFloat tranTouchX;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the input touch of Y")]
        public FsmFloat tranTouchY;
        
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
            UsingTouchToRotation();
        }

        bool TransformVertical(float mvY)
        {
            // Y大于一定值才会触发前后移动
            var isMvFwd = false;
            if (mvY >= 0f)
            {
                tranMoveY.Value = mvY > 0.4f ? 1f : 0f;
                isMvFwd = tranMoveY.Value != 0f;
            }
            else
            {
                tranMoveY.Value = mvY < -0.4f ? -1f : 0f;
            }
            return isMvFwd;
        }

        void TransformSidesway(bool isMvFwd)
        {
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
        }
        
        void UsingJoystickToRotation()
        {
            tranMoveX.Value = moveX.Value;
            tranMoveY.Value = moveY.Value;
        }
        
        void UsingTouchToRotation()
        {
            var mvX = moveX.Value;
            var mvY = moveY.Value;
            var tchX = touchX.Value;
            var tchY = touchY.Value;
            tranMoveX.Value = 0f;
            
            var isMvFwd = TransformVertical(mvY);

            TransformSidesway(isMvFwd);
            
            // 查看touch的值，如果touchX的abs值>一定值、touchY的abs值在一定范围内，触发与touch的值相反的左右平移
            if (canSidesway)
            {
                tranMoveX.Value = tchX switch
                {
                    > 0.01f => -1f,
                    < -0.01f => 1f,
                    _ => tranMoveX.Value
                };
            }
        }
    }
}
