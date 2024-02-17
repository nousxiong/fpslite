using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private CharacterController _characterController;
    private HealthManager healthManager;
    public Transform respawnPosition;
    private PlayerController _playerController;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _characterController = _playerController.characterController;
        healthManager = GetComponent<HealthManager>();
    }
    public void RespawnPlayer()
    {
        _characterController.enabled = false;
        this.transform.position = respawnPosition.position;
        _characterController.enabled = true;
        healthManager.RestoreHealth();
        _playerController.defaultMovement.mechanicEnabled = true;
        _playerController.jumping.mechanicEnabled = true;
        _playerController.crouching.mechanicEnabled = true;
    }
}
