using UnityEngine;
using System.Collections;



public class GameManager : Singleton< GameManager >
{
	
	void Awake ()
	{
		if ( mInstance == null )
		{
			mInstance = this;
			DontDestroyOnLoad( gameObject );

			initGame();
		}
		else
		{
			Destroy( gameObject );
		}
	}


	public void initGame()
	{
		GameConfigManager.instance.loadAll();
		GameDataManager.instance.loadAll();

	}

	public void releaseUnused()
	{
		LoginUIHandler.instance.Show();
		LoginUIHandler.instance.UnShow();

		GameUIManager.instance.releaseUnusedHandler();
		GameObjectManager.instance.releaseUnusedHandler();

		Resources.UnloadUnusedAssets();
	}
	
	
	#if UNITY_EDITOR
	
	public bool editorRelease = false;
	
	void Update()
	{
		if ( editorRelease )
		{
			releaseUnused();
			
			editorRelease = false;
		}
	}
	
	#endif
}
