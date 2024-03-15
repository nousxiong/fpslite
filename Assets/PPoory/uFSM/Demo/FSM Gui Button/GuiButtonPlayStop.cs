
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using PPoory;

public class GuiButtonPlayStop : MonoBehaviour {

    public Image btnImage;
    public Text btnText;

    [HideInInspector]
    public Color originColor;
    public Color stopColor;

    public void Awake () {
        originColor = btnImage.color;
    }

    public void OnClickPlayStop () {

        uFSM fsm = GetComponent<uFSM>();

        if ( null == fsm.currState ) {
            Debug.LogWarning( "Current State is NULL !!" );
            return;
        }

        if ( "StateBtnPlay" == fsm.currState.GetStateName() ) {
            fsm.Event( "stop" );
        }
        else if ( "StateBtnStop" == fsm.currState.GetStateName() ) {
            fsm.Event( "play" );
        }
    }	
}

