using UnityEngine;

public class Melee : MonoBehaviour
{
    private ItemManager _itemManager;
    private WeaponStats _weaponStats;
    public Animator animator;
    private bool fireMelee;
    private WeaponObject _weaponItem;

    void Start()
    {
        _weaponStats = GetComponent<WeaponStats>();
        _itemManager = GetComponentInParent<ItemManager>();
        _weaponItem = _weaponStats.itemSettings.item as WeaponObject;
        animator = GetComponentInChildren<Animator>();
        _weaponStats.currentFireTimer = _weaponItem.timeBetweenShots;
        _itemManager.inputManager.onFireInputDown.AddListener(Fire);
        if (_itemManager.firstPersonItemHeadbob != null)
        {
            _itemManager.firstPersonItemHeadbob.playMovementAnimations = false;
        }
    }
    void Update()
    {
        UpdateWeaponUI();
        ManageAnimations();
        FireTimer();
    }
    private void FireTimer()
    {
        if (fireMelee)
        {
            if (_weaponStats.currentFireTimer > 0)
            {
                if (_weaponStats.currentFireTimer == _weaponItem.timeBetweenShots)
                {
                    FireMelee();
                    CheckTarget();
                }
                _weaponStats.currentFireTimer -= Time.deltaTime;
            }
            else
            {
                _weaponStats.currentFireTimer = _weaponItem.timeBetweenShots;
                fireMelee = false;
            }
        }
    }
    private void ManageAnimations()
    {
        if (!fireMelee)
        {
            if (_itemManager.playerController.playerState.running)
            {
                animator.SetInteger("State", 1);
            }
            else
            {
                animator.SetInteger("State", 0);
            }
        }
    }
    public void FireMelee()
    {
        animator.SetInteger("State", 2);
        _itemManager.audioSource.PlayOneShot(_weaponItem.fireSFX);
        animator.Play("Fire", -1, 0);
    }
    private void Fire()
    {
        if (!fireMelee && this.gameObject.activeSelf)
        {
            fireMelee = true;
        }
    }
    private void CheckTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position+transform.forward, 1);
        foreach (var hitCollider in hitColliders)
        {
            IDamageable _damageable = hitCollider.GetComponent<IDamageable>();
            if (_damageable != null)
            {
                _damageable.Damage(_weaponItem.weaponDamage);
            }
        }
    }
    private void OnEnable()
    {
        if (_itemManager == null)
        {
            _itemManager = GetComponentInParent<ItemManager>();
        }
        if (_itemManager.UIReference == null)
        {
            _itemManager.UIReference = GameObject.FindGameObjectWithTag("UIReference").GetComponent<UIManager>();
        }
        _itemManager.firstPersonItemHeadbob.playMovementAnimations = false;
        _itemManager.UIReference.FPItemInfoUI.ammunitionIcon.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        _itemManager.firstPersonItemHeadbob.playMovementAnimations = true;
    }
    private void UpdateWeaponUI()
    {
        _itemManager.UIReference.FPItemInfoUI.itemIcon.sprite = _weaponItem.itemIcon;
        _itemManager.UIReference.FPItemInfoUI.ammunitionCount.text = "";
        _itemManager.UIReference.FPItemInfoUI.totalAmmunition.text = "";
        _itemManager.UIReference.FPItemInfoUI.itemName.text = _weaponItem.itemName.ToString();
    }
}
