﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;



public class SequenceListener : MonoBehaviour
{

	Interactable[] _interectables;
	Switch[] _switches;
	public SwitchPanel _switchPanel;
	public SwitchPanel _emergencyPowerSwitch;
	public Text _eventPrompt;
	public Text _minieventPrompt;

	string _requiredEvent;

	public ARWindShield _windShieldPrompt;
	public Transform _consoleParent;

	public Animation _EffectsAnimations;

	public bool _cockpitActivated = false;
	public bool _hyperDrive1Primed;
	public bool _hyperDrive1Done;
	public bool _hyperDrive2Primed;
	public bool _hyperDrive2Done;
	public bool _hyperDrive3Primed;
	public bool _emergencyPowerOn;
	public bool _powerOutage;
	public bool _powerBypass;
	public bool _safetyOverride;


	public Interactable _removablePanel;
	public CharView _charview;
	public AudioSource _staticAudio;

	public GameObject[] _spaceScenes;
	public SkyboxRotate _strandedSpaceRotation;

	public GameObject _powerOutageMsg;

	public Animator _backPanelCoverAnim;

	public Material _skyboxMaterial;
	public AudioSource _steamAudio, _hyperdrivePrimedAudio, _hyperdriveErrorAudio;
	public float _diagnosticHoldTime = 2;

	Vector3 _openConsolePos;
	public Keypad _keypad;

	public Text	ConsoleTxt;

	void disableAllInteractables ()
	{
		_charview.removeCurrentFocus ();

		foreach (Interactable interactable in _interectables) {
			interactable.disableInteractable ();
		}
	}

	void enableAllInteractables (bool overridePrevious = false)
	{
		_charview.removeCurrentFocus ();

		foreach (Interactable interactable in _interectables) {
			interactable.enableInteractable (overridePrevious);
		}
	}



	void Awake ()
	{
		_interectables = GameObject.FindObjectsOfType<Interactable> () as Interactable[];
//		_keypad = GameObject.FindObjectOfType<Keypad> ();
		//_switchPanel = GameObject.FindObjectOfType<SwitchPanel> () as SwitchPanel;

		_openConsolePos = _consoleParent.position;
		Vector3 desiredStartingConsolePos = _openConsolePos;
		desiredStartingConsolePos.y -= 2.5f;
		_consoleParent.position = desiredStartingConsolePos;
	}

	void OnEnable ()
	{
		foreach (Interactable i in _interectables) {
			i.PressedEvent += pressedEvent;
			i.ReleasedEvent += releasedEvent;
			i.HoldEvent += holdEvent;
		}


		_switchPanel.TriggerEvent += eventTriggered;
		_emergencyPowerSwitch.TriggerEvent += eventTriggered;
		_keypad.TriggerEvent += eventTriggered;


	}

	void OnDisable ()
	{
		foreach (Interactable i in _interectables) {
			i.PressedEvent -= pressedEvent;
			i.ReleasedEvent -= releasedEvent;
			i.HoldEvent -= holdEvent;
		}

		_switchPanel.TriggerEvent -= eventTriggered;
		_emergencyPowerSwitch.TriggerEvent -= eventTriggered;
		_keypad.TriggerEvent -= eventTriggered;
	}


	void eventTriggered (string eventName)
	{
		if (_cockpitActivated) {
			if (eventName.Equals ("switchPanel")) {
				Debug.Log ("SwitchPanel");
				//_windShieldPrompt.showLifeOK ();
			}
		}

		if (eventName.StartsWith ("keypad")) {
			if (eventName.Contains ("8717") && !_hyperDrive1Done) {
				ConsoleTxt.text = "HyperDrive Primed";
				_hyperDrive1Primed = true;
				_hyperdrivePrimedAudio.Play ();

			} else if (eventName.Contains ("7178")) {
				ConsoleTxt.text = "HyperDrive Primed";
				if (_hyperDrive2Done)
					_hyperDrive3Primed = true;
				else
					_hyperDrive2Primed = true;
				_hyperdrivePrimedAudio.Play ();
			} else {
				ConsoleTxt.text = "Wrong Launch Code";
				_hyperdriveErrorAudio.Play ();
			}
		}

		if (eventName == "emergencyPower" && !_emergencyPowerOn && _powerOutage) {
			automaticPowerBack ();

		}
		if (eventName == "safetyOverride" && _emergencyPowerOn) {
			_safetyOverride = true;
		}

	}

