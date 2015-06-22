using UnityEngine;
using System.Collections;

public class TransparencyToIntensity : MonoBehaviour
{


	public Light DestionationLight;
	public Renderer SourceRender;
	public float Scale;

	
	// Update is called once per frame
	void Update ()
	{
		DestionationLight.intensity = SourceRender.material.GetFloat ("_Transparency") * Scale;
	}
}
