using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();
    public void AddItem(ItemObject _item,int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < container.Count; i++)
        {
            if(container[i].item == _item)
            {
                hasItem = true;
                if (container[i].item.stackable)
                {
                    container[i].AddAmount(_amount);
                }
                else
                {
                    break;
                }
                break;
            }
        }
        if(!hasItem)
        {
            container.Add(new InventorySlot(_item, _amount));
        }
    }
}
[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item,int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;    
    }
}

