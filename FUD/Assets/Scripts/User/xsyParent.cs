using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;


public class xsyParent  : MonoBehaviour
{

	public event EventHandler _delegate;

	public ArrayList _arrList;

	protected void AssetsLoadedp()
	{
		if(_delegate != null) _delegate(null,null);
	}

	public xsyParent()
	{
		_arrList = new ArrayList();
		OnStart();
	}

	void OnStart()
	{

//		Invoke("InvokeLoaded",1f);

		if(!_arrList.Contains(this))
		{
		_arrList.Add(this);
		Debug.Log(" OnStart !  contain num :  " + _arrList.Count);
		}

		_arrList.Contains(this);
		_arrList.Add(this);
		Debug.Log(" OnStart !  contain num :  " + _arrList.Count);
	}

	void InvokeLoaded()
	{
//		AssetsLoadedp();
		Debug.Log(" InvokeLoaded after 1 second(s) ");
	}
}
