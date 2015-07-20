using UnityEngine;
using System.Collections;

public delegate void StringDelegate (string eventName);

public class SwitchPanel : MonoBehaviour {

	public Switch[] _switches;


	public bool[] _rightCombination;
	public bool[] _lastCombination;

	public string _eventName;


	public event StringDelegate TriggerEvent;

	void OnTriggerEvent()
	{
		if (TriggerEvent != null)
			TriggerEvent (_eventName);
	}

	bool combinationChanged()
	{
		for (int i = 0; i < _switches.Length; i++) 
		{
			if (_switches [i]._isOn != _lastCombination[i]) 
			{
				return true;
			}
		}
		return false;
	}

	void Start()
	{
		_lastCombination = new bool[_switches.Length];
	}

	void updateLastCombination()
	{
		for (int i = 0; i < _switches.Length; i++) 
		{
			_lastCombination[i] = _switches [i]._isOn; 
		}
	}

	// Update is called once per frame
	void Update () 
	{
		checkForCombination ();
	}

	void checkForCombination ()
	{
		int count = 0;
		for (int i = 0; i < _switches.Length; i++) 
		{
			if (_switches [i]._isOn == _rightCombination[i]) 
			{
				count++;
			}
		}

		if (count == _switches.Length && combinationChanged()) 
		{
			OnTriggerEvent ();
		}

		updateLastCombination ();
	}

}
