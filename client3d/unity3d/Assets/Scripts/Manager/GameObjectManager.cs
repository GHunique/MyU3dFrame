using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameObjectManager : GameHandlerManager< GameObjectManager >
{
	
	void Awake ()
	{
		if ( mInstance == null )
		{
			mInstance = this;
		}
	}

	public GameObject createObject( string name )
	{
		string s = GameSetting.SpritePath;
		s += name;
		
		GameObject cloneObject = (GameObject)Resources.Load (s);

		return (GameObject)Instantiate( cloneObject );
	}

	public GameObject getCloneObject( string name )
	{
		string s = GameSetting.SpritePath;
		s += name;
		
		GameObject cloneObject = (GameObject)Resources.Load (s);

		return cloneObject;
	}
	
	#if UNITY_EDITOR
	
	public bool editorReload = false;
	
	void Update()
	{
		if ( editorReload )
		{
			editorReload = false;
		}
	}
	#endif
}

