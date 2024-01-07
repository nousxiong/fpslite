using UnityEngine;
// ReSharper disable CheckNamespace

namespace HutongGames.PlayMaker.Actions
{
    /// <summary>
    /// 根据Paths信息，计算GameObject的转向范围
    /// </summary>
	[ActionCategory("Navigation")]
	public class GetClampedRotation : FsmStateAction
	{
        [RequiredField]
        [Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;
        
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The current paths")]
        public FsmArray paths;

        public override void Reset()
        {
            paths = null;
        }

		// Code that runs on entering the state.
		public override void OnEnter()
        {
		}

		// Code that runs every frame.
		public override void OnUpdate()
        {
            DoGetClampedRotation();
        }

        void DoGetClampedRotation()
        {
            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            if (paths.Length == 0)
            {
                return;
            }
            
            // 遍历paths，计算最大夹角的两个path
            
        }


	}

}
