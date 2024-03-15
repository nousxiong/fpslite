using UnityEngine;

// ReSharper disable once CheckNamespace
namespace PPoory.uFSMs.Demo.FSM_Scene
{
    public class SceneManager : MonoBehaviour {

        public static SceneManager instance;

        void Awake () {

            if ( null == instance ) {
                instance = this;
            }
        }

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
        }

    }
}
