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
	}

	public  void pressed ()
	{

		OnButtonPressed ();

	}


	public override void triggerPressedEvent ()
	{
		if (!_interactable)
			return;

		pressed ();

		base.triggerPressedEvent ();
	}
}
