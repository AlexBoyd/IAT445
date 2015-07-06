using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	

	public float getSavedSFXVolume()
	{
		if (PlayerPrefs.HasKey ("SFXVolume"))
			return PlayerPrefs.GetFloat ("SFXVolume");

		PlayerPrefs.SetFloat ("SFXVolume", 1);

		return 1;
	}
	public float getSavedMusicVolume()
	{
		if (PlayerPrefs.HasKey ("MusicVolume"))
			return PlayerPrefs.GetFloat ("MusicVolume");
		
		PlayerPrefs.SetFloat ("MusicVolume", 1);

		return 1;
	}
	public void saveSFXVolume(float v)
	{
		PlayerPrefs.SetFloat ("SFXVolume", v);	
	}
	public void saveMusicVolume(float v)
	{
		PlayerPrefs.SetFloat ("MusicVolume", v);
	}


	public float MusicVolume
	{
		get{ return getSavedMusicVolume();}
		set { _musicVolume = value; saveMusicVolume(value);}
	}

	public float SFXVolume
	{
		get{ return getSavedSFXVolume();}
		set { _sfxVolume = value;saveSFXVolume(value);}
	}
	float _musicVolume, _sfxVolume;

	public static SoundManager _instance;

	public GameObject[] _nStartingGameObjects;
	public int[] _nStartingPool;
	Dictionary<GameObject, List<GameObject>> _pool;

	public void pool(GameObject prefab, int n)
	{
		
		int hash = prefab.GetHashCode ();

		for(int i = 0; i < n; i++)
		{
			GameObject go = instantiate(prefab);
			
			checkPool(prefab);
			_pool[prefab].Add(go);
		}
	}
	public GameObject instantiate(GameObject prefab)
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		
		go.SetActive(false);
		
		go.transform.parent = transform;
		
		return go;
	}

	void checkPool (GameObject prefab)
	{
		if (!_pool.ContainsKey (prefab)) 
			_pool.Add (prefab, new List<GameObject> ());
		
	}

	void Awake()
	{
		_instance = this;

		MusicVolume = getSavedMusicVolume ();
		SFXVolume = getSavedSFXVolume ();
	
//		_instance = this;
		_pool = new Dictionary<GameObject, List<GameObject>> ();
		
		int minIndex = Mathf.Min (_nStartingPool.Length, _nStartingGameObjects.Length);
		
		for(int i = 0; i < minIndex; i++)
		{
			pool(_nStartingGameObjects[i],_nStartingPool[i]);	
		}
	}

	void Start()
	{
	}

	void stopSound (string name)
	{
		GameObject prefab = findPrefab (name);


		foreach (GameObject go in _pool[prefab]) 
		{
			go.GetComponent<AudioSource>().Stop();
		}
	}

	
	public GameObject getObject(GameObject prefab)
	{

		if (!_pool.ContainsKey (prefab)) 
			return null;
		
		foreach (GameObject go in _pool[prefab]) 
		{
			if(!go.activeInHierarchy)
			{
				return go;
			}
		}

//		return instantiate(prefab);

//		Debug.LogWarning("Nao tem no pool, cria");
		GameObject go2 = instantiate (prefab);
		checkPool(prefab);
		
		_pool[prefab].Add(go2);
		
		return go2;

	}


	GameObject findPrefab(string clipName)
	{
		foreach (GameObject go in _nStartingGameObjects) 
		{
//			Debug.LogError("go.name"+go.name);
//			Debug.LogError("name"+clipName);
			if(go.name == clipName)
				return go;
		}

		Debug.LogError ("ERROR: SOUND " + clipName + " NOT FOUND!!!");
	return null;
	}

	public AudioSource playSound(string audioClipName, bool loop=false, bool oneShot=true, bool overrideVolume =  false, float volume = 1, bool overridePitch = false, float pitch = 1)
	{
		GameObject prefab = findPrefab(audioClipName);
		AudioSource sourcePrefab = prefab.GetComponent<AudioSource> ();

		if (prefab == null)
						return null;

		//GameObject go = new GameObject (audioClip.name);
		
		GameObject go = getObject (prefab);

		go.SetActive (true);

		AudioSource source = go.GetComponent<AudioSource> ();
		
		//source.clip = audioClip;

		float vol = sourcePrefab.volume;

		if(overrideVolume)
			vol = volume;

		vol = prefab.CompareTag ("Music") ? vol * _musicVolume : vol * _sfxVolume; 

		source.volume = vol;


		if (overridePitch)
			source.pitch = pitch;	

		source.Play ();
		
		source.loop = loop;

		if(oneShot)
			StartCoroutine (disableForClipLength (go));

//		_allSources.Add (source);

		return source;
		
		//return source;
	}

	void playSound(AudioClip audioClip, bool loop=false, bool oneShot=true, float volume = 1)
	{
		GameObject go = new GameObject (audioClip.name);
		
		//GameObject go = getObject (audioClip);
		
		AudioSource source = go.AddComponent<AudioSource> ();
		
		source.clip = audioClip;
		
		source.volume = volume;
		
		source.Play ();
		
		source.loop = loop;
		
//		_allSources.Add (source);
		
		//return source;
	}

	void playSound(GameObject audioPrefab, bool loop=false, bool oneShot=true, float volume = 1)
	{
		//GameObject go = new GameObject (clip.name);
		
		GameObject go = getObject (audioPrefab);

		go.SetActive (true);

		AudioSource audioSource = go.AddComponent<AudioSource> ();
		
		//source.clip = audioPrefab.audio.clip;
		
		audioSource.volume = volume;
		
		audioSource.Play ();
		
		audioSource.loop = loop;


		StartCoroutine (disableForClipLength (go));

		//go.transform.GetComponent<MonoBehaviour>().Invoke ("OnDisable", source.clip.length);

//		_allSources.Add (audioSource);
		
		//return source;
	}

	IEnumerator disableForClipLength (GameObject go)
	{

		yield return null;
		//Debug.LogWarning ("Source:" + source);

		yield return new WaitForSeconds (go.GetComponent<AudioSource>().clip.length);

		go.SetActive (false);

	}

