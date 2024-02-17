using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    public Animator animator;
    private bool _doorOpen;
    private void Start()
    {
        _doorOpen = false;
    }
    public void Interact()
    {
        InteractDoor();
    }
    private void InteractDoor()
    {
        if (_doorOpen)
        {
            animator.SetInteger("State", 2);
            _doorOpen = false;
        }
        else
        {
            animator.SetInteger("State", 1);
            _doorOpen = true;
        }

    }
}
