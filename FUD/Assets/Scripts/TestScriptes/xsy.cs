using UnityEngine;
using System.Collections;

public class xsy : xsyParent ,xsyInterface
{

	// Use this for initialization

	void Start () 
	{
		this.setDelegate();
		Debug.Log(" xsy :");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnFinished()
	{
		Debug.Log(" OnFinished -- - --!__!_ ");
	}

	public void AssetsLoaded()
	{
		Debug.Log(" assests Loaded!!!! ---  !! ");
	}
	
}
