using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;

[System.Serializable]
public class SequenceCues
{
	public string name;
	public List<GameObject> cuePrefabs;
}

public class SequenceListener : MonoBehaviour
{

	public enum SequenceTrigger {
		NONE, //NOOP
		DIAGNOSTIC_ON,// Diagnostic mode turned on
		DIAGNOSTIC_OFF, // Diagnostic mode turned off
		KEYPAD_UP, // Keypad brought up
		KEYPAD_DOWN, // Keypad brought down
		ENGINES_BUTTON, // Engines button clicked
		POWERS_BUTTON, // Powers button clicked
		LIFESUPPORT_BUTTON, // Life support clicked
		ARTIFICIALGRAVITY_BUTTON, // Artificial Gravity clicked
		PANEL_REMOVED, // Wires panel removed
		WIRES_BYPASSED, // Wires mini game solved
		SAFETY_BYPASSEED, // Safety switched bypassed
		HYPERSPACE_JUMP1_BEGIN, // Hyperspace jump 1 began
		HYPERSPACE_JUMP1_OVER,// Hyperspace jump 1 ended
		HYPERSPACE_JUMP2_BEGIN, // Hyperspace jump 2 began
		HYPERSPACE_JUMP2_OVER,// Hyperspace jump 2 ended
		HYPERSPACE_JUMP3_BEGIN, // Hyperspace jump 3 began
		HYPERSPACE_JUMP3_OVER,// Hyperspace jump 3 ended
		HYPERDRIVE_PRIMED,// Hyperspace was primed
		WRONG_LAUNCH_CODE, // Wrong launch code was typed in the keypad
		EMERGENCY_POWER_SWITCH // Emergency power switch used

	}

	Interactable[] _interectables;
	Switch[] _switches;
	public SwitchPanel _switchPanel;
	public SwitchPanel _emergencyPowerSwitch;
	public RemovablePanel _wiresPanel;
	public WireMiniGame _wireMiniGame;
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

	public bool _useNarration = false;

    public GameObject _hyperSuccessAudio, _hyperFailureAudio, _hyperSuccessAudio2;


	public Interactable _removablePanel;
	//public CharView _charview;
	public AudioSource _staticAudio;

	public GameObject[] _spaceScenes;
	public SkyboxRotate _strandedSpaceRotation;

	public GameObject _powerOutageMsg;

	public GameObject _backPanelCover;

	public GameObject _emergencySparks;
    public GameObject _puzzleSparks;
    public GameObject _puzzlelight;
	public Material _skyboxMaterial;
	public AudioSource _steamAudio, _hyperdrivePrimedAudio, _hyperdriveErrorAudio;
	public float _diagnosticHoldTime = 2;

	Vector3 _openConsolePos;
	public Keypad _keypad;

	public Text	ConsoleTxt;

	// Used to move the story forward based on what the player is doing
	public SequenceTrigger _currentInput;

	// Used to prevent diagnostic mode from toggling on/off repeatedly if players keep on pressing the diagnostic button past the required time
	bool _enableModeChange = true;

	public List<SequenceCues> _sequenceCues;

	public AudioSource _currentCue;

	void disableAllInteractables ()
	{
		//_charview.removeCurrentFocus ();

		foreach (Interactable interactable in _interectables) {
			interactable.disableInteractable ();
		}
	}

