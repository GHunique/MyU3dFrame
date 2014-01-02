using UnityEngine;
using System.Collections;

public class LoginUIComponent : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnClick()
	{
		LoadingUIHandler.instance.Show();
		LoadingUIHandler.instance.loadScene( 1 );
	}
}
