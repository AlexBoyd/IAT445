using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Keypad : MonoBehaviour
{

	public KeypadButton[] _keypadButtons;

//	public string _eventName;

	public string _currentInput;

	public Text _keypadInput;
	public event StringDelegate TriggerEvent;

	public SequenceListener _SequenceListener;

	public Text	ConsoleTxt;

	public AudioSource _KeypadAudio;

	void OnTriggerEvent (string eventName)
	{
		if (TriggerEvent != null)
			TriggerEvent ("keypad_"+eventName);
	}

	void Awake ()
	{
		if (_keypadButtons == null) {
			_keypadButtons = GetComponentsInChildren<KeypadButton> () as KeypadButton[];
		}
	}

	void OnEnable ()
	{
		foreach (KeypadButton button in _keypadButtons) {
			button.ButtonPressed += keypadButtonPressed;
		}
	}

	void OnDisable ()
	{
		foreach (KeypadButton button in _keypadButtons) {
			button.ButtonPressed -= keypadButtonPressed;
		}
	}

	void keypadButtonPressed (string keyValue)
	{	
		_KeypadAudio.Play ();
		if (keyValue == "#") {
			OnTriggerEvent (_currentInput);
			_currentInput = string.Empty;
//			if (_currentInput == "8717") {
//				ConsoleTxt.text = "HyperDrive Primed";
//				_currentInput = string.Empty;
//				_SequenceListener._hyperDrive1Primed = true;
//			} else if (_currentInput == "7178") {
//				ConsoleTxt.text = "HyperDrive Primed";
//				_currentInput = string.Empty;
//				_SequenceListener._hyperDrive2Primed = true;
//			}
		} else if (keyValue == "*") {
			_currentInput = string.Empty;

		} else {
			
			_currentInput += _currentInput.Length < 4 ? keyValue : "";
		}
		_keypadInput.text = _currentInput;
	}


}
