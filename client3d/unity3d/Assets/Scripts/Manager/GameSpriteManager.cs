using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameSpriteManager : GameHandlerManager< GameSpriteManager >
{
	
	void Awake ()
	{
		if ( mInstance == null )
		{
			mInstance = this;
			DontDestroyOnLoad( gameObject );
			
			loadAll();
			
		}
		else
		{
			Destroy( gameObject );
		}
		
	}
	
	public void loadAll()
	{

	}

	public GameObject createSprite( string name )
	{
		string s = GameSetting.SpritePath;
		s += name;
		
		GameObject cloneObject = (GameObject)Resources.Load (s);

		return (GameObject)Instantiate( cloneObject );
	}

	public GameObject getCloneSprite( string name )
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
			loadAll();
			
			editorReload = false;
		}
	}
	#endif
}

