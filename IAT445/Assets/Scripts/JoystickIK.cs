using UnityEngine;
using System.Collections;

public class JoystickIK : MonoBehaviour {
	
	
	public Transform _joystickGrip;
	public Transform _headRotation;
	Animator _animator;



	void Awake()
	{
		_animator = GetComponent<Animator> ();
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
			_animator.SetLookAtPosition(_headRotation.position + -_headRotation.transform.forward * 10);



		}        
		
		
		
	}    
}
