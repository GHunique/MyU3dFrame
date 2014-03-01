using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public delegate void AssetsLoadMessage(AssetBundle assetsBundle);

public class UGBundlePoolManager
{
	static UGBundlePoolManager _UGBPool = null;
	Dictionary<string,UGAssetsBundle> _bundleDic = null;
//	Dictionary<string,UGBody> _targetsDic = null;
	string _assetsname;

	public int Count {get{return _bundleDic.Count;}}
	
	private UGBundlePoolManager(){}
	public static UGBundlePoolManager instance()
	{
		if(_UGBPool == null)
		{
			_UGBPool = new UGBundlePoolManager();
			_UGBPool.UGInit();
		}

		return _UGBPool;
	}

	void UGInit()
	{
		_bundleDic = new Dictionary<string, UGAssetsBundle>();
	}

	public void Dispose()
	{
		_bundleDic.Clear();
		_bundleDic = null;
	}

	public bool ContainsUGAssetsBundle(string key)
	{
		return _bundleDic.ContainsKey(key);
	}

	public UGAssetsBundle TryGetUGAssetsBundle(string key)
	{
		UGAssetsBundle ugAssets = null;
		_bundleDic.TryGetValue(key,out ugAssets);
		return ugAssets;
	}

	public void AddUGAssetsBundle(string key,UGAssetsBundle ugAssets)
	{
		if(!_bundleDic.ContainsKey(key))
		_bundleDic.Add(key,ugAssets);
	}

	public void ReplaceUGAssetsBundle(string key,UGAssetsBundle nugAssets)
	{
			UnloadeUGAssetsBundle(key);
			AddUGAssetsBundle(key,nugAssets);
	}

	public void UnloadeUGAssetsBundle(string assetsName)
	{
		UGAssetsBundle ugAssets = TryGetUGAssetsBundle(assetsName);
		ugAssets.Unload();
		_bundleDic.Remove(assetsName);
		ugAssets = null;
	}

	public static void CloseManager()
	{
		if(_UGBPool != null)
		{
			_UGBPool.Dispose();
			_UGBPool = null;
			System.GC.Collect();
		}
	}

}
