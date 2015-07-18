using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Keypad : MonoBehaviour
{

	public bool _isUp;

	Animator _animator;

	public KeypadButton[] _keypadButtons;

//	public string _eventName;

	public string _currentInput;

	public Text _keypadInput;
	public event StringDelegate TriggerEvent;

	KeypadHandle _keypadHandle;

	public Text	ConsoleTxt;

	public AudioSource _KeypadAudio;

	void OnTriggerEvent (string eventName)
	{
		if (TriggerEvent != null)
			TriggerEvent ("keypad_" + eventName);
	}

	void Awake ()
	{
		_animator = GetComponent<Animator> ();
		_keypadHandle = GetComponent<KeypadHandle> ();
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

	public void toggleUpDown() 
	{
		_animator.SetBool ("Up", !_isUp);
		_isUp = !_isUp;

		string eventName = _isUp ? "up" : "down";
		OnTriggerEvent (eventName);
	}

	public void bringUp()
	{
		_isUp = true;
		_animator.SetBool ("Up", _isUp);
	}
	public void bringDown()
	{
		_isUp = false;
		_animator.SetBool ("Up", _isUp);
	}

	void keypadButtonPressed (string keyValue)
	{	
		_KeypadAudio.Play ();
		if (keyValue == "#") {
			OnTriggerEvent (_currentInput);
			_currentInput = string.Empty;
		} else if (keyValue == "*") {
			_currentInput = string.Empty;

		} else {
			
			_currentInput += _currentInput.Length < 5 ? keyValue : "";
		}
		_keypadInput.text = _currentInput;
	}


}
