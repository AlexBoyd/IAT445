using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;




public class SoundManagerWindow : EditorWindow
{

	enum PrefabCreationAction {add,replace,nothing};

	[MenuItem ("Window/Sound Manager")]
	public static void ShowWindow ()
	{
		EditorWindow.GetWindow (typeof(SoundManagerWindow));
	}

	SoundManager _soundManager;
	GameObject _templatePrefab;
	string _audioFolder;
	PrefabCreationAction _creationAction;
	bool _replaceOldPrefabs = true;

	bool audioFolderOK ()
	{
		bool OK = false;
		if (Directory.Exists (_audioFolder)) {
			OK = true;
		}

		return OK;
	}

	void OnGUI ()
	{
		// The actual window code goes here
		_soundManager = (SoundManager)EditorGUILayout.ObjectField ("Sound Manager Object", _soundManager, typeof(SoundManager), true);

		if (_soundManager != null) {
			string audioFolder = EditorGUILayout.TextField ("Audio Clips folder (Ex.:Assets/Sound)", _audioFolder);
			if (!string.IsNullOrEmpty (audioFolder)) {
				_audioFolder = audioFolder.EndsWith ("/") ? audioFolder.Substring (0, audioFolder.Length - 1) : audioFolder;
			}

			GameObject templatePrefab = (GameObject)EditorGUILayout.ObjectField ("Audio Source Template", _templatePrefab, typeof(GameObject), false);
			if (templatePrefab != _templatePrefab) {
				if (templatePrefab==null || templatePrefab.GetComponent<AudioSource> ())
					_templatePrefab = templatePrefab;
				else
					Debug.LogError ("Template does not have AudioSource");
			}

			_replaceOldPrefabs = EditorGUILayout.Toggle ("Replace", _replaceOldPrefabs);
			_creationAction = (PrefabCreationAction)EditorGUILayout.EnumPopup ("SoundManager Starting Pool", _creationAction);


			if (audioFolderOK ()) {
				if (GUILayout.Button ("Create prefabs")) {
					createPrefabs (_replaceOldPrefabs);
				}
			}
				
		}
	}

	List<T> getAllAssetsInFolder<T> (string folder) where T : UnityEngine.Object
	{
		if (!Directory.Exists (_audioFolder)) {
			return null;
		}
		string[] files = Directory.GetFiles (folder);

		List<T> typeMatchFiles = new List<T> ();

		foreach (string file in files) {
			Object o = AssetDatabase.LoadAssetAtPath (file, typeof(T));
			if (o != null)
				typeMatchFiles.Add ((T)o);
		}
		return typeMatchFiles;
	}

	List<string> getAllFilesInFolder<T> (string folder) where T : UnityEngine.Object
	{
		if (!Directory.Exists (_audioFolder)) {
			return null;
		}
		string[] files = Directory.GetFiles (folder);

		List<string> typeMatchFiles = new List<string> ();

		foreach (string file in files) {
			Object o = AssetDatabase.LoadAssetAtPath (file, typeof(T));
			if (o != null)
				typeMatchFiles.Add (file);
		}
		return typeMatchFiles;
	}

	string getFileName (string path)
	{
		return path.Substring (path.LastIndexOf ("/") + 1);
	}

	void createPrefabs (bool replaceOldPrefabs)
	{
		List<AudioClip> allFiles = getAllAssetsInFolder<AudioClip> (_audioFolder);
		List<GameObject> prefabs = new List<GameObject> ();

		foreach (AudioClip clip in allFiles) {

			string filenamePathFinal = _audioFolder + "/" + clip.name + ".prefab";

			GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject> (filenamePathFinal);

			if (gameObject != null && !	replaceOldPrefabs)
				continue;

			bool replacing = gameObject != null;

			if (_templatePrefab == null) {
				if (!replacing) {
					gameObject = new GameObject (clip.name);
					gameObject.AddComponent<AudioSource> ();
				}
			} else {
				if (!replacing) {
					gameObject = GameObject.Instantiate (_templatePrefab);
					gameObject.name = clip.name;
				}
			}
			AudioSource source= gameObject.GetComponent<AudioSource> ();
			source.clip = clip;
		
			if (replacing) {
				EditorUtility.SetDirty (gameObject);

				prefabs.Add (gameObject);
			} else {
				PrefabUtility.CreatePrefab (filenamePathFinal, gameObject);
				DestroyImmediate (gameObject);

				GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject> (filenamePathFinal);
				prefabs.Add (prefab);
			}


		}

		switch (_creationAction) 
		{
		case PrefabCreationAction.add:
			int oldPoolSize = _soundManager._startingGameObjects.Length;
			int newPoolSize = oldPoolSize + prefabs.Count;
			System.Array.Resize (ref _soundManager._startingGameObjects, newPoolSize);
			System.Array.Resize (ref _soundManager._startingPoolCount, newPoolSize);

			for (int i = oldPoolSize; i < newPoolSize; i++) 
			{
				_soundManager._startingGameObjects [i] = prefabs [i - oldPoolSize];
				_soundManager._startingPoolCount [i] = 1;
			}

			break;
		case PrefabCreationAction.nothing:
			break;
		case PrefabCreationAction.replace:
			_soundManager._startingGameObjects = prefabs.ToArray();
			_soundManager._startingPoolCount = new int[prefabs.Count];
			for (int i = 0; i < _soundManager._startingPoolCount.Length; i++) {
				_soundManager._startingPoolCount [i] = 1;
			}
			break;
		}

		EditorUtility.SetDirty (_soundManager);
	}
}