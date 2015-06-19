using UnityEngine;
using System.Collections;


public delegate void InterectableDelegate (Interactable interactable);

public class Interactable : MonoBehaviour {

	MeshRenderer _meshRenderer;

	static Material _sharedLitMaterial;

	public Color _boxColor;

	Material _originalMaterial;

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
			_sharedLitMaterial = Resources.Load("InteractableShared") as Material;
		}
		_meshRenderer = GetComponentInChildren<MeshRenderer> ();
		_originalMaterial = _meshRenderer.material;
		_originalMaterial.color = _boxColor;
		_meshRenderer.material = _originalMaterial;


		Debug.LogWarning ("_orign: " + _originalMaterial);

	}

	public void litUp()
	{
		Color c = _originalMaterial.color;
		c.a = 0.8f;
		_sharedLitMaterial.color = c;
		_sharedLitMaterial.SetColor("_EmissionColor",c);

		_meshRenderer.material = _sharedLitMaterial;
	}

	public void unlit()
	{
		_meshRenderer.material = _originalMaterial;
	}

	public virtual void triggerPressedEvent()
	{
		_pressStartTime = Time.time;


		OnPressedEvent ();

	}

	public virtual void triggerReleasedEvent()
	{
		_pressDuration = Time.time - _pressStartTime;

		OnReleasedEvent ();

	}

}
