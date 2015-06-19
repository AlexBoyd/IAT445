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

	public LayerMask _interectableLayerMask;

	public GameObject _3dCrosshair;
	public LayerMask _3dLayerMask;

	void Awake() 
	{
		player = ReInput.players.GetPlayer (playerId);
	}
		void updateFacingObject ()
	{
		Vector3 p1 = transform.position;
	

		Debug.DrawRay(p1,(_cameraTransformLeft.forward + _cameraTransformRight.forward).normalized * 10f,Color.red);


		RaycastHit hit;
		if (Physics.SphereCast (new Ray (transform.position, (_centerReference.position - transform.position).normalized), _sphereRadius, out hit, _interactDistance,_interectableLayerMask)) {
			if (_currentFocus != null && _currentFocus != hit.transform.gameObject) {
				removeCurrentFocus();
			
			}
		
			if (hit.transform.gameObject.GetComponent<Interactable> () != null) {
				setCurrentFocus(hit.transform.gameObject);
			}

		} else {
			if (_currentFocus != null)
			{
				removeCurrentFocus();
			}
		}

//		_3dCrosshair.SetActive (_currentFocus == null);

//		if (_currentFocus == null) 
//		{
			check3DCrosshair();
//		}
	
	}

	void setCurrentFocus(GameObject go)
	{
		_currentFocus = go;
		_currentFocus.GetComponent<Interactable> ().litUp ();
	}
	void removeCurrentFocus()
	{

			_currentFocus.GetComponent<Interactable> ().unlit ();
			_currentFocus = null;

	}
	

	void check3DCrosshair()
	{
		RaycastHit hit;
		if (Physics.Raycast (new Ray (transform.position, (_centerReference.position - transform.position).normalized), out hit, Mathf.Infinity,_3dLayerMask)) 
		{
			_3dCrosshair.SetActive (true);
			_3dCrosshair.transform.position = Vector3.Distance(_3dCrosshair.transform.position,hit.point) > 1 ?  hit.point :  Vector3.Lerp(_3dCrosshair.transform.position,hit.point,Time.deltaTime * 15);

		}
		else
			_3dCrosshair.SetActive (false);
	}

	void checkButtonPress()
	{
		if (player.GetButtonDown ("Use")) 
		{
			if(_currentFocus!=null)
				_currentFocus.GetComponent<Interactable>().triggerPressedEvent();
		}
		if (player.GetButtonUp ("Use")) 
		{
			if(_currentFocus!=null)
				_currentFocus.GetComponent<Interactable>().triggerReleasedEvent();
		}
	}
	
	void Update()
	{

		updateFacingObject ();
		checkButtonPress ();

	}

}