	void enableAllInteractables (bool overridePrevious = false)
	{
		//_charview.removeCurrentFocus ();

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
		//_wiresPanel.PressedEvent += pressedEvent;
		_wireMiniGame.TriggerEvent += eventTriggered;


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
		//_wiresPanel.PressedEvent -= pressedEvent;
		_wireMiniGame.TriggerEvent -= eventTriggered;
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
				_currentInput = SequenceTrigger.HYPERDRIVE_PRIMED;
				_hyperdrivePrimedAudio.Play ();

			} else if (eventName.Contains ("7178")) {
				ConsoleTxt.text = "HyperDrive Primed";
				_currentInput = SequenceTrigger.HYPERDRIVE_PRIMED;
				if (_hyperDrive2Done)
					_hyperDrive3Primed = true;
				else
					_hyperDrive2Primed = true;
				_hyperdrivePrimedAudio.Play ();
			} else {
				if(eventName == ("keypad_up"))
				{
					_currentInput = SequenceTrigger.KEYPAD_UP;
				}
				else if (eventName == ("keypad_down"))
				{
					_currentInput = SequenceTrigger.KEYPAD_DOWN;	
				}
				else
				{
					_currentInput = SequenceTrigger.WRONG_LAUNCH_CODE;
					ConsoleTxt.text = "Wrong Launch Code";
					_hyperdriveErrorAudio.Play ();
				}
			}
		}

		if (eventName == "emergencyPower" && !_emergencyPowerOn && _powerOutage) {
			automaticPowerBack ();

		}
		if (eventName == "safetyOverride" && _emergencyPowerOn) {
			_safetyOverride = true;
			_currentInput = SequenceTrigger.SAFETY_BYPASSEED;
		}

		if (!_powerBypass && eventName == "power_bypassed") {
			_currentInput = SequenceTrigger.WIRES_BYPASSED;
			_powerBypass = true;
			_windShieldPrompt.showPowerRepaired ();
		}

	}

	void pressedEvent (Interactable interactable)
	{
		if (!_cockpitActivated)
			return;
		
		if (interactable._eventName.Equals ("gravity_button")) {
			_currentInput = SequenceTrigger.ARTIFICIALGRAVITY_BUTTON;
			_windShieldPrompt.showGravity ();
		}
		if (interactable._eventName.Equals ("engines_button")) {
			_currentInput = SequenceTrigger.ENGINES_BUTTON;
			_windShieldPrompt.showEngines ();
		}
		if (interactable._eventName.Equals ("powers_button")) {
			_currentInput = SequenceTrigger.POWERS_BUTTON;
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
			_currentInput = SequenceTrigger.LIFESUPPORT_BUTTON;
			_windShieldPrompt.showLife ();
		}
	
		if (interactable._eventName.Equals ("panel_removed")) {
			_currentInput = SequenceTrigger.PANEL_REMOVED;
		}




	}

	void releasedEvent (Interactable interactable)
	{

		if (interactable._eventName.Equals ("initializeDrill")) {
			_enableModeChange = true;
		}


		if (interactable._eventName.Equals ("initializeDrill") && interactable._pressDuration <= _diagnosticHoldTime) {
			if (_hyperDrive1Primed) {// || (_powerBypass && _hyperDrive2Primed)) {
				_currentInput = SequenceTrigger.HYPERSPACE_JUMP1_BEGIN;
				hideConsole ();
				_keypad.bringDown ();
				//Invoke ("bringConsoleBack", 13.5f);
				StartCoroutine (switchSpaceVisual ());

				_EffectsAnimations.Play ("HyperDriveSuccess");
				_windShieldPrompt.ARText.text = string.Empty;

				_hyperDrive1Primed = false;
				_hyperDrive1Done = true;

				//_hyperSuccessAudio.GetComponent<AudioSource>().Play() ;
				_hyperSuccessAudio.GetComponent<OSPAudioSource>().Play();
				Debug.Log("Playing h audio");
			} else if (_hyperDrive2Primed) {
				_currentInput = SequenceTrigger.HYPERSPACE_JUMP2_BEGIN;
				hideConsole ();
				_keypad.bringDown ();
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
				_emergencySparks.SetActive (true);
				_puzzleSparks.SetActive(true);

				//_hyperFailureAudio.GetComponent<AudioSource>().Play();
				_hyperFailureAudio.GetComponent<OSPAudioSource>().Play();


			} else if (_hyperDrive3Primed && _powerBypass && _safetyOverride) {
				_currentInput = SequenceTrigger.HYPERSPACE_JUMP3_BEGIN;
				hideConsole ();
				_keypad.bringDown ();
				//Invoke ("bringConsoleBack", 13.5f);
				StartCoroutine (switchSpaceVisual ());

				_EffectsAnimations.Play ("HyperDriveSuccess");
				_windShieldPrompt.ARText.text = string.Empty;

				_hyperDrive1Primed = false;	
				StartCoroutine (gameOver ());

				//_hyperSuccessAudio.GetComponent<>().Play();         
				_hyperSuccessAudio2.GetComponent<OSPAudioSource>().Play();
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

	void holdEvent (Interactable interactable)
	{
		if (interactable._eventName.Equals ("initializeDrill") && interactable._pressDuration > _diagnosticHoldTime && _enableModeChange) {
			_cockpitActivated = !_cockpitActivated;
			_enableModeChange = false;

			if (_cockpitActivated) {
				_minieventPrompt.text = "Diagnostic Mode Activated";
				bringConsoleBack ();
				Debug.Log ("Pressed for 2 seconds the initialize drill!");
				_currentInput = SequenceTrigger.DIAGNOSTIC_ON;
			} else {	
				hideConsole ();
				_minieventPrompt.text = "Diagnostic Mode Disabled";
				_currentInput = SequenceTrigger.DIAGNOSTIC_OFF;
			}
		} 
	}



	void automaticPowerBack ()
	{
		_currentInput = SequenceTrigger.EMERGENCY_POWER_SWITCH;

		_EffectsAnimations.Play ("EmergencyPower");
		_emergencyPowerOn = true;
		_powerOutageMsg.SetActive (false);
		enableAllInteractables (true);

		disableStaticAudio ();

		_emergencyPowerSwitch.transform.GetComponent<Interactable> ().disableInteractable ();

		_strandedSpaceRotation.enabled = true;
		_emergencySparks.SetActive (false);
        //The sparks should stay.

		CancelInvoke ("automaticPowerBack");
	}

	IEnumerator gameOver ()
	{
		yield return new WaitForSeconds (16f);

		disableAllInteractables ();

		while (true) {
			ConsoleTxt.text = "Welcome Back";
			yield return new WaitForSeconds (1.25f);
			ConsoleTxt.text = "Thanks for playing";
			yield return new WaitForSeconds (1.25f);
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
				_backPanelCover.gameObject.SetActive(false);
                _puzzlelight.gameObject.SetActive(true);
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


		if (_useNarration) {
			StartCoroutine (StartGameNarration ());
		}
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.H)) {
			_EffectsAnimations.Play ("HyperDriveSuccess");

		}

		if (Input.GetKeyUp (KeyCode.J)) {
			_EffectsAnimations.Play ("HyperDriveFailure");
			
		}
	}

	void ChangeSkyBoxRotation ()
	{
		float currentRotation = _skyboxMaterial.GetFloat ("_Rotation");
		float newRotation = Random.Range (0, 5);
		float time = Mathf.Abs (newRotation - currentRotation) * 10f;
		_skyboxMaterial.DOFloat (newRotation, "_Rotation", time);
		Invoke ("ChangeSkyBoxRotation", time + 0.1f);
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

	void consumeCurrentInput()
	{
		_currentInput = SequenceTrigger.NONE;
	}

	GameObject getRandomCueForSequence(string sequenceName)
	{
		List<GameObject> cuePrefabs = _sequenceCues.Find (item => item.name == sequenceName).cuePrefabs;


		return cuePrefabs [UnityEngine.Random.Range (0, cuePrefabs.Count)];
	}



	void playCue(string sequenceName)
	{
		if(_currentCue == null || !_currentCue.isPlaying)
		{
			_currentCue = SoundManager._instance.playSound(getRandomCueForSequence(sequenceName));
		}
	}

	private float cueWaitTime = 0.0f;
	private float pauseTimeMin = 0.8f;
	private float pauseTimeMax = 1.3f;
	void playCueLoop(string sequenceName)
	{
		if(_currentCue == null || (!_currentCue.isPlaying && Time.time > cueWaitTime))
		{
			_currentCue = SoundManager._instance.playSound(getRandomCueForSequence(sequenceName));
			cueWaitTime = Time.time + _currentCue.clip.length * (1 + Random.Range(pauseTimeMin,pauseTimeMax));
		}
	}

	void playCueReplace(string sequenceName)
	{
		if (_currentCue != null) {
			_currentCue.gameObject.SetActive (false);
			_currentCue = null;
		}

		playCue (sequenceName);
	}

	IEnumerator playCueQueued (string sequenceName)
	{
		while (_currentCue.isPlaying) {
			yield return null;
		}
		_currentCue = SoundManager._instance.playSound(getRandomCueForSequence(sequenceName));
	}

	IEnumerator WaitDiagnosticMode(bool enabled)
	{
		bool goNextStep = false;
		while (!goNextStep) 
		{
			playCue ("WaitDiagnostic");
			//Debug.Log ("Wait DIAGNOSTIC "+ enabled);
			goNextStep = enabled ? _currentInput == SequenceTrigger.DIAGNOSTIC_ON : _currentInput == SequenceTrigger.DIAGNOSTIC_OFF;
			if (goNextStep)
				consumeCurrentInput ();
			
			yield return null;
		}
	}

	IEnumerator WaitCheckAllModulesNoOrder()
	{
		bool goNextStep = false;
		bool artificialPressed, enginesPressed, powersPressed, lifeSupportPressed;
		artificialPressed= enginesPressed= powersPressed= lifeSupportPressed = false;

		StartCoroutine(playCueQueued ("LookAtShipsModules"));

		while (!goNextStep) 
		{

			if (_currentInput == SequenceTrigger.ARTIFICIALGRAVITY_BUTTON) {
				artificialPressed = true;
				consumeCurrentInput ();
			}
			if (_currentInput == SequenceTrigger.ENGINES_BUTTON) {
				enginesPressed = true;
				consumeCurrentInput ();
			}
			if (_currentInput == SequenceTrigger.LIFESUPPORT_BUTTON) {
				powersPressed = true;
				consumeCurrentInput ();
			}
			if (_currentInput == SequenceTrigger.POWERS_BUTTON) {
				lifeSupportPressed = true;
				consumeCurrentInput ();
			}

			if (artificialPressed && enginesPressed && powersPressed && lifeSupportPressed)
				goNextStep = true;

			Debug.Log ("Wait all 4 module buttons no order");
			yield return null;
		}
	}

	IEnumerator WaitCheckAllModules()
	{
		bool goNextStep = false;
		bool artificialPressed, enginesPressed, powersPressed, lifeSupportPressed;
		artificialPressed= enginesPressed= powersPressed= lifeSupportPressed = false;

		StartCoroutine(playCueQueued ("LookAtShipsModules"));

		yield return StartCoroutine(WaitCheckModule (SequenceTrigger.ARTIFICIALGRAVITY_BUTTON));
		yield return StartCoroutine(WaitCheckModule (SequenceTrigger.ENGINES_BUTTON));
		yield return StartCoroutine(WaitCheckModule (SequenceTrigger.POWERS_BUTTON));
		yield return StartCoroutine(WaitCheckModule (SequenceTrigger.LIFESUPPORT_BUTTON));

	}

	IEnumerator WaitCheckModule(SequenceTrigger trigger)
	{
		bool goNextStep = false;

		while (!goNextStep) 
		{
			switch (trigger) {
			case SequenceTrigger.ARTIFICIALGRAVITY_BUTTON:
				playCue ("PressArtificialButton");
				break;
			case SequenceTrigger.ENGINES_BUTTON:
				playCue ("PressEnginesButton");
				break;
			case SequenceTrigger.POWERS_BUTTON:
				playCue ("PressPowersButton");
				break;
			case SequenceTrigger.LIFESUPPORT_BUTTON:
				playCue ("PressLifeSupportButton");
				break;
			}

			if (_currentInput == trigger) {
				goNextStep = true;
				consumeCurrentInput ();
			}
			
			Debug.Log ("Wait trigger: "+trigger);
			yield return null;
		}
	}

	IEnumerator WaitHyperDrivePrimed(string soundCue)
	{
		bool goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.HYPERDRIVE_PRIMED) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			playCueLoop (soundCue);

			Debug.Log ("Wait HYPERDRIVE_PRIMED");
			yield return null;
		}
	}

	IEnumerator WaitKeypadUP()
	{
		bool goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.KEYPAD_UP) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			playCueLoop ("keypad_prime_dialogue");

			Debug.Log ("Wait KEYPAD_UP");
			yield return null;
		}
	}

	IEnumerator StartGameNarration()
	{
		
		StartCoroutine (BeforeJump ());
		yield return null;
	}

	IEnumerator BeforeJump()
	{
		bool goNextStep = false;

		playCue ("hello_dialogue");

		yield return new WaitForSeconds (_currentCue.clip.length * 1.2f);

		playCue ("hello_dialogue2");

		yield return new WaitForSeconds (_currentCue.clip.length * 1.2f);

		yield return StartCoroutine (WaitKeypadUP ());

		yield return StartCoroutine (WaitHyperDrivePrimed ("coordinates_dialogue"));

		goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.HYPERSPACE_JUMP2_BEGIN) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			Debug.Log ("Wait HYPERSPACE_JUMP2_BEGIN");
			yield return null;
		}

		StartCoroutine (MalfunctionBegin ());
	}

	IEnumerator MalfunctionBegin()
	{
		yield return new WaitForSeconds (15);

		bool goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.EMERGENCY_POWER_SWITCH) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			playCueLoop ("error_dialogue");

			Debug.Log ("Wait EMERGENCY_POWER_SWITCH");
			yield return null;
		}

		playCueReplace("power_restored_dialogue");

		goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.PANEL_REMOVED) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			playCueLoop ("puzzle_dialogue");


			Debug.Log ("Wait PANEL_REMOVED");
			yield return null;
		}

		goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.WIRES_BYPASSED) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			playCueLoop ("safety_dialogue");

			Debug.Log ("Wait WIRES_BYPASSED");
			yield return null;
		}

		goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.SAFETY_BYPASSEED) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			Debug.Log ("Wait SAFETY_BYPASSEED");
			yield return null;
		}

		yield return StartCoroutine (WaitHyperDrivePrimed ("coordinates_2_dialogue"));

		goNextStep = false;
		while (!goNextStep) 
		{
			if (_currentInput == SequenceTrigger.HYPERSPACE_JUMP3_BEGIN) {
				goNextStep = true;
				consumeCurrentInput ();
			}

			Debug.Log ("Wait HYPERSPACE_JUMP3_BEGIN");
			yield return null;
		}

		StartCoroutine (GameOver ());
	}

	IEnumerator GameOver()
	{
		yield return new WaitForSeconds (15);

		Debug.Log ("Game Over");
	}
}
