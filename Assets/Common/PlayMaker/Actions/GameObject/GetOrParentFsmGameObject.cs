// ReSharper disable CheckNamespace
namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GameObject)]
    public class GetOrParentFsmGameObject : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject that owns the FSM.")]
        public FsmOwnerDefault gameObject;
		
        [UIHint(UIHint.FsmName)]
        [Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
		
        [RequiredField]
        [UIHint(UIHint.FsmGameObject)]
        [Tooltip("The name of the FSM variable to get.")]
        public FsmString variableName;
		
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the value in a GameObject variable in this FSM.")]
        public FsmGameObject storeValue;
    }
}
