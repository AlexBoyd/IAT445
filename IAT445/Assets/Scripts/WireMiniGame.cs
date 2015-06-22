using UnityEngine;
using System.Collections;
using System.Linq;

public class WireMiniGame : MonoBehaviour
{

	public WirePiece[] _pieces;
	public SequenceListener _SequenceListener;


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
		if (_pieces.All ((p) => p.Locked) && !_SequenceListener._powerBypass) {
			_SequenceListener._powerBypass = true;
		}
	}
}
