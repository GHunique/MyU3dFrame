using UnityEngine;
using System.Collections;
using System;

public class xsy : xsyParent ,xsyInterface
{

	// Use this for initialization

	void Start () 
	{
		_delegate += new EventHandler(this.AssetsLoaded);
		Debug.Log(" xsy :");
	}

	void test()
	{
		this.AssetsLoadedp();
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnFinished()
	{
		Debug.Log(" OnFinished -- - --!__!_ ");
	}

	public void AssetsLoaded(object sender,EventArgs e)
	{
		Debug.Log(" assests Loaded!!!! ---  !! ");
	}
	
}
