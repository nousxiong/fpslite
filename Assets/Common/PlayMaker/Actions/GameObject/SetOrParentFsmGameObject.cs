using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	public class SetOrParentFsmGameObject : FsmStateAction
	{
        [UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
		
        [RequiredField]
        [UIHint(UIHint.FsmGameObject)]
        [Tooltip("The name of the FSM variable.")]
        public FsmString variableName;
        
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The GameObject Variable to set.")]
        public FsmGameObject variable;

        // Note: NOT a required field since can set to null
        [Tooltip("Set the variable value. NOTE: leave empty to set to null.")]
        public FsmGameObject setValue;

        [Tooltip("event when set the value of the variable in the parent")]
        public FsmEvent parentEvent;

        [Tooltip("event when set the value of the variable self")]
        public FsmEvent selfEvent;
        
        public override void Reset()
        {
            fsmName = "";
            variableName = "";
            variable = null;
            setValue = null;
            parentEvent = null;
            selfEvent = null;
        }

		// Code that runs on entering the state.
		public override void OnEnter()
        {
            DoSetOrParentFsmGameObject();
			Finish();
		}

        void DoSetOrParentFsmGameObject()
        {
            if (!DoSetParentFsmGameObject())
            {
                DoSetGameObject();
                Fsm.Event(selfEvent);
            }
            else
            {
                Fsm.Event(parentEvent);
            }
        }

        void DoSetGameObject()
        {
            variable.Value = setValue?.Value;
        }

        bool DoSetParentFsmGameObject()
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
			
            FsmGameObject fsmGameObject = fsm.FsmVariables.FindFsmGameObject(variableName.Value);
            if (fsmGameObject == null)
            {
                return false;
            }
            fsmGameObject.Value = setValue?.Value;
            return true;
        }

	}

}
