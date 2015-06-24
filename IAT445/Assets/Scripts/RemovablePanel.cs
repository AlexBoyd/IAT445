using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RemovablePanel : Interactable
{

	Animator _animator;

    Transform _rotateCoverPivot;
    Vector3 _originalRotation;
    Vector3 _openRotation;
	public Vector3 _force;
    bool closed = true;

	void Awake ()
	{
		base.Awake ();
        _rotateCoverPivot = transform.parent;
		_animator = GetComponent<Animator> ();
        _originalRotation = _rotateCoverPivot.localRotation.eulerAngles;
        _openRotation = _originalRotation;
        _openRotation.x = 92.1f;
	}

	public  void pressed ()
	{
//		_animator.SetBool ("PressButton", true);
//		Audio.Play ();
        if (closed)
        {
            closed = false;
            _rotateCoverPivot.DOKill();
            _rotateCoverPivot.DOLocalRotate(_openRotation,1);
        }
        else
        {
            closed = true;
            _rotateCoverPivot.DOKill();
            _rotateCoverPivot.DOLocalRotate(_originalRotation,1);
        }
	}

	public override void triggerPressedEvent ()
	{
		if (!_interactable)
			return;

		pressed ();

		base.triggerPressedEvent ();


		// Just for now
		//gameObject.SetActive (false);
	}

}
