using UnityEngine;
using System.Collections;
using System.Timers;
using System.Collections;
using System;

public  class UGBodyObjectsManager 
{
	static UGBodyObjectsManager _assManager = null;
	Timer autoTimer;
	ArrayList _objectsArr;

	bool _autoRelease = false;
	int _ugTagInfo = 0;

	public static UGBodyObjectsManager instance()
	{
		if(_assManager == null)
		{
			 _assManager = new UGBodyObjectsManager();
			_assManager.UGInit();

		}else
		{

		}

		return _assManager;
	}

	//释放池刷新时间
	private double interval = 1000d;
	public double PoolInterval {set{interval = value;} get{return interval;}}

	void UGInit()
	{
		_objectsArr = new ArrayList();
	
		autoTimer = new Timer(interval);
		autoTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReleasePool);
		autoTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
		autoTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

		if(_autoRelease)
		{
			autoTimer.Start();
		}else
		{
			autoTimer.Stop();
		}
	}

	public void Dispose()
	{
		_objectsArr.Clear();
		_objectsArr = null;
		autoTimer.Dispose();
	}

	public void AddObject(UGBody body)
	{
		if(!_objectsArr.Contains(body))
		{
			_objectsArr.Add(body);
		}
	}

	public void RemoveObject(UGBody body)
	{
		if(_objectsArr.Contains(body))
		{
			//计数不大于0，表示没有游戏体引用该资源束
			if(body.ugAssets.Keeping <= 0)
			{
				Debug.Log("UGBodyObjectsManager RemoveObject  " );
				UGBundlePoolManager.instance().UnloadeUGAssetsBundle(body.assetsPackage);
			}

			_ugTagInfo--;
			_objectsArr.Remove(body);
		}
	}

	public CreateBody CreateUGBody(string assetsPackage,int packageVersion)
	{
		GameObject createBody = GameObject.Find("UGCreateBody");
		CreateBody body = 	createBody.AddComponent("CreateBody") as CreateBody;

		body.packageVersion = packageVersion;
		body.BodyName = "MainPlayer"+_ugTagInfo;
		body.ugTag = _ugTagInfo++;

		return body;
	}

	public CreateBody CreateUGBody(string assetsPackage,int packageVersion,string bodyName)
	{
		GameObject createBody = GameObject.Find("UGCreateBody");
		CreateBody body = 	createBody.AddComponent("CreateBody") as CreateBody;

		body.packageVersion = packageVersion;
		body.BodyName = bodyName;
		body.ugTag = _ugTagInfo++;
		return body;
	}

	public CreateBody CreateUGBody(string assetsPackage,int packageVersion,string bodyName,int ugTag)
	{
		GameObject createBody = GameObject.Find("UGCreateBody");
		CreateBody body = 	createBody.AddComponent("CreateBody") as CreateBody;

		body.packageVersion = packageVersion;
		body.BodyName = bodyName;
		body.ugTag = ugTag;
		_ugTagInfo++;

		return body;
	}


	public GameObject TryGetUGGameObject(string bodyName)
	{
		foreach(UGBody assetObject in _objectsArr)
		{
			if(assetObject.BodyName == bodyName)
			{
				return assetObject.UGGameObject;
			}
		}

		return null;
	}

	public GameObject TryGetUGGameObject(int ugTag)
	{
		foreach(UGBody assetObject in _objectsArr)
		{
			if(assetObject.ugTag == ugTag)
			{
				return assetObject.UGGameObject;
			}
		}

		return null;
	}

	public void ReloadAllAssets(bool destroyInstantiate)
	{
		foreach(UGBody assetObject in _objectsArr)
		{
			UGBundlePoolManager.instance().UnloadeUGAssetsBundle(assetObject.assetsPackage);
		}
	}

	public static void CloseManager()
	{

		if(_assManager != null)
		{
			_assManager.Dispose();
			Debug.Log(" UGBodyObjectRelaesed!! ");
			_assManager = null;
			GC.Collect();
		}
	}

	public void SetAutoUnloadAssets(bool auto)
	{

		if(auto)
		{
			if(auto != _autoRelease)
			{
				autoTimer.Start();
			}
		}else
		{
			if(auto != _autoRelease)
				autoTimer.Stop();
		}

		_autoRelease = auto;
	}

	void ReleasePool(object source, System.Timers.ElapsedEventArgs e)
	{
		foreach(UGBody assetObject in _objectsArr)
		{

			UGAssetsBundle ugAssets = UGBundlePoolManager.instance().TryGetUGAssetsBundle(assetObject.assetsPackage);
			if(ugAssets.Keeping <= 0)
			{
				UGBundlePoolManager.instance().UnloadeUGAssetsBundle(assetObject.assetsPackage);
				Debug.Log(" Execute ReleasePool . ReleaseAssets "+assetObject.assetsPackage );
				Debug.LogWarning("PoolbodyObjects : " + UGBundlePoolManager.instance().Count );
			}
		}

		Debug.Log("ReleasePool active");
	}


}
