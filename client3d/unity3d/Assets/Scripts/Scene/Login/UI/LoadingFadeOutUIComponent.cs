using UnityEngine;
using System.Collections;

public class LoadingFadeOutUIComponent : MonoBehaviour 
{
	private bool isShow = false;
	private float alpha = 1.0f;
	
	
	// Use this for initialization
	void Start ()
	{
		isShow = true;
		//renderer.material.color = new Color( renderer.material.color.r , renderer.material.color.g , renderer.material.color.b , 0f );
	}


	// Update is called once per frame
	void Update () 
	{
		if ( isShow )
		{
			alpha -= Time.deltaTime * 1.5f;
			
			renderer.material.color = new Color( renderer.material.color.r , renderer.material.color.g , renderer.material.color.b , alpha );
			
			if ( alpha <= 0f )
			{
				renderer.material.color = new Color( renderer.material.color.r , renderer.material.color.g , renderer.material.color.b , 0f );
				isShow = false;
				
				LoadingFadeOutUIHandler.instance.Release();
			}
			
		}
	}
	
	
}
