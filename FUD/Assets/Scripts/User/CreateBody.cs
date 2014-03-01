using UnityEngine;
using System.Collections;

public class CreateBody : UGAssetBody
{

	// Use this for initialization
	void Start () 
	{
		this.init();
	}

	void OnDestroy()
	{
		this.UGRelease();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
