using UnityEngine;
using System.Collections;
using Rewired;

public class SkyboxRotate : MonoBehaviour {


	public int playerId;
	private Player player;

	public float rotationSpeed;

	public float currentHorizontal;
	public float currentVertical;

	void Awake() 
	{
		player = ReInput.players.GetPlayer (playerId);
	}

	void Update()
	{
		float h = player.GetAxis ("Horizontal");
		float v = player.GetAxis ("Vertical");


		currentHorizontal = Mathf.Lerp (currentHorizontal, h, Time.deltaTime);
		currentVertical = Mathf.Lerp (currentVertical, v, Time.deltaTime);

		Quaternion horizontalRotation = Quaternion.AngleAxis( currentVertical,transform.right);
		Quaternion verticalRotation = Quaternion.AngleAxis (currentHorizontal, -transform.up);

		transform.rotation = Quaternion.Slerp (transform.rotation, transform.rotation* horizontalRotation * verticalRotation, rotationSpeed * Time.deltaTime);

	}

}
