using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PPoory {

    public class Actor : MonoBehaviour {

        public static Actor instance;
        public GameObject target;

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
