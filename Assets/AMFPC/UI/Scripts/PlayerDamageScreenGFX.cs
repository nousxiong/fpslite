using UnityEngine;

public class PlayerDamageScreenGFX : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>().OnDamage.AddListener(PlayScreenDamageAnimation);
    }
    private void PlayScreenDamageAnimation()
    {
        _animator.Play("DamageAnimation");
    }
}
