using UnityEngine;
using System.Collections;

public class WireMiniGame : MonoBehaviour {

	public WirePiece[] _pieces;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		checkPieces ();
	}

	void checkPieces()
	{
		for (int i = 0; i < _pieces.Length; i++) 
		{
			if (_pieces [i].OrientedCorrectly) {
				if(!_pieces[i].Locked)
					_pieces [i].lockPiece ();
			} else {
				break;
			}
		}
	}
}
