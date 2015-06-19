using UnityEngine;
using System.Collections;


public class Button : Interactable {

	Animator _animator;

	void Awake()
	{
		base.Awake ();
		_animator = GetComponent<Animator> ();
	}

	public  void pressed() 
	{
		_animator.SetBool ("Press", true);
	}

	public override void triggerPressedEvent()
	{
		pressed ();

		base.triggerPressedEvent ();
	}

}
