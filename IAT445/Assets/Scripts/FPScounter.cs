/* **************************************************************************
 * FPS COUNTER
 * **************************************************************************
 * Written by: Annop "Nargus" Prapasapong
 * Created: 7 June 2012
 * *************************************************************************/
 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
/* **************************************************************************
 * CLASS: FPS COUNTER
 * *************************************************************************/
public class FPScounter : MonoBehaviour {
	/* Public Variables */
	public float frequency = 0.5f;
 
 	public Text fpsCounterText;
 	
	/* **********************************************************************
	 * PROPERTIES
	 * *********************************************************************/
	public int FramesPerSec { get; protected set; }
 
	/* **********************************************************************
	 * EVENT HANDLERS
	 * *********************************************************************/
	/*
	 * EVENT: Start
	 */
	private void Start() {
		StartCoroutine(FPS());
	}
 
	void Update(){
		if(Input.GetKeyDown (KeyCode.Y)) {
		QualitySettings.vSyncCount = 0;
		}
	}
 
	/*
	 * EVENT: FPS
	 */
	private IEnumerator FPS() {
		for(;;){
			// Capture frame-per-second
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
			yield return new WaitForSeconds(frequency);
			float timeSpan = Time.realtimeSinceStartup - lastTime;
			int frameCount = Time.frameCount - lastFrameCount;
 
			// Display it
			FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
			fpsCounterText.text = FramesPerSec.ToString() + " fps";
		}
	}
}