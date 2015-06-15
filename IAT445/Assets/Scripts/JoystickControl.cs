using UnityEngine;
using System.Collections;
using Rewired;


public class JoystickControl : MonoBehaviour {

	public float _degree;
	Quaternion _maxLeft;
	Quaternion _maxRight;
	Quaternion _maxUp;
	Quaternion _maxDown;

	public int playerId;
	private Player player;
	
	void Awake() {
		player = ReInput.players.GetPlayer(playerId);


		Vector3 eulerAngles = transform.rotation.eulerAngles;
		eulerAngles.z = _degree;
		_maxLeft = Quaternion.Euler (eulerAngles);

		eulerAngles.z = -_degree;
		_maxRight = Quaternion.Euler (eulerAngles);


		eulerAngles = transform.rotation.eulerAngles;
		eulerAngles.x = _degree;
		_maxUp = Quaternion.Euler (eulerAngles);
		
		eulerAngles.x = -_degree;
		_maxDown = Quaternion.Euler (eulerAngles);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(player.GetAxis("Horizontal"));
//		Debug.Log(player.GetAxis("Vertical"));

		Quaternion newRot = Quaternion.Slerp (_maxLeft, _maxRight, (player.GetAxis ("Horizontal") + 1) / 2f) * Quaternion.Slerp (_maxDown, _maxUp, (player.GetAxis ("Vertical") + 1) / 2f);
		transform.rotation = newRot;



	}
}
