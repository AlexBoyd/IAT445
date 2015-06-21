using UnityEngine;
using System.Collections;


public class Button : Interactable
{

	Animator _animator;
	public AudioSource Audio;

	void Awake ()
	{
		base.Awake ();
		_animator = GetComponent<Animator> ();
	}

	public  void pressed ()
	{
		_animator.SetBool ("PressButton", true);
		Audio.Play ();

	}

	public  void released ()
	{
		_animator.SetBool ("PressButton", false);
		Audio.Play ();

	}

	public override void triggerPressedEvent ()
	{
		pressed ();

		base.triggerPressedEvent ();
	}

	public override void triggerReleasedEvent ()
	{
		released ();

		base.triggerReleasedEvent ();
	}

}
