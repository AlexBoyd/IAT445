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

		_meshRenderer.material.SetColor ("_EmissionColor", Color.blue);
//		_meshRenderer.material.color = Color.blue;
	}

	public override void triggerPressedEvent ()
	{
		pressed ();

		base.triggerPressedEvent ();
	}

}
