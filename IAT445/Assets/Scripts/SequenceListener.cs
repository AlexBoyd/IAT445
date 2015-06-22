using UnityEngine;
using UnityEngine.UI;
using System.Collections;




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
			if (_emergencyPowerOn && !_powerBypass) {
				_windShieldPrompt.showPowerError ();
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
				_hyperDrive1Primed = false;
			} else if (_hyperDrive2Primed) {
				_EffectsAnimations.Play ("HyperDriveFailure");
				_powerOutage = true;
				_hyperDrive2Primed = false;

			}
		} 
	}


	// Use this for initialization
	void Start ()
	{
	
//		promptRandomInteractable ();
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
