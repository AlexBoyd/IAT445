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

	void OnHandHoverBegin()
	{
		this.litUp ();
	}
	
	void OnHandHoverEnd()
	{
		this.unlit ();
	}
	
	void HandHoverUpdate ( VRHand hand )
	{
		if(hand.GetStandardInteractionButtonDown() || 
		   (hand.controller != null && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger )))
		{
			triggerPressedEvent();
		}
		
		if(hand.GetStandardInteractionButtonUp() || 
		   (hand.controller != null && hand.controller.GetPressUp( Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger )))
		{
			triggerReleasedEvent();
		}
	}




}
