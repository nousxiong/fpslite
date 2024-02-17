using UnityEngine;

public enum ItemType
{
    equipement,
    consumable,
    ammunition,
    basic,
}

public abstract class ItemObject : ScriptableObject
{
    [Header("General Item Settings")]
    public string itemName;
    public Sprite itemIcon;
    public GameObject prefab;
    public ItemType type;
    public bool stackable;
    public bool isFirstPersonItem;
    public int FPItemIndex;
    [TextArea(5,10)] public string description;
}
