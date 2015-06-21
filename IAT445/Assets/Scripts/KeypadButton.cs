using UnityEngine;
using System.Collections;


public class KeypadButton : Interactable
{

//	Animator _animator;
//	public AudioSource Audio;

	public string _value;

	public event StringDelegate ButtonPressed;

	void OnButtonPressed()
	{
		if (ButtonPressed!=null) 
		{
			ButtonPressed (_value);
		}

	}

	void Awake ()
	{
		base.Awake ();
//		_animator = GetComponent<Animator> ();
	}

	public  void pressed ()
	{
//		_animator.SetBool ("PressButton", true);
//		Audio.Play ();
		OnButtonPressed ();

	}

	public  void released ()
	{
//		_animator.SetBool ("PressButton", false);
//		Audio.Play ();

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
