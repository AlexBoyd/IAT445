using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Keypad : MonoBehaviour {

	public KeypadButton[] _keypadButtons;

	public string _eventName;

	public string _currentInput;

	public Text _keypadInput;
	public event StringDelegate TriggerEvent;

	void OnTriggerEvent()
	{
		if (TriggerEvent != null)
			TriggerEvent (_eventName);
	}

	void Awake()
	{
		if (_keypadButtons == null) 
		{
			_keypadButtons = GetComponentsInChildren<KeypadButton> () as KeypadButton[];
		}
	}

	void OnEnable()
	{
		foreach(KeypadButton button in _keypadButtons)
		{
			button.ButtonPressed += keypadButtonPressed;
		}
	}

	void OnDisable()
	{
		foreach(KeypadButton button in _keypadButtons)
		{
			button.ButtonPressed -= keypadButtonPressed;
		}
	}

	void keypadButtonPressed(string keyValue)
	{
		_currentInput += keyValue;
		_keypadInput.text = _currentInput;
	}


}
