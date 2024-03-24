using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Animator gunAnimator;
    private ItemManager _itemManager;
    private FirstPersonAim _firstPersonAim;
    private WeaponStats weaponStats;
    public ParticleSystem MuzzleFlash;
    [HideInInspector] public bool reload;
    private WeaponObject _weaponItem;
    private ParticleSystem _bulletImpact;
    private Vector3 trailEnd;
    private int TrailCycle;
    [HideInInspector] public List<LineRenderer> lineRenderers;
    [HideInInspector] public List<TrailFaide> trailsFade;
    private bool _autoFire;
    private float _landTimer;

    private void Awake()
    {
        weaponStats = GetComponent<WeaponStats>();
        _itemManager = GetComponentInParent<ItemManager>();
        _firstPersonAim = GetComponent<FirstPersonAim>();
        _weaponItem = weaponStats.itemSettings.item as WeaponObject;
    }
    void Start()
    {
        SpawnBulletTrails(3);
        _itemManager.inputManager.onReloadInputDown.AddListener(ReloadWeapon);
        _bulletImpact = Instantiate(_weaponItem.hitImpact, transform.position, _weaponItem.hitImpact.transform.rotation);
        _firstPersonAim.sniper = _weaponItem.sniper;
        _itemManager.playerController.collisions.OnLand += ResetLandTimer;
        SetHoldAim();

    }
    void Update()
    {
        UpdateWeaponUI();
        Fire();
        ReloadTimer();
        ManageAnimations();
        _landTimer -= Time.deltaTime;
    }
    private void ManageAnimations()
    {
        if (weaponStats.currentFireTimer <= 0 && (weaponStats.currentReloadTime == weaponStats.weaponItem.reloadDuration))
        {
            PlayerState _playerState = _itemManager.playerController.playerState;
            if (_playerState.walking && _itemManager.playerController.collisions.isGrounded && !_playerState.crouching && !_firstPersonAim.isAiming )
            {
                gunAnimator.SetInteger("state", 3);
            }
            else if (_playerState.running && _itemManager.playerController.collisions.isGrounded && !_playerState.crouching && _landTimer < 0f)
            {
                gunAnimator.SetInteger("state", 4);
            }
            else 
            {
                gunAnimator.SetInteger("state", 0);
            }
        }
    }
    private void ReloadTimer()
    {
        if (reload)
        {
            if (weaponStats.currentReloadTime > 0)
            {
                weaponStats.currentReloadTime -= Time.deltaTime;
            }
            else
            {
                _firstPersonAim.canAim = true;
                weaponStats.RestoreMagazineCount(_weaponItem.maxMagazineSize);
                weaponStats.currentReloadTime = _weaponItem.reloadDuration;
                reload = false;
            }
        }
    }
    public void ReloadWeapon()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        if(!reload && weaponStats.ammunitionCount < _weaponItem.maxMagazineSize && weaponStats.ammoSlot.amount > 0)
        {
            reload = true;
            _firstPersonAim.SwitchAiming(false);
            _firstPersonAim.canAim = false;
            _itemManager.audioSource.PlayOneShot(_weaponItem.reloadSFX);
            gunAnimator.SetInteger("state", 2);
        }
    }
    public void PlayFireSFX()
    {
        _itemManager.audioSource.PlayOneShot(_weaponItem.fireSFX);
    }
    private void ShowBulletTrail()
    {
        trailEnd = transform.parent.position + (transform.forward * 30);
        if (TrailCycle >= lineRenderers.Count)
        {
            TrailCycle = 0;
        }
        lineRenderers[TrailCycle].SetPosition(0, transform.position);
        lineRenderers[TrailCycle].SetPosition(1, trailEnd);
        trailsFade[TrailCycle].SetAlpha(1);
        TrailCycle++;
    }
    private void OnEnable()
    {
        _firstPersonAim.canAim = true;
        _itemManager.aimAssist.strength = _weaponItem.aimAssistStrength;
        gunAnimator.Play("Idle", -1, 0);
        reload = false;
        weaponStats = GetComponent<WeaponStats>();
        if (_itemManager.UIReference.FPItemInfoUI == null)
        { 
            _itemManager.UIReference.FPItemInfoUI = GameObject.FindGameObjectWithTag("WeaponInfoUI").GetComponent<FPItemInfoUI>();
        }
        _itemManager.UIReference.FPItemInfoUI.gameObject.SetActive(true);

        _itemManager.UIReference.FPItemInfoUI.ammunitionIcon.gameObject.SetActive(true);
        weaponStats.currentReloadTime = _weaponItem.reloadDuration;
    }
    public void Fire()
    {
        _autoFire = _weaponItem.automaticFire && _itemManager.cameraRayCast.damageableDetected;
        bool _fire = _autoFire || _itemManager.inputManager.fire;
        if (weaponStats.currentFireTimer > 0)
        {
            weaponStats.currentFireTimer -= Time.deltaTime;
        }
        if (reload || _itemManager.playerController.playerState.running || _itemManager.switching) 
        {
            return;
        }
        if (weaponStats.ammunitionCount <= 0 && _fire)
        {
            ReloadWeapon();
            return;
        }
        if (_fire)
        {
            if (weaponStats.currentFireTimer <= 0)
            {
                ShowBulletTrail();
                PlayFireSFX();
                MuzzleFlash.Play();
                weaponStats.SubstractFiredAmmo(1);
                weaponStats.currentFireTimer = _weaponItem.timeBetweenShots;
                ShowBulletImpact();
                gunAnimator.Play("Fire", -1, 0);
                gunAnimator.SetInteger("state",1);
                if (_itemManager.cameraRayCast.damageableDetected)
                {
                    _itemManager.cameraRayCast.damageable.Damage(_weaponItem.weaponDamage);
                }
            }
        }
    }
    public void ShowBulletImpact()
    {
        _bulletImpact.transform.position = _itemManager.cameraRayCast.hit.point;
        _bulletImpact.Play();
    }
    private void SetHoldAim()
    {
        if (_weaponItem.holdAds)
        {
            _itemManager.inputManager.onAdsInputUp.AddListener(() => _firstPersonAim.SwitchAiming(false));
        }
    }
    private void SpawnBulletTrails(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject bulleTrailEffect = Instantiate(_weaponItem.trail.gameObject, transform.position, Quaternion.identity);
            LineRenderer LineRenderer = bulleTrailEffect.GetComponent<LineRenderer>();
            LineRenderer.SetPosition(0, transform.position);
            LineRenderer.SetPosition(1, trailEnd);
            lineRenderers.Add(LineRenderer);
            trailsFade.Add(bulleTrailEffect.GetComponent<TrailFaide>());
        }
    }
    private void UpdateWeaponUI()
    {
        _itemManager.UIReference.FPItemInfoUI.itemIcon.sprite = _weaponItem.itemIcon;
        _itemManager.UIReference.FPItemInfoUI.ammunitionIcon.sprite = _weaponItem.ammunitionType.itemIcon;
        _itemManager.UIReference.FPItemInfoUI.ammunitionCount.text = weaponStats.ammunitionCount.ToString();
        _itemManager.UIReference.FPItemInfoUI.totalAmmunition.text = weaponStats.ammoSlot.amount.ToString();
        _itemManager.UIReference.FPItemInfoUI.itemName.text = _weaponItem.itemName.ToString();
    }
    private void ResetLandTimer()
    {
        _landTimer = 0.2f;
    }
}
