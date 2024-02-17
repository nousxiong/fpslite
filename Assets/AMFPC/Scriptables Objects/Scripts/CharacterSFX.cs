using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class CharacterSFX : ScriptableObject
{
    public AudioClip[] walkSFX;
	public AudioClip[] runSFX;
	public AudioClip[] jumpSFX;
	public AudioClip[] landSFX;
	public AudioClip[] slideSFX;
	public AudioClip[] deathSFX;
	public AudioClip[] damagedSFX;
}
