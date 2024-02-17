using UnityEngine;
[CreateAssetMenu(fileName = "New Ammunition Object", menuName = "Inventory System/Items/Ammunition")]
public class AmmunitionObject : ItemObject
{
    [Header("Ammunition Settings")]
    public AmmunitionType ammunitionType;
    private void Awake()
    {
        type = ItemType.ammunition;
    }
}
public enum AmmunitionType
{
    sniperAmmunition,
    lightAmmunition,
    heavyAmmunition,
    shotgunAmmunition
}
