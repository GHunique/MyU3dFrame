using UnityEngine;
using System.Collections;



public class LoadingUIHandler : GameUIHandler< LoadingUIHandler >
{

	AsyncOperation async = null;


	void Awake ()
	{
		mInstance = this;

		if ( isLoaded )
		{
			Release();
		}
	}
	
	public override void onRelease()
	{
		
	}
	public override void onInit()
	{
		
	}
	public override void onOpen()
	{
		
	}
	public override void onClose()
	{
		
	}

	public void loadScene( int s )
	{
		StartCoroutine( loadSceneCoroutine( s ) );
	}


	IEnumerator loadSceneCoroutine( int s )
	{
		async = Application.LoadLevelAsync( s );

		yield return async;
	}
	
	
	// Use this for initialization
	void Start () 
	{
		//Show();
	}

	
	// Update is called once per frame
	void Update () 
	{
		if ( async != null )
		{
			//async.progress
		}

	}

}
