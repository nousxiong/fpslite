using System.Collections;
using UnityEngine;

public class CameraHeadbob : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerState _playerState;
    private Animator _headbobAnimator;
    private bool _playJumpHeadbob, _playLandingHeadbob;
    private float _jumpAnimationLength, _landAnimationLength;
    private void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        _headbobAnimator = GetComponent<Animator>();
        _playerState = _playerController.playerState;
        GetAnimationClipsLength();
        _playerController.jumping.OnJump += PlayJumpHeadbob;
        _playerController.collisions.OnLand += PlayLandingHeadbob;
    }
    private void Update()
    {
        ManageHeadbobAnimations();
    }
    private void ManageHeadbobAnimations()
    {
        if (!_playJumpHeadbob && !_playLandingHeadbob)
        {
            if (_playerState.idle || _playerState.inAir || _playerState.climbing || _playerState.sliding)
            {
                _headbobAnimator.SetInteger("State", 0);
            }
            if (_playerState.walking)
            {
                _headbobAnimator.SetInteger("State", 1);
            }
            if (_playerState.running)
            {
                _headbobAnimator.SetInteger("State", 2);
            }
        }
        else if (!_playLandingHeadbob)
        {
            _headbobAnimator.SetInteger("State", 3);
        }
        else
        {
            _headbobAnimator.SetInteger("State", 4);
            _playJumpHeadbob = false;
        }
    }
    public void PlayJumpHeadbob()
    {
        StopCoroutine(Jump());
        StartCoroutine(Jump());
        _playJumpHeadbob = true;
    }
    public void PlayLandingHeadbob()
    {
        StopCoroutine(Land());
        StartCoroutine(Land());
        _playLandingHeadbob = true;
    }
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(_jumpAnimationLength);
        _playJumpHeadbob = false;
    }
    IEnumerator Land()
    {
        yield return new WaitForSeconds(_landAnimationLength);
        _playLandingHeadbob = false;
    }
    public void GetAnimationClipsLength()
    {
        AnimationClip[] clips = _headbobAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "JumpHeadbob":
                    _jumpAnimationLength = clip.length;
                    break;
                case "LandHeadbob":
                    _landAnimationLength = clip.length;
                    break;
            }
        }
    }
}
