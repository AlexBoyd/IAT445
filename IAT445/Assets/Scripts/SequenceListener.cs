using UnityEngine;
using UnityEngine.UI;
using System.Collections;




public class SequenceListener : MonoBehaviour {

	Interactable[] _interectables;

	public Text _eventPrompt;

	string _requiredEvent;

	void Awake()
	{
		_interectables = GameObject.FindObjectsOfType<Interactable> () as Interactable[];
	}

	void OnEnable()
	{
		foreach (Interactable i in _interectables)
			i.TriggerEvent += eventTriggered;
	}

	void OnDisable()
	{
		foreach (Interactable i in _interectables)
			i.TriggerEvent -= eventTriggered;
	}

	void eventTriggered(string eventName)
	{
		if (eventName == _requiredEvent) 
		{
			promptRandomInteractable();
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
