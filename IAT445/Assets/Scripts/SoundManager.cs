using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	public GameObject[] _startingGameObjects;
	public int[] _startingPoolCount;
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

		checkPool (prefab);
		_pool[prefab].Add(go);

		return go;
	}

	void checkPool (GameObject prefab)
	{
		if (!_pool.ContainsKey (prefab)) {
			_pool [prefab] = new List<GameObject> ();
		}
		
	}

	void createInitialPool ()
	{
		_pool = new Dictionary<GameObject, List<GameObject>> ();
		int minIndex = Mathf.Min (_startingPoolCount.Length, _startingGameObjects.Length);

		for (int i = 0; i < minIndex; i++) {
			pool (_startingGameObjects [i], _startingPoolCount [i]);
		}
	}

	void Awake()
	{
		if (_instance != null && _instance != this) {
			Destroy (this);
		} else {
			_instance = this;

			MusicVolume = getSavedMusicVolume ();
			SFXVolume = getSavedSFXVolume ();
	
			createInitialPool ();
		}
	}


	void stopSound (string name)
	{
		GameObject prefab = findPrefab (name);

		foreach (GameObject go in _pool[prefab]) 
		{
			go.GetComponent<AudioSource>().Stop();
		}
	}

	bool validPrefab(GameObject prefab)
	{
		return prefab.GetComponent<AudioSource> () != null;
	}
	
	public GameObject getObject(GameObject prefab)
	{

		if (!_pool.ContainsKey (prefab)) {
			if (validPrefab (prefab)) {
				return instantiate (prefab);
			} else {
				Debug.LogError ("Sound Manager can't play prefab (" + prefab.name + ") because it does not contain a AudioSource");
				return null;
			}
		}

		GameObject go = _pool [prefab].Find (item => item.activeInHierarchy == false);
		if (go != null)
			return go;

		go = instantiate (prefab);


		return go;

	}


	GameObject findPrefab(string clipName)
	{
		GameObject prefab = null;
		foreach(KeyValuePair<GameObject,List<GameObject>> pair in _pool)
		{
			if (pair.Key.name == clipName)
				prefab = pair.Key;
		}

		if (prefab == null) {
			foreach(GameObject go in _startingGameObjects)
			{
				if (go.name == clipName)
					prefab = go;
			}
		}
		if(prefab == null)
			Debug.LogError ("ERROR: SOUND " + clipName + " NOT FOUND!!!");

		return prefab;
	}

	public AudioSource playSound(string audioClipName, bool loop=false, bool oneShot=true, bool overrideVolume =  false, float volume = 1, bool overridePitch = false, float pitch = 1)
	{
		GameObject prefab = findPrefab(audioClipName);
		AudioSource sourcePrefab = prefab.GetComponent<AudioSource> ();

		if (prefab == null)
			return null;
		
		GameObject go = getObject (prefab);

		go.SetActive (true);

		AudioSource source = go.GetComponent<AudioSource> ();

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
		
		return source;

	}



	public AudioSource playSound(GameObject prefab, bool loop=false, bool oneShot=true, bool overrideVolume =  false, float volume = 1, bool overridePitch = false, float pitch = 1)
	{
		GameObject go = getObject (prefab);
		AudioSource sourcePrefab = prefab.GetComponent<AudioSource> ();

		go.SetActive (true);

		AudioSource source = go.GetComponent<AudioSource> ();

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

		return source;
	}

	void playSound(AudioClip audioClip, bool loop=false, bool oneShot=true, float volume = 1)
	{
		GameObject go = new GameObject (audioClip.name);

		AudioSource source = go.AddComponent<AudioSource> ();

		source.clip = audioClip;

		source.volume = volume;

		source.Play ();

		source.loop = loop;

	}

	IEnumerator disableForClipLength (GameObject go)
	{

		yield return null;
		//Debug.LogWarning ("Source:" + source);

		yield return new WaitForSeconds (go.GetComponent<AudioSource>().clip.length);

		go.SetActive (false);

	}


}

