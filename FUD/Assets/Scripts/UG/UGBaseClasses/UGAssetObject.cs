using UnityEngine;
using System.Collections;
using System.Threading;

public class UGAssetObject : MonoBehaviour
{

	protected UGAssetObject()
	{

	}

	/**
	 * 	  继承本类的类必须要实现
	 *   必须实现接口 AssetsEventsInterFace
	 * 	 事件 			    AssetsLoadedEventHandle
	 */

	protected event AssetsLoadedEventHandle _loadSucDelegate; 
	protected event AssetsLoadedEventHandle _loadFaicDelegate; 

	void AssetsLoadedSuccess()
	{
		if(_loadSucDelegate != null) _loadSucDelegate();
	}

	void AssetsLoadedFailed()
	{
		if(_loadFaicDelegate != null) _loadFaicDelegate();
	}

	//Resource
	private GameObject _object_self = null;
	private Object _assetBoj = null;

	//AssetsBundle
	protected AssetBundle _assetBundle = null;
	
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

	public AssetBundle LoadAsset(string path)
	{
		if(_assetBundle == null)
		{
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

//			print("AssetBundle path: "+PathURL + "  \n Gloable:" + Global.PathURL);
			StartCoroutine( this.LoadFromCacheOrDownload(PathURL));
		}

		return _assetBundle;
	}

	private IEnumerator LoadFromCacheOrDownload(string path)
	{
//		print(" AssetVersion : " + Global.Version);
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
			this.AssetsLoadedFailed();
		}else
		{
			_assetBundle = www.assetBundle;
			this.AssetsLoadedSuccess();
		}
	}

	public void UnloadAsset(bool destroyThis)
	{
		print("UGAssetObject [UnloadAsset] : " + _assetBundle);

		if(_assetBundle != null){
			_assetBundle.Unload(destroyThis);
			_assetBundle = null;
		}
	}
	
}
