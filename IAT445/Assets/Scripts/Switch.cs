using UnityEngine;
using System.Collections;

public class Switch : Interactable {

	public bool _isOn;

	Animator _animator;

	void Awake()
	{
		base.Awake ();
		_animator = GetComponent<Animator> ();
	}


//	public override void pressed()
//	{
//		Debug.LogWarning ("pressed switch");
//	}

	public  void pressed() 
	{
//		_animator.SetBool ("toggle", !_isOn);
		_isOn = !_isOn;

	}

	public override void triggerPressedEvent()
	{
		pressed ();

		base.triggerPressedEvent ();
	}




}
