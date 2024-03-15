
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PPoory {

    [System.Serializable]
    [DisallowMultipleComponent]
    public class StateFind : BaseState {

        public override void OnEnter () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnEnter();
        }

        public override void OnExit () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnExit();
        }

        public override string GetStateName () {
            return GetType().Name;
        }

        void Start () {
        }

        void Update () {
            FindCube();
        }

        void FindCube () {

            GameObject[] targetArray = GameObject.FindGameObjectsWithTag( "Target Cube" );
            GameObject target = fsm.ownerObj.GetComponent<Actor>().target;
            List<GameObject> targetList;

            if ( null != target ) {
                targetList = targetArray.Where( e => e != target ).Distinct().ToList();
            }
            else {
                targetList = targetArray.ToList();
            }

            if ( targetList.Count > 0 ) {
                int rand = UnityEngine.Random.Range( 0, targetList.Count );
                fsm.ownerObj.GetComponent<Actor>().target = targetList[rand];

                fsm.Event( "move" );
            }
        }
        
    }

}


