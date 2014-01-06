using UnityEngine;
using System.Collections;

public class UGAssetObject : MonoBehaviour{

	public delegate int AssetsLoadedDelegate();

	/**
	 *		保存子类信息
	 */
	private GameObject _object_self = null;
	private Object _assetBoj = null;
	protected AssetBundle _assetBundle = null;

	public GameObject _target = null;
	public string _callBackFName = "";
	
	protected void AssetsLoaded()
	{
		print (" Parent AssetsLoaded ");
	}

	protected void AssetsLoadFailed()
	{
		Debug.LogWarning(" parent AssetsLoadFailed !!! ");
	}

	public GameObject LoadResource(string path)
	{
		if(_object_self == null)
		{
			_assetBoj = Resources.Load(path);
			_object_self = Instantiate(_assetBoj) as GameObject;
		}

		return _object_self;
	}

	public void UnloadResource(bool removeBunld)
	{
		print("UGBehaviour [UnloadResource]");
		Resources.UnloadAsset(_assetBoj);
	}

	public AssetBundle LoadAsset(string path,GameObject target,string callbackFName)
	{
		if(_assetBundle == null)
		{
			_target = target;
			_callBackFName = callbackFName;

			RuntimePlatform run_platform = Application.platform;

			//不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
			string PathURL  = "";

			if(run_platform == RuntimePlatform.Android)
			{
				PathURL =  "jar:file://" + Application.dataPath + "!/assets/";
			}
			else if(run_platform == RuntimePlatform.IPhonePlayer)
			{
				 PathURL = Application.dataPath + "/Raw/";
			}
			else if(run_platform == RuntimePlatform.OSXEditor || run_platform == RuntimePlatform.WindowsEditor)
			{
				PathURL = "file://" + Application.dataPath + "/StreamingAssets/";
			}

			PathURL = PathURL+path + ".unity3d";

			print("AssetBundle path: "+PathURL + "  \n Gloable:" + Global.PathURL);
			StartCoroutine( this.LoadFromCacheOrDownload(PathURL));
		}

		return _assetBundle;
	}

	private IEnumerator LoadFromCacheOrDownload(string path)
	{
		print(" AssetVersion : " + Global.Version);
		// 载入 AssetBundle
		if(Caching.IsVersionCached(path,Global.Version))
		{ 
			Debug.LogWarning("这个版本的资源已经缓存了!!!");
		}

		WWW www =  WWW.LoadFromCacheOrDownload(path,Global.Version);
		//等待载入完成
		yield return www;

		if(www.error != null) 
		{
			this.AssetsLoadFailed();
		}else
		{
			_assetBundle = www.assetBundle;
			this.AssetsLoaded();
			_target.SendMessage(_callBackFName,gameObject,SendMessageOptions.DontRequireReceiver);
		}

	}

	public void UnloadAsset(bool removeBunld)

	{
		print("UGAssetObject [UnloadAsset] : " + _assetBundle);
		_assetBundle.Unload(removeBunld);
	}
}
