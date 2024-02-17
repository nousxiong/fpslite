using UnityEngine;

public class PlayerDeathUI : MonoBehaviour
{
    public UIButton respawnButton;
    private PlayerController _playerController;
    private void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        _playerController.healthManager.OnDeath.AddListener(() => EnablePlayerDeathUI(true));
        respawnButton.OnButtonDown.AddListener(_playerController.playerRespawn.RespawnPlayer);
        respawnButton.OnButtonDown.AddListener(() => EnablePlayerDeathUI(false));
    }

    public void EnablePlayerDeathUI(bool value)
    {
        _playerController.inputManager.ToggleInventoryUI();
        this.transform.GetChild(0).gameObject.SetActive(value);
    }
}
