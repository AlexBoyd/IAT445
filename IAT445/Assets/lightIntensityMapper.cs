using UnityEngine;
using System.Collections;

public class lightIntensityMapper : MonoBehaviour
{

	public Light _sourceLight;

	private Material _mat;

    public MeshRenderer[] _turnedOnObj;
    public MeshRenderer[] _turnedOffObj;

	void Start ()
	{
		_mat = GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_mat.SetColor ("_EmissionColor", _sourceLight.color * _sourceLight.intensity * 0.1f);
        if (_sourceLight.intensity > 0.1f)
        {
            for (int i = 0; i < _turnedOnObj.Length; i++)
            {
                _turnedOnObj[i].enabled = true;
            }
            for (int i = 0; i < _turnedOffObj.Length; i++)
            {
                _turnedOffObj[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < _turnedOnObj.Length; i++)
            {
                _turnedOnObj[i].enabled = false;
            }
            for (int i = 0; i < _turnedOffObj.Length; i++)
            {
                _turnedOffObj[i].enabled = true;
            }
        }
	}
}
