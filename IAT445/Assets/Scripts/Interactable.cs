using UnityEngine;
using System.Collections;


public delegate void InterectableDelegate (Interactable interactable);

public class Interactable : MonoBehaviour {

	public Renderer _meshRenderer;
	public int _materialIndex = 0;

	static Material _sharedLitMaterial;
	static Color _normalGlowColor = new Color(255/255f,145/255f,26/255f);
	static Color _pressedGlowColor = new Color(26/255f,189/255f,255/255f);

	public Color _boxColor;

	public Material _originalMaterial;

	public string _eventName;
	public string _eventNamePretty;

	public event InterectableDelegate PressedEvent;
	public event InterectableDelegate ReleasedEvent;
	public event InterectableDelegate HoldEvent;

	float _pressStartTime ;
	public float _pressDuration;

	public AudioSource Audio;

	public bool _interactable = true;

	bool _previousInteractableState;

	public void disableInteractable()
	{
		_previousInteractableState = _interactable;

		_interactable = false;

	}

	public void enableInteractable(bool overridePrevious)
	{
		_interactable = overridePrevious ? true : _previousInteractableState;
	}

	void OnPressedEvent()
	{
		if (PressedEvent != null)
			PressedEvent (this);
	}

	void OnReleasedEvent()
	{
		if (ReleasedEvent != null)
			ReleasedEvent (this);
	}

	void OnHoldEvent()
	{
		if (HoldEvent != null)
			HoldEvent (this);
	}

	protected void Awake()
	{

		if (_sharedLitMaterial == null) 
		{
			_sharedLitMaterial = Resources.Load("UltraGlow") as Material;
		}
		if(_meshRenderer == null)
			_meshRenderer = GetComponentInChildren<Renderer> ();
		
		_originalMaterial = _meshRenderer.materials[_materialIndex];

		if(Audio==null)
			Audio = GetComponentInChildren<AudioSource> ();

	}
		
	public void litUp()
	{
		if (!_interactable)
			return;

		_sharedLitMaterial.mainTexture = _originalMaterial.mainTexture;

		_sharedLitMaterial.SetColor("_GlowColor",_normalGlowColor);
		Material[] materials = _meshRenderer.materials;
		materials[_materialIndex] = _sharedLitMaterial;
		_meshRenderer.materials = materials;
	}

	public void unlit()
	{
		if (!_interactable)
			return;

		Material[] materials = _meshRenderer.materials;
		materials[_materialIndex] = _originalMaterial;
		_meshRenderer.materials = materials;
	}

	public virtual void triggerPressedEvent()
	{
		if (!_interactable)
			return;

		_pressStartTime = Time.time;
		_sharedLitMaterial.SetColor("_GlowColor",_pressedGlowColor);

		if (Audio!=null) {
			Audio.Play ();
		}

		OnPressedEvent ();
	}

	public virtual void triggerReleasedEvent()
	{
		if (!_interactable)
			return;

		_pressDuration = Time.time - _pressStartTime;
		_sharedLitMaterial.SetColor("_GlowColor",_normalGlowColor);

		OnReleasedEvent ();
	}

	public virtual void triggerHoldEvent()
	{
		if (!_interactable)
			return;

		_pressDuration = Time.time - _pressStartTime;
		OnHoldEvent ();
	}


}
