using UnityEngine;
using System.Collections;

public class KeypadHandle : Interactable {

//	public bool _isUp;

//	Animator _animator;
//	MeshRenderer _meshRenderer;

	Keypad _keypad;
	void Awake()
	{
		base.Awake ();
		_keypad = GetComponentInParent<Keypad> ();
	}
//
	public  void pressed() 
	{
		_keypad.toggleUpDown ();
//		_animator.SetBool ("Up", !_isUp);
//		_isUp = !_isUp;

	}
//
//	public void bringUp()
//	{
//		_isUp = true;
//		_animator.SetBool ("Up", _isUp);
//	}
//	public void bringDown()
//	{
//		_isUp = false;
//		_animator.SetBool ("Up", _isUp);
//	}
//
//
	public override void triggerPressedEvent()
	{
		if (!_interactable)
			return;

		pressed ();

		base.triggerPressedEvent ();
	}




}
