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

	public float _crosshairZOffset;

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
		
			if (hit.transform.gameObject != _currentFocus && hit.transform.gameObject.GetComponent<Interactable> () != null) {
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
	public void removeCurrentFocus()
	{
		if (_currentFocus != null) {
			_currentFocus.GetComponent<Interactable> ().unlit ();
			_currentFocus = null;
		}

	}
	

	void check3DCrosshair()
	{
		RaycastHit hit;
		Ray r = new Ray (transform.position, (_centerReference.position - transform.position).normalized);

		if (Physics.Raycast (r, out hit, Mathf.Infinity,_3dLayerMask)) 
		{
			_3dCrosshair.SetActive (true);

			Vector3 newHitPoint = hit.point - r.direction * _crosshairZOffset;

			_3dCrosshair.transform.position = Vector3.Distance(_3dCrosshair.transform.position,newHitPoint) > 1 ?  newHitPoint :  Vector3.Lerp(_3dCrosshair.transform.position,newHitPoint,Time.deltaTime * 15);

		}
		else
			_3dCrosshair.SetActive (false);
	}

	void checkButtonPress()
	{
		if (player.GetButtonDown ("Use")) 
		{
			if (_currentFocus != null) {
				_currentFocus.GetComponent<Interactable> ().triggerPressedEvent ();

				StartCoroutine (buttonPressed (_currentFocus.GetComponent<Interactable>()));

			}
		}

	}
	
	void Update()
	{

		updateFacingObject ();
		checkButtonPress ();

        if (player.GetButton("Reset1") && player.GetButton("Reset2") && player.GetButton("Reset3"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}

	IEnumerator buttonPressed (Interactable interactable)
	{
		while (true) 
		{
			if (player.GetButton ("Use"))
			{
				interactable.triggerHoldEvent ();
			}
			if (player.GetButtonUp ("Use")) 
			{
				interactable.triggerReleasedEvent ();
				yield break;
			}

			yield return null;
		}

	}

}
