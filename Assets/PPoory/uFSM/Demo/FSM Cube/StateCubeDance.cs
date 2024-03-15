
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PPoory {

    [System.Serializable]
    [DisallowMultipleComponent]
    public class StateCubeDance : BaseState {

        private Material mtrl;
        private Color originColor;
        private Vector3 originScale;
        private Coroutine coDance;

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

            mtrl = fsm.ownerObj.GetComponent<MeshRenderer>().materials[0];
            originColor = mtrl.GetColor( "_Color" );
            originScale = fsm.ownerObj.transform.localScale;

            coDance = StartCoroutine( "Dance", 3.0f );
        }

        // State Exit
        public override void OnExit () {

            if ( fsm.infoMessage ) {
                Debug.Log( string.Format( "{0}.{1}.{2}", name, GetType().Name, MethodBase.GetCurrentMethod().Name ) );
            }

            base.OnExit();

            if ( null != coDance ) {
                StopCoroutine( coDance );
            }
        }

        IEnumerator Dance ( float limit ) {

            float timer = 0.0f;

            // Set Color
            mtrl.SetColor( "_Color", Color.yellow );

            Vector3 maxScale = new Vector3( 2.0f, 2.0f, 2.0f );
            Vector3 minScale = new Vector3( 0.5f, 0.5f, 0.5f );
            Vector3 val = new Vector3( 3.0f, 3.0f, 3.0f );

            Transform trans = fsm.ownerObj.transform;

            while ( true ) {

                timer += Time.deltaTime;
                if ( timer > limit ) {
                    break;
                }

                trans.localScale += val * Time.deltaTime;

                if ( trans.localScale.sqrMagnitude > maxScale.sqrMagnitude ) {
                    trans.localScale = maxScale;
                    val *= -1;
                }
                else if ( trans.localScale.sqrMagnitude < minScale.sqrMagnitude ) {
                    trans.localScale = minScale;
                    val *= -1;
                }

                yield return null;
            }

            mtrl.SetColor( "_Color", originColor );
            trans.transform.localScale = originScale;

            fsm.Event( "finish" );
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

