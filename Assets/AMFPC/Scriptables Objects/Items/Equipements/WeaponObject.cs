using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Equipement/Weapon")]

public class WeaponObject : EquipementObject
{
    [Header("Weapon Settings")]
    public WeaponType weaponType;
    public bool holdAds;
    public bool sniper;
    public bool automaticFire;
    [Range(0, 50)] public float aimAssistStrength;
    public int weaponDamage;
    public int maxMagazineSize;
    [Range(0, 3)] public float timeBetweenShots;
    [Range(0, 3)] public float reloadDuration;
    public AmmunitionObject ammunitionType;
    public ParticleSystem hitImpact;
    public LineRenderer trail;
    [Header("Sound Effects")]
    public AudioClip fireSFX;
    public AudioClip reloadSFX;
    public AudioClip aimSFX;
    public AudioClip cancelAimSFX;
    private void Awake()
    {
        type = ItemType.equipement;
        equipementType = EquipementType.weapon;
    }
}
public enum WeaponType
{ 
    fireArm,
    melee,
    throwable,
}