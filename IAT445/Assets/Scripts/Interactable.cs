using UnityEngine;
using System.Collections;


public delegate void InterectableDelegate (Interactable interactable);

public class Interactable : MonoBehaviour {

	public Renderer _meshRenderer;

	static Material _sharedLitMaterial;
	static Color _normalGlowColor = new Color(255/255f,145/255f,26/255f);
	static Color _pressedGlowColor = new Color(26/255f,189/255f,255/255f);

	public Color _boxColor;

	protected Material _originalMaterial;

	public string _eventName;
	public string _eventNamePretty;

	public event InterectableDelegate PressedEvent;
	public event InterectableDelegate ReleasedEvent;

	float _pressStartTime ;
	public float _pressDuration;

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

	protected void Awake()
	{

		if (_sharedLitMaterial == null) 
		{
			_sharedLitMaterial = Resources.Load("UltraGlow") as Material;
		}
		if(_meshRenderer == null)
			_meshRenderer = GetComponentInChildren<Renderer> ();


		_originalMaterial = _meshRenderer.material;
		//_originalMaterial.color = _boxColor;
		_meshRenderer.material = _originalMaterial;

	}
		
	public void litUp()
	{
		_sharedLitMaterial.mainTexture = _meshRenderer.material.mainTexture;

		_sharedLitMaterial.SetColor("_GlowColor",_normalGlowColor);


		_meshRenderer.material = _sharedLitMaterial;
	}

	public void unlit()
	{
		_meshRenderer.material = _originalMaterial;
	}

	public virtual void triggerPressedEvent()
	{
		_pressStartTime = Time.time;
		_sharedLitMaterial.SetColor("_GlowColor",_pressedGlowColor);


		OnPressedEvent ();

	}

	public virtual void triggerReleasedEvent()
	{
		_pressDuration = Time.time - _pressStartTime;
		_sharedLitMaterial.SetColor("_GlowColor",_normalGlowColor);


		OnReleasedEvent ();

	}

}
