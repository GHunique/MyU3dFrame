using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public delegate void AssetsLoadedDelegate();
public delegate void TestDelegate();

public class xsyParent : MonoBehaviour {

	AssetsLoadedDelegate _assetsLoaded;
	TestDelegate _tDelegate;

	// Use this for initialization
	protected void setDelegate(/*object target,IntPtr pMethod*/) 
	{
//		_assetsLoaded = new  AssetsLoadedDelegate(target,pMethod);
		Invoke("InvokeLoaded",1.2f);
		Debug.Log(" xsyParent ;");

		_assetsLoaded = new  AssetsLoadedDelegate(this.whatEver);
	}

	void InvokeLoaded()
	{
		_assetsLoaded();
		print(" InvokeLoaded after 1 second(s) ");
	}

	void whatEver()
	{
		Marshal.GetDelegateForFunctionPointer 
		print(" delegate hao nangao! ! :" + Marshal.GetFunctionPointerForDelegate(_assetsLoaded));
	}
}
