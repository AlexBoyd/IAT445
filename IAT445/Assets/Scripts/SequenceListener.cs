using UnityEngine;
using UnityEngine.UI;
using System.Collections;




public class SequenceListener : MonoBehaviour {

	Interactable[] _interectables;
	Switch[] _switches;
	SwitchPanel _switchPanel;
	public Text _eventPrompt;

	string _requiredEvent;


	void Awake()
	{
		_interectables = GameObject.FindObjectsOfType<Interactable> () as Interactable[];
		_switchPanel = GameObject.FindObjectOfType<SwitchPanel> () as SwitchPanel;

	}

	void OnEnable()
	{
		foreach (Interactable i in _interectables) {
			i.PressedEvent += pressedEvent;
			i.ReleasedEvent += releasedEvent;
		}


		_switchPanel.TriggerEvent += eventTriggered;
	}

	void OnDisable()
	{
		foreach (Interactable i in _interectables) {
			i.PressedEvent -= pressedEvent;
			i.ReleasedEvent -= releasedEvent;
		}

		_switchPanel.TriggerEvent -= eventTriggered;
	}

	void eventTriggered(string eventName)
	{
		if (eventName == _requiredEvent) 
		{
			promptRandomInteractable();
		}


		if (eventName.Equals ("switchPanel")) 
		{
			Debug.Log ("SwitchPanel");
		}
	}

	void pressedEvent(Interactable interactable)
	{

	}

	void releasedEvent(Interactable interactable)
	{
		if (interactable._eventName.Equals ("initializeDrill") && interactable._pressDuration > 4) {
			Debug.Log ("Pressed for 4 seconds the initialize drill!");
		} else {
			Debug.Log ("Not 4 seconds or not drill!");
		}
	}


	// Use this for initialization
	void Start () {
	
		promptRandomInteractable ();
	}

	void promptRandomInteractable()
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
