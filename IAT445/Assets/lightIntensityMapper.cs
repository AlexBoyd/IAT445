using UnityEngine;
using System.Collections;

public class lightIntensityMapper : MonoBehaviour
{

	public Light _sourceLight;

	private Material _mat;

	void Start ()
	{
		_mat = GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_mat.SetColor ("_EmissionColor", _sourceLight.color * _sourceLight.intensity * 0.1f);
	}
}
