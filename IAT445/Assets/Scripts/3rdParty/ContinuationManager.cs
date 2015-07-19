// Script made by kjems (http://answers.unity3d.com/questions/221651/yielding-with-www-in-editor.html)

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;

internal static class ContinuationManager
{
		private class Job
		{
				public Job(Func<bool> completed, Action continueWith)
				{
						Completed = completed;
						ContinueWith = continueWith;
				}
				public Func<bool> Completed { get; private set; }
				public Action ContinueWith { get; private set; }
		}

		private static readonly List<Job> jobs = new List<Job>();

		public static void Add(Func<bool> completed, Action continueWith)
		{
				if (!jobs.Any()) EditorApplication.update += Update;
				jobs.Add(new Job(completed, continueWith));
		}

		private static void Update()
		{
				for (int i = 0; i >= 0; --i)
				{
						var jobIt = jobs[i];
						if (jobIt.Completed())
						{
								jobIt.ContinueWith();
								jobs.RemoveAt(i);
						}
				}
				if (!jobs.Any()) EditorApplication.update -= Update;
		}
}