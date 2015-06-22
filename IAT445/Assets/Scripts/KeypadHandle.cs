using UnityEngine;
using System.Collections;

public class KeypadHandle : Interactable {

	public bool _isUp;

	Animator _animator;
//	MeshRenderer _meshRenderer;

	void Awake()
	{
		base.Awake ();

		_animator = GetComponentInParent<Animator> ();
	}

	public  void pressed() 
	{


		_animator.SetBool ("Up", !_isUp);
		_isUp = !_isUp;


	}


	public override void triggerPressedEvent()
	{
		if (!_interactable)
			return;

		pressed ();

		base.triggerPressedEvent ();
	}




}
