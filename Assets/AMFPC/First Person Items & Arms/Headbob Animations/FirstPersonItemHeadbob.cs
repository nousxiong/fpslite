using System.Collections;
using UnityEngine;

public class FirstPersonItemHeadbob : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerState _playerState;

    [HideInInspector] public bool aiming, playMovementAnimations;

    private Animator _anim;
    private float _jumpClipLength, _landClipLength;
    private bool _playJumpHeadbob, _playLandHeadbob;
    private void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        _playerState = _playerController.playerState;
        playMovementAnimations = true;
        _anim = GetComponent<Animator>();
        GetAnimationClipsLenght(_anim);
        _playerController.jumping.OnJump += PlayJumpHeadbob;
        _playerController.collisions.OnLand += PlayLandingHeadbob;
    }
    void Update()
    {
        ManageAnimations();
    }
    private void ManageAnimations()
    {
        if (aiming)
        {
            SetAnimation(0);
            return;
        }
        if (_playLandHeadbob)
        {
            SetAnimation(5); return;
        }
        if (_playJumpHeadbob)
        {
            SetAnimation(4); return;
        }
        if (!playMovementAnimations)
        {
            SetAnimation(0);
            return;
        }
        if (_playerState.idle || _playerState.inAir || _playerState.climbing || _playerState.sliding )
        {
            SetAnimation(0);
        }
    }
    public void PlayJumpHeadbob()
    {
        if(!aiming)
        {
            StopCoroutine(Jump());
            StartCoroutine(Jump());
            _playJumpHeadbob = true;
        }
    }
    public void PlayLandingHeadbob()
    {
        if(!aiming)
        {
            StopCoroutine(Land());
            StartCoroutine(Land());
            _playLandHeadbob = true;
        }
    }
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(_jumpClipLength);
        _playJumpHeadbob = false;
    }
    IEnumerator Land()
    {
        yield return new WaitForSeconds(_landClipLength);
        _playLandHeadbob = false;
    }
    public void GetAnimationClipsLenght(Animator animator)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Jump":
                    _jumpClipLength = clip.length;
                    break;
                case "Land":
                    _landClipLength = clip.length;
                    break;
            }
        }
    }
    private void SetAnimation(int TransitionInt)
    {
        _anim.SetInteger("State", TransitionInt);
    }
}
