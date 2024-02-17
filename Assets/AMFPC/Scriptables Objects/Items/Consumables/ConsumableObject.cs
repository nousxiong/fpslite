using UnityEngine;
[CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    [Header("Consumable Item Settings")]
    public int restoreHealthValue;
    private void Awake()
    {
        type = ItemType.consumable;
    }
}
