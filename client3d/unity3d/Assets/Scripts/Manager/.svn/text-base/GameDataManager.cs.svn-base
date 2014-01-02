using UnityEngine;
using System.Collections;



public class GameDataManager : Singleton< GameDataManager >
{

	void Awake ()
	{
		if ( mInstance == null )
		{
			mInstance = this;
			DontDestroyOnLoad( gameObject );
		}
		else
		{
			Destroy( gameObject );
		}
		
	}
	
	public void loadAll()
	{
		GamePlayerData.instance.loadData();
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
