using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable UnassignedField.Global

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Navigation")]
	[Tooltip("如果有符合规定的Parent，则++parent fsm上的stayRefCnt；否则++自己的")]
	public class AddOrParentInt : FsmStateAction
	{
        [UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
		
        [RequiredField]
        [UIHint(UIHint.FsmInt)]
        [Tooltip("The name of the FSM variable.")]
        public FsmString variableName;
        
        [Tooltip("The int value to add.")]
        public FsmInt addValue;
        
        [UIHint(UIHint.Variable)]
        [Tooltip("The result after add; Also as self var to add")]
        public FsmInt intVar;
        
        [Tooltip("Event when add the value of the variable in the parent")]
        public FsmEvent parentEvent;

        [Tooltip("Event when add the value of the variable self")]
        public FsmEvent selfEvent;

		// Code that runs on entering the state.
		public override void OnEnter()
		{
            if (DoAddParentInt())
            {
                Fsm.Event(parentEvent);
            }
            else
            {
                intVar.Value += addValue.Value;
                Fsm.Event(selfEvent);
            }
			Finish();
		}

        bool DoAddParentInt()
        {
            Transform parent = Owner.transform.parent;
            if (parent == null)
            {
                return false;
            }
            
            PlayMakerFSM fsm = ActionHelpers.GetGameObjectFsm(parent.gameObject, fsmName.Value);
            if (fsm == null)
            {
                return false;
            }
            
            if (!fsm.FsmVariables.Contains(variableName.Value))
            {
                return false;
            }
            
            FsmInt parentValue = fsm.FsmVariables.GetFsmInt(variableName.Value);
            parentValue.Value += addValue.Value;
            intVar.Value = parentValue.Value;
            return true;
        }

	}

}
