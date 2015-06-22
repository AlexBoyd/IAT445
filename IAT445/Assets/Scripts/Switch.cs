using UnityEngine;
using System.Collections;

public class Switch : Interactable {

	public bool _isOn;

	Animator _animator;
//	MeshRenderer _meshRenderer;

	void Awake()
	{
		base.Awake ();

		_animator = GetComponent<Animator> ();
	}


	public void updateColor()
	{
		Color c = _meshRenderer.material.color;

		c = _isOn ? Color.green : Color.red;

		_meshRenderer.material.color = c;
	}

	public  void pressed() 
	{
//		if(Audio)
//		{
//			if (_isOn) {
//				Audio.time = Audio.clip.length;
//				Audio.pitch = -1;
//			} else {
//				
//				Audio.time = 0;
//				Audio.pitch = 1;
//			}
//		}


		_animator.SetBool ("Up", !_isOn);
		_isOn = !_isOn;


	}


	public override void triggerPressedEvent()
	{
		pressed ();

		base.triggerPressedEvent ();
	}




}
