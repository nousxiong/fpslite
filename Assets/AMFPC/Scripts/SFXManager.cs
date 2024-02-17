using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private PlayerController _playerController;
    public AudioSource audioSource;
    public PlayerSFX playerSFX;
    private HealthManager _healthManager;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    void Start()
    {
        _playerController.jumping.OnJump += PlayJumpSFX; 
        GetComponent<Sliding>().OnSlide.AddListener(PlaySlideSFX);
        _playerController.collisions.onFall.AddListener(PlayLandSFX);
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDamage.AddListener(PlayDamagedSFX);
        _healthManager.OnDeath.AddListener(PlayDeathSFX);
    }
    public void PlayJumpSFX()
    {
        PlayRandom(playerSFX.jumpSFX);
    }
    public void PlayWalkSFX()
    {
        PlayRandom(playerSFX.walkSFX);
    }
    public void PlayRunSFX()
    {
        PlayRandom(playerSFX.runSFX);
    }
    private void PlaySlideSFX()
    {
        PlayRandom(playerSFX.slideSFX);
    }
    private void PlayLandSFX(float _distance )
    {
        if(_distance > 1)
        PlayRandom(playerSFX.landSFX);
    }
    private void PlayDamagedSFX()
    {
        PlayRandom(playerSFX.damagedSFX);
    }
    private void PlayDeathSFX()
    {
        PlayRandom(playerSFX.deathSFX);
    }
    private void PlayRandom(AudioClip[] clips)
    {
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
