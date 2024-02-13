using System.Diagnostics.CodeAnalysis;
using HutongGames.PlayMaker;
// ReSharper disable FieldCanBeMadeReadOnly.Global

// ReSharper disable UnusedMember.Global

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

        public FsmFloat fwdSpeed = 1f;
        public FsmFloat bwdSpeed = 0.5f;
        [Tooltip("Sideways speed")]
        public FsmFloat swSpeed = 1f;
        
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        // 是否可以平移
        bool canSidesway;
        // 是否曾有过前移
        bool hadFwd;

        public override void Reset()
        {
            everyFrame = false;
            canSidesway = false;
            hadFwd = false;
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
            // var mvX = moveX.Value;
            UsingTouchToRotation();
            // 覆盖touchX，使用moveX作为旋转的值
            // tranTouchX.Value = mvX;
        }

        void TransformVertical(float mvY, out bool isMvFwd, out bool isMvBwd, out bool isBwd)
        {
            // Y大于一定值才会触发前后移动
            isMvFwd = false;
            isMvBwd = false;
            isBwd = false;
            if (mvY >= 0f)
            {
                // 如果当前hadFwd，则也触发前移
                tranMoveY.Value = mvY > 0.5f || hadFwd ? fwdSpeed.Value : 0f;
                isMvFwd = tranMoveY.Value != 0f;
            }
            else
            {
                isBwd = mvY is < 0f and > -0.5f;
                tranMoveY.Value = mvY < -0.5f ? -bwdSpeed.Value : 0f;
                isMvBwd = tranMoveY.Value != 0f;
            }
        }

        void TransformForward(bool isMvFwd, bool isMvBwd)
        {
            if (isMvFwd)
            {
                hadFwd = true;
            }
            
            // 抬起输入或者后移时重置
            if (inputUp.Value || isMvBwd)
            {
                hadFwd = false;
            }
        }

        // ReSharper disable once UnusedParameter.Local
        void TransformSidesway(bool isMvFwd, bool isMvBwd, bool isBwd)
        {
            // 只要曾有过前移动，就可以平移
            if (isMvFwd)
            {
                canSidesway = true;
            }

            // 抬起输入或者向后时重置平移
            if (inputUp.Value || isMvBwd)
            {
                canSidesway = false;
            }
        }
        
        void UsingTouchToRotation()
        {
            var mvX = moveX.Value;
            var mvY = moveY.Value;
            var tchX = touchX.Value;
            var tchY = touchY.Value;
            tranMoveX.Value = 0f;
            
            TransformVertical(mvY, out var isMvFwd, out var isMvBwd, out var isBwd);
            TransformForward(isMvFwd, isMvBwd);
            TransformSidesway(isMvFwd, isMvBwd, isBwd);
            
            // 查看touch的值，如果touchX的abs值>一定值、touchY的abs值在一定范围内，触发与touch的值相反的左右平移
            if (canSidesway)
            {
                tranMoveX.Value = tchX switch
                {
                    > 0.01f => -swSpeed.Value,
                    < -0.01f => swSpeed.Value,
                    _ => tranMoveX.Value
                };
            }
        }
    }
}
