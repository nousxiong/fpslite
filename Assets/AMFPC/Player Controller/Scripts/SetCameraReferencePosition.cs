using UnityEngine;

public class SetCameraReferencePosition : MonoBehaviour
{
    private PlayerController _playerController;
    public Transform cameraReference;
    private CharacterController _CC;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    void Start()
    {
        _CC = _playerController.characterController;
    }
    void Update()
    {
        SetPosition();
    }
    private void SetPosition()
    {
        Vector3 _targetPos = transform.position + Vector3.up * (_CC.height / 2 - 0.1f);
        cameraReference.position = Vector3.Lerp(cameraReference.position, _targetPos, 25 * Time.deltaTime);
    }
}
