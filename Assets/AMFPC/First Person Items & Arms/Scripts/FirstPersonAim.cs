using UnityEngine;

public class FirstPersonAim : MonoBehaviour
{
    [Header("Aim Settings")]
    [Range(0, 180)] public float aimFieldOfView;
    [Range(0, 10)] public float aimTransitionDuration;
    private float _aimTransitionTimer;
    [HideInInspector] public bool isAiming;
    [HideInInspector] public bool holdAim;
    [Range(0, 2)] public float aimSensitivityMultiplier;
    [HideInInspector] public bool canAim,sniper;
    public Vector3 aimPosition;
    private Vector3 _initialPosition,_targetPosition;
    private ItemManager _itemManager;
    private FirstPersonSway _firstPersonSway;
    private WeaponObject _weaponItem;


    void Start()
    {
        canAim = true;
        isAiming = false;
        _firstPersonSway = transform.GetChild(0).GetComponent<FirstPersonSway>();
        _itemManager = transform.parent.GetComponent<ItemManager>();
        _initialPosition = transform.localPosition;
        _aimTransitionTimer = aimTransitionDuration;
        _targetPosition = _initialPosition;
        _weaponItem = GetComponent<ItemSettings>().item as WeaponObject;
        if (!holdAim)
        {
            _itemManager.inputManager.onADSInputDown.AddListener(() => SwitchAiming(!isAiming));
        }
    }
    void Update()
    {
        SetItemPosition();
        AimingTimer();

    }
    private void SetItemPosition()
    {
        _targetPosition = isAiming ? aimPosition : _initialPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, 20 * Time.deltaTime);
    }
    private void AimingTimer()
    {
        _aimTransitionTimer -= Time.deltaTime;
        if (_aimTransitionTimer < aimTransitionDuration / 2)
        {
            _aimTransitionTimer = aimTransitionDuration;
            _itemManager.UIReference.crosshair.SetActive(!isAiming);
            _itemManager.playerController.defaultMovement.forceWalkSpeed = isAiming;
        }
    }
    private void OnDisable()
    {
        _targetPosition = _initialPosition;
        isAiming = false;
        _firstPersonSway.AimSway(isAiming);
        _itemManager.fieldOfView.targetFOV = _itemManager.fieldOfView.initialFOV;
    }
    public void SwitchAiming(bool _aim)
    {
        if ((!canAim && _aim) || !this.gameObject.activeSelf )
        {
            return;
        }
        if (_aim)
        {
            _itemManager.audioSource.PlayOneShot(_weaponItem.aimSFX);
        }
        else
        {
            _itemManager.audioSource.PlayOneShot(_weaponItem.cancelAimSFX);
        }
        _aimTransitionTimer = aimTransitionDuration;
        isAiming = _aim;
        _itemManager.firstPersonItemHeadbob.aiming = isAiming;
        _itemManager.cameraLook.sensitivityMultiplier = isAiming ? aimSensitivityMultiplier : 1;
        _itemManager.playerController.defaultMovement.canRun = !isAiming;
        _firstPersonSway.AimSway(isAiming);
        _itemManager.fieldOfView.targetFOV = _aim ? aimFieldOfView : _itemManager.fieldOfView.initialFOV;
    }
    public void ShowFirstPersonItemGFX(bool value)
    {
        if(value)
        {
            _itemManager.secondaryCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("FPS Arms"));
        }
        else
        {
            _itemManager.secondaryCamera.cullingMask |= 1 << LayerMask.NameToLayer("FPS Arms");
        }
    }
}
