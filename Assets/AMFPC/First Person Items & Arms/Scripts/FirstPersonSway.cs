using UnityEngine;

public class FirstPersonSway : MonoBehaviour
{
    private InputManager _inputManager;
    [Range(0, 10)] public float swayAmount,rotationClamp,lerpSpeed;
    [Range(0, 1)] public float aimSwayMultiplier;
    private float swayMultiplier;
    private Vector2 _targetRot;
    [Range(0, 1)] public float xSwayMultiplier, ySwayMultiplier, zSwayMultiplier;

    void Start()
    {
        swayMultiplier = 1;
        _inputManager = transform.parent.parent.GetComponent<ItemManager>().inputManager;
    }
    void Update()
    {
        SetTargetRotations();
        Rotate(_targetRot);
    }
    private void SetTargetRotations()
    {
        float _ver = _inputManager.cameraInput.y;
        float _hor = _inputManager.cameraInput.x;
        Vector2 _rot = Vector2.zero;
        _rot.x = swayAmount * _hor * swayMultiplier;
        _rot.y = swayAmount * _ver * swayMultiplier;
        _rot = Vector2.ClampMagnitude(_rot,rotationClamp);
        _targetRot = Vector2.Lerp(_targetRot, _rot, lerpSpeed * Time.deltaTime);
    }
    public void AimSway(bool value)
    {
        swayMultiplier = value ? aimSwayMultiplier : 1;
    }
    private void Rotate(Vector2 _rot)
    {
        transform.localRotation = Quaternion.Euler(-_rot.x * xSwayMultiplier, _rot.y * ySwayMultiplier, _rot.y * zSwayMultiplier);
    }
}
