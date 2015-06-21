using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour
{

	public GameObject _HyperDriveAudio;
	public GameObject _HyperDriveAnimation;
	public GameObject _AlarmAudio;
	public GameObject _AlarmAnimation;
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
			_AlarmAudio.SetActive (!_AlarmAudio.activeInHierarchy);
			_AlarmAnimation.SetActive (!_AlarmAnimation.activeInHierarchy);
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			_HyperDriveAudio.SetActive (!_HyperDriveAudio.activeInHierarchy);
			//_AlarmAnimation.SetActive (!_AlarmAnimation.activeInHierarchy);
		}
	}
}
