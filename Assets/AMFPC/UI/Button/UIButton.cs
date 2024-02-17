using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
[System.Serializable]
public class ButtonDown : UnityEvent{}
[System.Serializable]
public class ButtonUp : UnityEvent{}
public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{   
    public  ButtonDown OnButtonDown;
    public  ButtonUp OnButtonUp;
    [HideInInspector] public bool PointerDown, PointerUp;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnButtonDown != null) { OnButtonDown.Invoke();}
        PointerDown = true; PointerUp = false;
    }
    //
    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnButtonUp != null) { OnButtonUp.Invoke(); }
        PointerDown = false; PointerUp = true;
    }

}
  