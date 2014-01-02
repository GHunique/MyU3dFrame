using System;
using UnityEngine;
using System.Collections;


public abstract class GameUIHandler< T > : Singleton< T > , GameHandler
{
	public string uiName;
	public Vector3 localPosition;
	public Transform anchor;


	public abstract void onRelease();
	public abstract void onInit();
	public abstract void onOpen();
	public abstract void onClose();


	public static bool isShow = false;
	public static GameObject uiObject = null;
	public static GameObject cloneObject = null;
	public static bool isLoaded = false;
	

	public void Show()
	{
		if ( !isLoaded )
		{
			string s = GameSetting.UIPath;
			s += uiName;
			
			cloneObject = (GameObject)Resources.Load( s );

			if ( !cloneObject ) 
			{
				Debug.LogError( "res not found " + s );
				return;
			}

			uiObject = NGUITools.AddChild( anchor.gameObject , cloneObject );
			uiObject.transform.localPosition = localPosition;

			isLoaded = true;

			GameUIManager.instance.addHandler( uiName , this );

			onInit();
		}
		else
		{
			uiObject.SetActive( true );
		}

		isShow = true;
		
		onOpen ();
	}
	
	public void UnShow()
	{
		if ( !uiObject )
		{
			return;
		}

		isShow = false;
		uiObject.SetActive (false);
		
		onClose ();
	}

	public void ReleaseUnused()
	{
		if ( !isShow )
		{
			Release();
		}
	}
	
	public void Release()
	{
		if ( !isLoaded ) 
		{
			return;
		}

		if ( isShow )
		{
			onClose ();
			isShow = false;
		}

		onRelease ();

		NGUITools.Destroy( uiObject );

		cloneObject = null;
		//DestroyImmediate( cloneObject );
		Resources.UnloadUnusedAssets();

		isLoaded = false;
	}
}


