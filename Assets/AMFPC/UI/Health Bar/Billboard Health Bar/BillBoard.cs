using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform _cameraTransform;
    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + _cameraTransform.forward);
    }
}
