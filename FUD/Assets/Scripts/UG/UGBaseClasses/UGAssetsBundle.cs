using UnityEngine;
using System;

public class UGAssetsBundle  
{
	private int _keep = 0;
	public int Keeping{get{return _keep;}}

	private string _assetsName = "No Name";
	public string AssetsName{set{_assetsName = value;} get{return _assetsName;}}

	public AssetBundle assetbundle;
	public UGAssetsBundle(AssetBundle a)
	{
		assetbundle = a;
	}
	
	public void Keep(){_keep++;Debug.Log(_assetsName + "++keep:"+_keep); }
	public void Leave(){_keep--; Debug.Log(_assetsName + "--keep:"+_keep); }

	public void Unload(bool unloadAllLoadedObjects)
	{
		assetbundle.Unload(unloadAllLoadedObjects);
	}

	public void Unload()
	{
		Debug.Log(" UGAssetsbundle  Unload : "+assetbundle);
		assetbundle.Unload(true);
	}
	
}
