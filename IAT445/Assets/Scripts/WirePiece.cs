using UnityEngine;
using System.Collections;
using DG.Tweening;


public class WirePiece : Interactable
{

	public int _rightOrientation;
	public int _orientation;
	bool _locked;
	public bool _isStraight;
//	Animator _animator;
//	public AudioSource Audio;

	public bool Locked { get { return _locked; } }
	public bool OrientedCorrectly { get { return _rightOrientation == _orientation; } }

	void Awake ()
	{
		base.Awake ();
		transform.localEulerAngles = new Vector3 (0, 0, _orientation * 90);
//		_animator = GetComponent<Animator> ();
	}

	public  void pressed ()
	{
		if (!_locked) {
			_orientation++;
			_orientation %= _isStraight? 2 : 4;

			transform.DOLocalRotate	 (new Vector3 (0, 0, _orientation * 90), 0.5f, RotateMode.Fast);
		}

//		_animator.SetBool ("PressButton", true);
//		Audio.Play ();

	}

	public void lockPiece()
	{
		_locked = true;
		_originalMaterial.color = Color.green;
	}

	public  void released ()
	{
//		_animator.SetBool ("PressButton", false);
//		Audio.Play ();

	}

	public override void triggerPressedEvent ()
	{
		pressed ();

		base.triggerPressedEvent ();
	}

	public override void triggerReleasedEvent ()
	{
		released ();

		base.triggerReleasedEvent ();
	}

}
