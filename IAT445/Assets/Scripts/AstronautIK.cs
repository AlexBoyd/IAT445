using UnityEngine;
using System.Collections;

public class AstronautIK : MonoBehaviour {
	
	
	public Transform _joystickGrip;
	public Transform _headRotation;
	public Transform _vrRoot;
	Animator _animator;

	public Transform _spineTransform;

	Vector3 _initialSpinePos, _initialVRPos;

	public float _xOffset, _zOffset;

	void Awake()
	{
		_animator = GetComponent<Animator> ();
		_initialSpinePos = _spineTransform.transform.position;
		_initialVRPos = _headRotation.transform.position;
	}
	
	//a callback for calculating IK
	void OnAnimatorIK()
	{
		if(_animator) {
			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
			_animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
			_animator.SetIKPosition(AvatarIKGoal.RightHand,_joystickGrip.position);
			_animator.SetIKRotation(AvatarIKGoal.RightHand,_joystickGrip.rotation);

			_animator.SetLookAtWeight(1);  
			_animator.SetLookAtPosition(_headRotation.position + _headRotation.transform.forward * 10);


		}        
		
		
		
	}   


	void Update()
	{

		_animator.SetFloat ("X", _xOffset);
		_animator.SetFloat ("Z", _zOffset);

	}
}
