using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject slotPrefab;
    private ItemManager _itemManager;
    public GameObject displayContentHolder;
    public GameObject displayParent;
    public Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    private CameraLook _cameraLook;
    private InputManager _inputManager;
    private void Awake()
    {
        _itemManager = Camera.main.gameObject.GetComponentInChildren<ItemManager>();
        _inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }
    void Start()
    {
        CreateDisplay();
        _inputManager.toggleInventoryUI.AddListener(EnableDisplayUI);
        _cameraLook = Camera.main.gameObject.GetComponent<CameraLook>();
    }
    void Update()
    {
        UpdateDisplay();
    }
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.container.Count; i++)
        {
            GameObject obj = Instantiate(slotPrefab, displayContentHolder.transform);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = inventory.container[i].item.itemIcon;
            UIButton _button = obj.GetComponent<UIButton>();
            ItemObject _item = inventory.container[i].item;
            if (_item.isFirstPersonItem )
            {
                obj.transform.GetChild(1).GetComponentInChildren<Text>().text = "";
                int _switchIndex = _item.FPItemIndex;
                _button.OnButtonDown.AddListener(() => _itemManager.SwitchToItem(_switchIndex));
                _button.OnButtonDown.AddListener(_inputManager.ToggleInventoryUI);
            }
            else
            {
                obj.transform.GetChild(1).GetComponent<Text>().text = "x"+inventory.container[i].amount.ToString();
            }
            itemsDisplayed.Add(inventory.container[i], obj);
        }

    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.Count; i++)
        {
            if(itemsDisplayed.ContainsKey(inventory.container[i]))
            {
                
                if (inventory.container[i].item.type == ItemType.equipement)
                {
                    itemsDisplayed[inventory.container[i]].transform.GetChild(1).GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    itemsDisplayed[inventory.container[i]].transform.GetChild(1).GetComponentInChildren<Text>().text = "x" + inventory.container[i].amount.ToString();
                }
            }
            else
            {
                var obj = Instantiate(slotPrefab, displayContentHolder.transform);
                UIButton _button = obj.GetComponent<UIButton>();
                obj.transform.GetChild(0).GetComponent<Image>().sprite = inventory.container[i].item.itemIcon;
                ItemObject _item = inventory.container[i].item;
                if (inventory.container[i].item.type == ItemType.equipement)
                {
                    obj.transform.GetChild(1).GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    obj.transform.GetChild(1).GetComponentInChildren<Text>().text = "x" + inventory.container[i].amount.ToString();
                }
                if (_item.isFirstPersonItem)
                {
                    obj.transform.GetChild(1).GetComponentInChildren<Text>().text = "";
                    int _switchIndex = _item.FPItemIndex;
                    _button.OnButtonDown.AddListener(() => _itemManager.SwitchToItem(_switchIndex));
                    _button.OnButtonDown.AddListener(_inputManager.ToggleInventoryUI);
                }
                itemsDisplayed.Add(inventory.container[i], obj);
            }
        }
    }
    public void EnableDisplayUI()
    {
        displayParent.SetActive(!displayParent.activeSelf);
        _cameraLook.canRotateCamera = !_cameraLook.canRotateCamera;
    }
}
