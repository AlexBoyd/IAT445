using UnityEngine;
using System.Collections;


public class KeypadButton : Interactable
{

//	Animator _animator;
//	public AudioSource Audio;

	public string _value;

	public event StringDelegate ButtonPressed;

	void OnButtonPressed()
	{
		if (ButtonPressed!=null) 
		{
			ButtonPressed (_value);
		}

	}

	void Awake ()
	{
		base.Awake ();
	}

	public  void pressed ()
	{

		OnButtonPressed ();

	}


	public override void triggerPressedEvent ()
	{
		if (!_interactable)
			return;

		pressed ();

		base.triggerPressedEvent ();
	}

	void OnHandHoverBegin()
	{
		this.litUp ();
	}
	
	void OnHandHoverEnd()
	{
		this.unlit ();
	}
	
	void HandHoverUpdate ( VRHand hand )
	{
		if(hand.GetStandardInteractionButtonDown() || 
		   (hand.controller != null && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger )))
		{
			triggerPressedEvent();
		}
		
		if(hand.GetStandardInteractionButtonUp() || 
		   (hand.controller != null && hand.controller.GetPressUp( Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger )))
		{
			triggerReleasedEvent();
		}
	}


}
