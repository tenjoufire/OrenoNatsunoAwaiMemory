using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppCoreControl : MonoBehaviour
{
	public int frameRateLimit = 90;
	public bool enableVsync = false;
	public int maxQueuedFrames = 2;
	public bool pauseOnFocusLoss = false;
	public enum AASetting { None , x2 , x4 , x8 }
	public AASetting antialiasing = AASetting.x4;

	[ContextMenu("Update Settings Now")]
	void Start ()
	{
		Application.runInBackground = !pauseOnFocusLoss;
		Application.targetFrameRate = frameRateLimit;
		if ( enableVsync )
			QualitySettings.vSyncCount = 1;
		else
			QualitySettings.vSyncCount = 0;
		QualitySettings.maxQueuedFrames = maxQueuedFrames;
		QualitySettings.antiAliasing = (int)antialiasing;
    }

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
	}
}
