using UnityEngine;
using System.Collections;



public class Global {

	//不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
	public static readonly string PathURL =
		#if UNITY_ANDROID   //安卓
		"jar:file://" + Application.dataPath + "!/assets/";
	#elif UNITY_IPHONE  //iPhone
	Application.dataPath + "/Raw/";
	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
	"file://" + Application.dataPath + "/StreamingAssets/";
	#else
	string.Empty;
	#endif

	public static string nextScene = "MainCity";
	public static string preScene = "";
	public static readonly string LoadingSName = "LoadingScene";
	public static  int ConnectPort = 9999;
	public static  string ConnectIP = "192.168.2.3";

	public static void  NextScene(string sceneName)
	{
		if(sceneName != null)
		{
			Global.preScene = Global.nextScene;
			Global.nextScene = sceneName;
		}
	}

	public static int Version
	{
		set{PlayerPrefs.SetInt("Version",value); } 
		get{return PlayerPrefs.GetInt("Version");}
	}
}


