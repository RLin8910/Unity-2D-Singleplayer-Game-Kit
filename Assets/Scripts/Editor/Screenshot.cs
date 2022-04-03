using System.IO;
using UnityEngine;
using UnityEditor;

public static class Screenshot
{
    [MenuItem("Game/Take Screen Shot")]
	static void TakeScreenshot(){
        Directory.CreateDirectory("Screenshots");
		string fileName = "Screenshots/Screenshot_" + System.DateTime.Now.ToString ("yyyy-MM-dd_HH-mm-ss") + ".png";
		ScreenCapture.CaptureScreenshot (fileName);
		Debug.Log ("Saved screenshot as " + fileName);
	}
}
