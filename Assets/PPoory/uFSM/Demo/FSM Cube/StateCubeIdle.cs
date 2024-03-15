
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PPoory {

    [System.Serializable]
    [DisallowMultipleComponent]
    public class StateCubeIdle : BaseState {

        //--------------------------------------------------
        // Essential
        public override string GetStateName () {
            return GetType().Name;
        }

        //--------------------------------------------------
        // State Enter
        public override void OnEnter () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnEnter();            
			// ToDo ...
        }

        //--------------------------------------------------
        // State Exit
        public override void OnExit () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnExit();
            // ToDo ...
        }

        //--------------------------------------------------
        void OnTriggerEnter ( Collider other ) {

            if ( other.tag == "Player" ) {
                if ( enabled ) {
                    fsm.Event( "dance" );
                }
            }
        }
        
    }

}

