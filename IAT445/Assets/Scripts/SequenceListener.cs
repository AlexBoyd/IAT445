using UnityEngine;
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

	string _requiredEvent;

	public ARWindShield _windShieldPrompt;

	public Animation _EffectsAnimations;

	public bool _cockpitActivated = false;
	public bool _hyperDrive1Primed;
	public bool _hyperDrive2Primed;
	public bool _emergencyPowerOn;
	public bool _powerOutage;
	public bool _powerBypass;

	public Interactable _removablePanel;
	public CharView _charview;
	public AudioSource _staticAudio;


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
		//_switchPanel = GameObject.FindObjectOfType<SwitchPanel> () as SwitchPanel;


	}

	void OnEnable ()
	{
		foreach (Interactable i in _interectables) {
			i.PressedEvent += pressedEvent;
			i.ReleasedEvent += releasedEvent;
		}


		_switchPanel.TriggerEvent += eventTriggered;
		_emergencyPowerSwitch.TriggerEvent += eventTriggered;
	}

	void OnDisable ()
	{
		foreach (Interactable i in _interectables) {
			i.PressedEvent -= pressedEvent;
			i.ReleasedEvent -= releasedEvent;
		}

		_switchPanel.TriggerEvent -= eventTriggered;
		_emergencyPowerSwitch.TriggerEvent -= eventTriggered;
	}


	void eventTriggered (string eventName)
	{
		if (_cockpitActivated) {
			if (eventName.Equals ("switchPanel")) {
				Debug.Log ("SwitchPanel");
				//_windShieldPrompt.showLifeOK ();
			}
		}

//		if (eventName == "gravity_button") 
//		{
//			_windShieldPrompt.showGravity ();
//		}
//		if (eventName == "engines_button") 
//		{
//			_windShieldPrompt.showEngines ();
//		}
//		if (eventName == "powers_button") 
//		{
//			_windShieldPrompt.showPowers();
//		}
//		if (eventName == "life_button") 
//		{
//			_windShieldPrompt.showLife ();
//		}
//
//
		if (eventName == "emergencyPower" && !_emergencyPowerOn && _powerOutage) {
			_EffectsAnimations.Play ("EmergencyPower");
			_emergencyPowerOn = true;

			enableAllInteractables (true);
			disableStaticAudio ();
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

	void releasedEvent (Interactable interactable)
	{
		if (interactable._eventName.Equals ("initializeDrill") && interactable._pressDuration > 4) {
			_cockpitActivated = !_cockpitActivated;
			if (_cockpitActivated) {
				_windShieldPrompt.ARText.text = "Diagnostic Mode Activated";
				Debug.Log ("Pressed for 4 seconds the initialize drill!");
			} else {
				_windShieldPrompt.ARText.text = "Diagnostic Mode Disabled";
			}
		} else if (interactable._eventName.Equals ("initializeDrill") && interactable._pressDuration <= 4) {
			if (_hyperDrive1Primed) {
				_EffectsAnimations.Play ("HyperDriveSuccess");
				_windShieldPrompt.ARText.text = string.Empty;

				_hyperDrive1Primed = false;
//				enableStaticAudio ();
			} else if (_hyperDrive2Primed) {
				_EffectsAnimations.Play ("HyperDriveFailure");
				_powerOutage = true;
				_hyperDrive2Primed = false;
				_emergencyPowerOn = false;
				_windShieldPrompt.ARText.text = string.Empty;

				Debug.LogWarning ("disableAllInteractables");

				// Disable all
				disableAllInteractables ();
				// Enable the emergency switch
				_emergencyPowerSwitch.GetComponentInChildren<Interactable> ().enableInteractable (true);
//
//				enableStaticAudio ();

			}
		} 
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
	
//		promptRandomInteractable ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1))
			enableAllInteractables ();
		if (Input.GetKeyDown (KeyCode.Alpha2))
			disableAllInteractables ();
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
