using UnityEngine;
using System.Collections;

public class Global {

	public static string nextScene = "MainCity";
	public static string preScene = "";
	public static readonly string LoadingSName = "LoadingScene";

	public static void  NextScene(string sceneName)
	{
		if(sceneName != null)
		{
			Global.preScene = Global.nextScene;
			Global.nextScene = sceneName;
		}
	}
}
