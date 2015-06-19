using UnityEngine;
using System.Collections;

public delegate void StringDelegate (string eventName);

public class SwitchPanel : MonoBehaviour {

	public Switch[] _switches;


	public bool[] _rightCombination;

	public string _eventName;


	public event StringDelegate TriggerEvent;

	void OnTriggerEvent()
	{
		if (TriggerEvent != null)
			TriggerEvent (_eventName);
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

		if (count == _switches.Length) 
		{
			OnTriggerEvent ();
		}
	}

}
