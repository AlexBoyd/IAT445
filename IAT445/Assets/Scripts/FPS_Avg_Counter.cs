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
public class FPS_Avg_Counter : MonoBehaviour {


	public int FramesPerSec { get { return (int)(1/_avgFrameDuration); } }

	private float _avgFrameDuration;
 
	public Text fpsCounterText;

	/** The size of the circular window of frame duration. */
	public int FPS_WINDOW_SIZE = 100;

	/** The circular window of frame duration. Used to calculate smooth FPS */
	private float []FPSWindow;

	/** The current sum of all the frames in the window */
	private float frameSum;
	/** The index of the last frame that was added to the window */
	private int frameIndex;

	void Awake()
	{
		FPSWindow = new float[FPS_WINDOW_SIZE];
	}

	/** Add a new frame duration to the window and get the average FPS */
	float getAvgFPS(float deltaTime)
	{

		frameSum -= FPSWindow[frameIndex];
		frameSum += deltaTime;
		FPSWindow[frameIndex] = deltaTime;

		if(++frameIndex==FPS_WINDOW_SIZE)
			frameIndex = 0;

		return frameSum/FPS_WINDOW_SIZE;
	}

	void Update()
	{
		_avgFrameDuration = getAvgFPS (Time.deltaTime);
		fpsCounterText.text = ""+FramesPerSec;
	}

}