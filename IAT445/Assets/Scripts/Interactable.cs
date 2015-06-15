using UnityEngine;
using System.Collections;


public delegate void StringDelegate (string parameter);

public class Interactable : MonoBehaviour {

	MeshRenderer _meshRenderer;

	static Material _sharedLitMaterial;

	public Color _boxColor;

	Material _originalMaterial;

	public string _eventName;
	public string _eventNamePretty;

	public event StringDelegate TriggerEvent;

	void OnTriggerEvent()
	{
		if (TriggerEvent != null)
			TriggerEvent (_eventName);
	}

	void Awake()
	{
		if (_sharedLitMaterial == null) 
		{
			_sharedLitMaterial = Resources.Load("InteractableShared") as Material;
		}
		_meshRenderer = GetComponentInChildren<MeshRenderer> ();
		_originalMaterial = _meshRenderer.material;
		_originalMaterial.color = _boxColor;
		_meshRenderer.material = _originalMaterial;


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

	public void triggerEvent()
	{
		Debug.Log ("Interacted with object: " + name + " and triggered " + _eventName);
		OnTriggerEvent ();
	}

}
