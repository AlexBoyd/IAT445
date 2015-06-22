using UnityEngine;
using System.Collections;


public class RemovablePanel : Interactable
{

	Animator _animator;


	public Vector3 _force;

	void Awake ()
	{
		base.Awake ();
		_animator = GetComponent<Animator> ();
	}

	public  void pressed ()
	{
//		_animator.SetBool ("PressButton", true);
//		Audio.Play ();

	}

	public override void triggerPressedEvent ()
	{
		if (!_interactable)
			return;

		pressed ();

		base.triggerPressedEvent ();

		// Just for now
		gameObject.SetActive (false);
	}

}
