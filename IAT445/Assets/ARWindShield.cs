﻿using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ARWindShield : MonoBehaviour
{

	public Text ARText;

	[TextArea(3, 10)]
	public string
		ArtificialGravityOK;
	[TextArea(3, 10)]
	public string
		EnginesOK;
	[TextArea(3, 10)]
	public string
		PowerSystemsOK;
	[TextArea(3, 10)]
	public string
		LifeSupportOK;
	[TextArea(3, 10)]
	public string
		PowerError;
	[TextArea(3, 10)]
	public string
		PowerRepaired;

	public void Update ()
	{
//		if (Input.GetKeyDown (KeyCode.Alpha1)) {
//			ARText.text = ArtificialGravityOK;
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha2)) {
//			ARText.text = EnginesOK;
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha3)) {
//			ARText.text = PowerSystemsOK;
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha4)) {
//			ARText.text = LifeSupportOK;
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha5)) {
//			ARText.text = LifeSupportError;
//		}
	}


	public void showGravity ()
	{
		ARText.color = Color.green;
		ARText.text = ArtificialGravityOK;
	}

	public void showEngines ()
	{
		ARText.color = Color.green;
		ARText.text = EnginesOK;
	}
	public void showPowers ()
	{
		ARText.color = Color.green;
		ARText.text = PowerSystemsOK;
	}
	public void showLife ()
	{
		ARText.color = Color.green;
		ARText.text = LifeSupportOK;
	}
	public void showPowerError ()
	{
		ARText.color = new Color (255f, 100f, 0);
		ARText.text = PowerError;
	}
	public void showPowerRepaired ()
	{
		ARText.color = new Color (100f, 100f, 0);
		ARText.text = PowerRepaired;
	}


}
