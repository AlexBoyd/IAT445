using UnityEngine;
using System.Collections;

[RequireComponent(typeof( VRInteractable ))]
public class Switch : Interactable {

	public bool _isOn;

	Animator _animator;
//	MeshRenderer _meshRenderer;

	void Awake()
	{
		base.Awake ();

		_animator = GetComponent<Animator> ();
	}

	public  void pressed() 
	{


		_animator.SetBool ("Up", !_isOn);
		_isOn = !_isOn;


	}


	public override void triggerPressedEvent()
	{
		if (!_interactable)
			return;

		pressed ();

		base.triggerPressedEvent ();
	}
	
}
