using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;

// ContinuationManager class made by kjems (http://answers.unity3d.com/questions/221651/yielding-with-www-in-editor.html)
using System.IO;


internal static class ContinuationManager
{
		private class Job
		{
				public Job(Func<bool> completed, Action<string> continueWith, string filename)
				{
						Completed = completed;
						ContinueWith = continueWith;
						this.filename = filename;
				}
				public Func<bool> Completed { get; private set; }
		public Action<string> ContinueWith { get; private set; }
				public string filename;
		}

		private static readonly List<Job> jobs = new List<Job>();

	public static void Add(Func<bool> completed, Action<string> continueWith, string filename)
		{
				if (!jobs.Any()) EditorApplication.update += Update;
				jobs.Add(new Job(completed, continueWith,filename));
		}

		private static void Update()
		{
				for (int i = 0; i >= 0; --i)
				{
						var jobIt = jobs[i];
						if (jobIt.Completed())
						{
								jobIt.ContinueWith(jobIt.filename);
								jobs.RemoveAt(i);
						}
				}
				if (!jobs.Any()) EditorApplication.update -= Update;
		}
}

class GoogleTTS : EditorWindow {
		[MenuItem ("Window/GoogleTTS")]

		public static void  ShowWindow () {
				EditorWindow.GetWindow(typeof(GoogleTTS));
		}

		TextAsset _textAsset;

		int _linesCounter = 1;
		string _audioOutputFolder;
		void OnGUI () {


			
				_textAsset = EditorGUILayout.ObjectField ("Text File",_textAsset, typeof(TextAsset), false) as TextAsset;

				if(_textAsset != null)
				{
					string audioFolder = EditorGUILayout.TextField ("Audio Clips folder (Ex.:Assets/Sound)", _audioOutputFolder);
					if (!string.IsNullOrEmpty (audioFolder)) {
						_audioOutputFolder = audioFolder.EndsWith ("/") ? audioFolder.Substring (0, audioFolder.Length - 1) : audioFolder;
					}

						if (GUILayout.Button ("Just do it")) 
						{
								_linesCounter = 0;
								string[] lines = _textAsset.text.Split ('\n');


								for (int i = 0; i < lines.Length; i++) {

										string dialogueName, dialogue;
					dialogue = lines [i].Substring (lines [i].IndexOf (":") + 1);
					dialogueName = lines [i].Substring (0,lines [i].IndexOf (":"));

					var www = new WWW (getTextURL (dialogue));
										ContinuationManager.Add (
												// Wait for
												() => www.isDone, 
												// Continue with
						(filename) => {
												if (!string.IsNullOrEmpty (www.error))
														Debug.Log ("WWW failed: " + www.error);
												else {
								File.WriteAllBytes(audioFolder+"/"+filename+".mp3", www.bytes);
//														AudioClip clip = www.GetAudioClip (false, false, AudioType.WAV);
//														SavWav.Save (audioFolder+"/line"+i, clip);
//														SavWav.Save ("TTS/line"+(filename), clip);
														Debug.Log ("WWW result : " + www.text);
												}
										
												
							}, dialogueName);
								}

								AssetDatabase.Refresh ();
						}
								
				}

				// The actual window code goes here
		}
				



		string getTextURL (string text)
		{
				// Remove the "spaces" in excess
				Regex rgx = new Regex ("\\s+");
				// Replace the "spaces" with "% 20" for the link Can be interpreted
				string result = rgx.Replace (text, "%20");
				string url = "http://translate.google.com/translate_tts?tl=en&q=" + result;
				Debug.LogWarning ("URL: " + url);
				return url;

		}
}