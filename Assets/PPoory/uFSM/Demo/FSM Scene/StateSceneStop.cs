using System.Reflection;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace PPoory.uFSMs.Demo.FSM_Scene {

    [System.Serializable]
    [DisallowMultipleComponent]
    public class StateSceneStop : BaseState {

        // Essential
        public override string GetStateName () {
            return GetType().Name;
        }

        // State Enter
        public override void OnEnter () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnEnter();

            Actor.instance.GetComponent<uFSM>().Event( "idle" );
        }

        // State Exit
        public override void OnExit () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnExit();
            // ToDo ...
        }

        // MonoBehaviour Function is called when this state is activated.
        void Start () {
        }

        // MonoBehaviour Function is called when this state is activated.
        void FixedUpdate () {
        }

        // MonoBehaviour Function is called when this state is activated.
        void Update () {
        }

        // MonoBehaviour Function is called when this state is activated.
        void LateUpdate () {
        }

    }

}

