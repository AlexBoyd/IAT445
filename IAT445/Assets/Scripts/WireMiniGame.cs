using UnityEngine;
using System.Collections;
using System.Linq;

public class WireMiniGame : MonoBehaviour
{

	public WirePiece[] _pieces;

	public event StringDelegate TriggerEvent;

	public string _eventName = "power_bypassed";

	void OnTriggerEvent ()
	{
		if (TriggerEvent != null)
			TriggerEvent (_eventName);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		checkPieces ();
	}

	void checkPieces ()
	{
		for (int i = 0; i < _pieces.Length; i++) {
			if (_pieces [i].OrientedCorrectly) {
				if (!_pieces [i].Locked)
					_pieces [i].lockPiece ();
			} else {
				break;
			}
		}
		if (_pieces.Where ((p) => p.Locked).Count () == 5 ) {

			OnTriggerEvent ();

		}
	}
}
