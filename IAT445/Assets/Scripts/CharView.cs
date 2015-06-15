using UnityEngine;
using System.Collections;
using Rewired;

public class CharView : MonoBehaviour
{
		[Range(0.1f,10f)]
	public float _interactDistance;

	[Range(0.1f,10f)]
	public float _sphereRadius;

	public GameObject _currentFocus;

	public Transform _cameraTransformLeft,_cameraTransformRight;

	public Transform _centerReference;

	public int playerId;
	private Player player;
	
	void Awake() 
	{
		player = ReInput.players.GetPlayer (playerId);
	}
		void updateFacingObject ()
	{
		Vector3 p1 = transform.position;
	

		Debug.DrawRay(p1,(_cameraTransformLeft.forward + _cameraTransformRight.forward).normalized * 10f,Color.red);


		RaycastHit hit;
		if (Physics.SphereCast (new Ray (p1, (_centerReference.position - transform.position).normalized), _sphereRadius, out hit, _interactDistance)) {
			if (_currentFocus != null && _currentFocus != hit.transform.gameObject) {
				_currentFocus.GetComponent<Interactable> ().unlit ();
				_currentFocus = null;
			
			}
		
			if (hit.transform.gameObject.GetComponent<Interactable> () != null) {
				_currentFocus = hit.transform.gameObject;
				_currentFocus.GetComponent<Interactable> ().litUp ();
			}

		} else {
			if (_currentFocus != null)
				_currentFocus.GetComponent<Interactable> ().unlit ();
		}
	
	}

	void checkButtonPress()
	{
		if (player.GetButtonDown ("Use")) 
		{
			if(_currentFocus!=null)
				_currentFocus.GetComponent<Interactable>().triggerEvent();
		}
	}
	
	void Update()
	{

		updateFacingObject ();
		checkButtonPress ();

	

	}

}
