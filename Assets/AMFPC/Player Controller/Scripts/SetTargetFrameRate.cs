using UnityEngine;

public class SetTargetFrameRate : MonoBehaviour
{
    public int targetFrameRate;
    private void Start()
    { 
        Application.targetFrameRate = Mathf.Abs(targetFrameRate); 
    }
}