	void pressedEvent (Interactable interactable)
	{
		if (!_cockpitActivated)
			return;
		
		if (interactable._eventName.Equals ("gravity_button")) {
			_windShieldPrompt.showGravity ();
		}
		if (interactable._eventName.Equals ("engines_button")) {
			_windShieldPrompt.showEngines ();
		}
		if (interactable._eventName.Equals ("powers_button")) {
			if (_emergencyPowerOn) {
				if (_powerBypass) {
					_windShieldPrompt.showPowerRepaired ();
				} else {
					_windShieldPrompt.showPowerError ();
				}
			} else {
				_windShieldPrompt.showPowers ();
			}
		}
		if (interactable._eventName.Equals ("life_button")) {
			_windShieldPrompt.showLife ();
		}
	}

	void automaticPowerBack ()
	{
		
		_EffectsAnimations.Play ("EmergencyPower");
		_emergencyPowerOn = true;
		_powerOutageMsg.SetActive (false);
		enableAllInteractables (true);

		disableStaticAudio ();

		_emergencyPowerSwitch.transform.GetComponent<Interactable> ().disableInteractable ();

		_strandedSpaceRotation.enabled = true;

		CancelInvoke ("automaticPowerBack");
	}

	void releasedEvent (Interactable interactable)
	{

		if (interactable._eventName.Equals ("initializeDrill")) {
			_enableModeChange = true;
		}


		if (interactable._eventName.Equals ("initializeDrill") && interactable._pressDuration <= _diagnosticHoldTime) {
			if (_hyperDrive1Primed) {// || (_powerBypass && _hyperDrive2Primed)) {

				hideConsole ();
				//Invoke ("bringConsoleBack", 13.5f);
				StartCoroutine (switchSpaceVisual ());

				_EffectsAnimations.Play ("HyperDriveSuccess");
				_windShieldPrompt.ARText.text = string.Empty;

				_hyperDrive1Primed = false;
				_hyperDrive1Done = true;
			} else if (_hyperDrive2Primed) {
				
				hideConsole ();

				_EffectsAnimations.Play ("HyperDriveFailure");
				_powerOutage = true;
				_hyperDrive2Primed = false;
				_emergencyPowerOn = false;
				_windShieldPrompt.ARText.text = string.Empty;


				StartCoroutine (switchSpaceVisual ());

				Debug.LogWarning ("disableAllInteractables");

				// Disable all
				disableAllInteractables ();
				// Enable the emergency switch
				_emergencyPowerSwitch.GetComponentInChildren<Interactable> ().enableInteractable (true);
//
//				enableStaticAudio ();
				_hyperDrive2Primed = false;
				_hyperDrive2Done = true;
				Invoke ("automaticPowerBack", 70);
				_strandedSpaceRotation.enabled = false;
			} else if (_hyperDrive3Primed && _powerBypass && _safetyOverride) {
				hideConsole ();
				//Invoke ("bringConsoleBack", 13.5f);
				StartCoroutine (switchSpaceVisual ());

				_EffectsAnimations.Play ("HyperDriveSuccess");
				_windShieldPrompt.ARText.text = string.Empty;

				_hyperDrive1Primed = false;	
				StartCoroutine (gameOver ());
			} else {
				if (_hyperDrive3Primed && _powerBypass && !_safetyOverride) {
					ConsoleTxt.text = "SAFETY LOCK ACTIVATED";
					_hyperdriveErrorAudio.Play ();
				} else if (_hyperDrive3Primed && !_powerBypass) {
					ConsoleTxt.text = "ERROR IN POWER SYSTEM";
					_hyperdriveErrorAudio.Play ();
				}
			
		
			}
		} 
	}

	IEnumerator gameOver ()
	{
		yield return new WaitForSeconds (12.5f);

		disableAllInteractables ();

		while (true) {
			ConsoleTxt.text = "Welcome Back";
			yield return new WaitForSeconds (1.25f);
			ConsoleTxt.text = "Thanks for playing";
			yield return new WaitForSeconds (1.25f);
		}
	}

	bool _enableModeChange = true;

	void holdEvent (Interactable interactable)
	{
		if (interactable._eventName.Equals ("initializeDrill") && interactable._pressDuration > _diagnosticHoldTime && _enableModeChange) {
			_cockpitActivated = !_cockpitActivated;
			_enableModeChange = false;
			if (_cockpitActivated) {
				//_windShieldPrompt.ARText.text = "Diagnostic Mode Activated";
				_minieventPrompt.text = "Diagnostic Mode Activated";
				bringConsoleBack ();
				Debug.Log ("Pressed for 4 seconds the initialize drill!");
			} else {
				//_windShieldPrompt.ARText.text = "Diagnostic Mode Disabled";
				hideConsole ();
				_minieventPrompt.text = "Diagnostic Mode Disabled";
			}
		} 
	}


