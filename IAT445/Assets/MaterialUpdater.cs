using UnityEngine;
using System.Collections;

public class MaterialUpdater : MonoBehaviour
{


	public float _MatTransparency;
	public float _MatXTile;
	public float _MatYTile;
	public float _MatXOffset;
	public float _MatYOffset;

	private Material _Mat;

	public void Start ()
	{
		_Mat = GetComponent<Renderer> ().material;
	}

	// Update is called once per frame
	void Update ()
	{
		_Mat.SetFloat ("_Transparency", _MatTransparency);
		_Mat.SetTextureOffset ("_Diffuse", new Vector2 (_MatXOffset, _MatYOffset));
		_Mat.SetTextureScale ("_Diffuse", new Vector2 (_MatXTile, _MatYTile));
	}
}
