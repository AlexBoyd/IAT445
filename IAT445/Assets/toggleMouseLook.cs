using UnityEngine;
using System.Collections;

public class toggleMouseLook : MonoBehaviour
{


	public MouseLook mouseLook;
	// Use this for initialization
	void Start ()
	{
		mouseLook = GetComponent<MouseLook> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.M)) {
			mouseLook.enabled = !mouseLook.enabled;
		}
	}
}