	public IEnumerator switchSpaceVisual ()
	{
		yield return new WaitForSeconds (1.4f);
		_spaceScenes [0].SetActive (false);
		_spaceScenes [1].SetActive (false);
		_spaceScenes [2].SetActive (true);
		yield return new WaitForSeconds (9.5f);

		if (_emergencyPowerOn) {
			//I have already passed through the problems and fixed it, ready to get back home
			_spaceScenes [0].SetActive (true);
			_spaceScenes [2].SetActive (false);
		} else {
			if (_powerOutage) {
				//Things just went really bad D: so don't show anything, just stars all the way
				//Oh, show the power outage thingy on the miniconsole
				_spaceScenes [2].SetActive (true);
				_powerOutageMsg.SetActive (true);
				//And pop the back panel
				_backPanelCoverAnim.Play ("BackPanelCoverAnimation", 0);
			} else {
				//This is the start of my journey, take me to the nebulas!
				_spaceScenes [1].SetActive (true);
				_spaceScenes [2].SetActive (false);
			}
		}
	}



	public void hideConsole ()
	{
		_minieventPrompt.text = "";
		_consoleParent.DOKill ();
		Vector3 desiredPos = _openConsolePos;
		desiredPos.y -= 2;
		_consoleParent.DOMove (desiredPos, 1.4f);

		_cockpitActivated = false;
		_steamAudio.Play ();
	}

	public void bringConsoleBack ()
	{
		_consoleParent.DOKill ();
		_consoleParent.DOMove (_openConsolePos, 1.4f);
		_steamAudio.Play ();
	}

	public void disableStaticAudio ()
	{
		Debug.LogWarning ("Disable static!");


		StartCoroutine (setStaticVolume (0.0f, 0.25f));
	}

	public void enableStaticAudio ()
	{
		Debug.LogWarning ("Enable static!");

		StartCoroutine (setStaticVolume (0.02f, 0.25f));
	}

	IEnumerator setStaticVolume (float endValue, float duration)
	{
		float elapsed = 0.0f;

		float initialValue = _staticAudio.volume;

		while (elapsed < duration) {
			float t = elapsed / duration;

			_staticAudio.volume = Mathf.Lerp (initialValue, endValue, t);

			elapsed += Time.deltaTime;

			yield return null;
		}

		_staticAudio.volume = endValue;
	}




	// Use this for initialization
	void Start ()
	{
		ChangeSkyBoxRotation ();
		
//		_skyboxMaterial.DOFloat (360.0f,"_Rotation", 10f);

//		promptRandomInteractable ();
	}

//	float _currentSkyboxRotation, _desiredSkyboxRotation, _duration;

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.H)) {
			_EffectsAnimations.Play ("HyperDriveSuccess");

		}

		if (Input.GetKeyUp (KeyCode.J)) {
			_EffectsAnimations.Play ("HyperDriveFailure");
			
		}
//		_currentSkyboxRotation = Mathf.Lerp (_currentSkyboxRotation, _desiredSkyboxRotation, Time.deltaTime);
//		_skyboxMaterial.SetFloat ("_Rotation", _currentSkyboxRotation);

//		if (Input.GetKeyDown (KeyCode.Alpha1))
//			enableAllInteractables ();
//		if (Input.GetKeyDown (KeyCode.Alpha2))
//			disableAllInteractables ();
	}

	void ChangeSkyBoxRotation ()
	{
		float currentRotation = _skyboxMaterial.GetFloat ("_Rotation");
		float newRotation = Random.Range (0, 5);
		float time = Mathf.Abs (newRotation - currentRotation) * 10f;
		_skyboxMaterial.DOFloat (newRotation, "_Rotation", time);
		Invoke ("ChangeSkyBoxRotation", time + 0.1f);
	}

	void promptRandomInteractable ()
	{
		int randomIndex = Random.Range (0, _interectables.Length);

		_eventPrompt.text = "Press the " + _interectables [randomIndex]._eventNamePretty;
		_requiredEvent = _interectables [randomIndex]._eventName;


		setTextInvisible ();

	}
	void setTextInvisible ()
	{
		setTextAlpha (0);
		Invoke ("setTextVisible", 1);
	}

	void setTextVisible ()
	{
		setTextAlpha (1);
	}

	void setTextAlpha (float f)
	{
		Color c = _eventPrompt.color;
		c.a = f;
		_eventPrompt.color = c;
	}
}
