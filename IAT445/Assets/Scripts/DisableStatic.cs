using UnityEngine;
using System.Collections;

public class DisableStatic : MonoBehaviour {

	public SequenceListener _sequenceListener;

	public void enableStatic()
	{
		_sequenceListener.enableStaticAudio ();
	}

	public void disableStatic()
	{
		_sequenceListener.disableStaticAudio ();
	}
}
