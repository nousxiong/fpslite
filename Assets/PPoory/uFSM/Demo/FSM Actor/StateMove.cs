
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

namespace PPoory {

    [DisallowMultipleComponent]
    public class StateMove : BaseState {

        private float offset = 1.0f;

        NavMeshAgent navAgent;
        NavMeshAgent NavAgent {
            get {
                if ( null == navAgent ) {
                    navAgent = fsm.ownerObj.GetComponent<NavMeshAgent>();
                }
                return navAgent;
            }
        }

        public override void OnEnter () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnEnter();

            NavAgent.isStopped = false;
        }

        public override void OnExit () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnExit();

            NavAgent.isStopped = true;
        }

        public override string GetStateName () {
            return GetType().Name;
        }

        void Move() {

            Vector3 actorPos = fsm.ownerObj.transform.position;
            Vector3 targetPos = fsm.ownerObj.GetComponent<Actor>().target.transform.position;
            
            float distance = ( targetPos - actorPos ).magnitude;

            if ( distance > offset ) {
                NavAgent.destination = targetPos;
            }
            else {
                NavAgent.isStopped = true;
                fsm.Event( "find" );
            }
        }

        // MonoBehaviour Function is called when this state is activated.
        void Start () {
        }

        // MonoBehaviour Function is called when this state is activated.
        void FixedUpdate () {
        }

        // MonoBehaviour Function is called when this state is activated.
        void Update () {
            Move();
        }

        // MonoBehaviour Function is called when this state is activated.
        void LateUpdate () {
        }

    }

}