//	void playSound(AudioClip clip, bool loop=false, bool oneShot=true, float volume = 1)
//	{
//		//GameObject go = new GameObject (clip.name);
//
//		GameObject go = getObject (clip);
//
//		AudioSource source = go.AddComponent<AudioSource> ();
//
//		source.clip = clip;
//
//		source.volume = volume;
//
//		source.Play ();
//
//		source.loop = loop;
//
//		_allSources.Add (source);
//
//		//return source;
//	}

//	IEnumerator computeFreq()
//	{
//		while (true) {
//		
//
//
//						cleanAllSources ();
//		
//						int n = 64;
//		
//						float[] spectrumSum = new float[n];
//		
//						foreach (AudioSource source in _allSources) {
//								float[] spectrum = source.GetSpectrumData (n, 0, FFTWindow.BlackmanHarris);
//			
//								for (int j = 0; j < spectrum.Length; j++) {
//										spectrumSum [j] += spectrum [j];
//								}
//
//						}
//		
//		
//						int i = 1;
//						while (i < n-1) {
//								Debug.DrawLine (new Vector3 (Mathf.Log (i - 1), Mathf.Log (spectrumSum [i - 1]), 3), new Vector3 (Mathf.Log (i), Mathf.Log (spectrumSum [i]), 3), Color.yellow);
//								i++;
//						}
//
//
//			UIHandler._instance.setSpectrum(spectrumSum);
//			yield return null;
//				}
//	}
	
	// Update is called once per frame
	void Update () {
	

//
//		cleanAllSources ();
//
//		int n = 256;
//
//		float[] spectrumSum = new float[n];
//
//		foreach (AudioSource source in _allSources) 
//		{
//			float[] spectrum = source.GetSpectrumData (n, 0, FFTWindow.BlackmanHarris);
//
//			for (int j = 0; j < spectrum.Length; j++)
//			{
//				spectrumSum[j] += spectrum[j];
//			}
//
////						int i = 1;
////						while (i < 1023) {
////								Debug.DrawLine (new Vector3 (i - 1, spectrum [i] + 10, 0), new Vector3 (i, spectrum [i + 1] + 10, 0), Color.red);
////								Debug.DrawLine (new Vector3 (i - 1, Mathf.Log (spectrum [i - 1]) + 10, 2), new Vector3 (i, Mathf.Log (spectrum [i]) + 10, 2), Color.cyan);
////								Debug.DrawLine (new Vector3 (Mathf.Log (i - 1), spectrum [i - 1] - 10, 1), new Vector3 (Mathf.Log (i), spectrum [i] - 10, 1), Color.green);
////								Debug.DrawLine (new Vector3 (Mathf.Log (i - 1), Mathf.Log (spectrum [i - 1]), 3), new Vector3 (Mathf.Log (i), Mathf.Log (spectrum [i]), 3), Color.yellow);
////								i++;
////						}
//				}
//
//
//		int i = 1;
//		while (i < n-1) {
//			//Debug.DrawLine (new Vector3 (i - 1, spectrumSum [i] + 10, 0), new Vector3 (i, spectrumSum [i + 1] + 10, 0), Color.red);
//			//Debug.DrawLine (new Vector3 (i - 1, Mathf.Log (spectrumSum [i - 1]) + 10, 2), new Vector3 (i, Mathf.Log (spectrumSum [i]) + 10, 2), Color.cyan);
//			//Debug.DrawLine (new Vector3 (Mathf.Log (i - 1), spectrumSum [i - 1] - 10, 1), new Vector3 (Mathf.Log (i), spectrumSum [i] - 10, 1), Color.green);
//			Debug.DrawLine (new Vector3 (Mathf.Log (i - 1), Mathf.Log (spectrumSum [i - 1]), 3), new Vector3 (Mathf.Log (i), Mathf.Log (spectrumSum [i]), 3), Color.yellow);
//			i++;
//		}
	}

	/*
	 * */


//	void stopMusic (AudioClip clip)
//	{
//		
//		for(int i = _allSources.Count-1; i>=0; i--)
//		{
//			if(_allSources[i].clip==clip)
//			{
//				_allSources[i].Stop();
//				Destroy(_allSources[i].gameObject);
//				_allSources.RemoveAt(i);
//			}
//		}
//	}
//
//	void cleanAllSources ()
//	{
//
//		for(int i = _allSources.Count-1; i>=0; i--)
//		{
//			if(_allSources[i]==null)
//				_allSources.RemoveAt(i);
//		}
//	}

}

