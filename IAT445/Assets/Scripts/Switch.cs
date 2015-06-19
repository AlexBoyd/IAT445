using UnityEngine;
using System.Collections;

public class Switch : Interactable {

	public bool _isOn;

	Animator _animator;
//	MeshRenderer _meshRenderer;

	void Awake()
	{
		_meshRenderer = GetComponentInChildren<MeshRenderer> ();

		base.Awake ();
		_animator = GetComponent<Animator> ();

//		updateColor();
	}


	public void updateColor()
	{
		Color c = _meshRenderer.material.color;

		c = _isOn ? Color.green : Color.red;

		_meshRenderer.material.color = c;
	}

	public  void pressed() 
	{
//		_animator.SetBool ("toggle", !_isOn);
		_isOn = !_isOn;

//		updateColor ();
	}

	public override void triggerPressedEvent()
	{
		pressed ();

		base.triggerPressedEvent ();
	}




}
