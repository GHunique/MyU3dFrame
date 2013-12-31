using UnityEngine;
using System.Collections;

public class UGBehaviour : MonoBehaviour {

	/**
	 *		保存子类信息
	 */
	private ArrayList _childrenArr = null;
	private GameObject _object_self = null;
	private Object _assetBoj = null;
	protected AssetBundle _assetBundle = null;

	public GameObject _target = null;
	public string _callBackFName = "";

	public UGBehaviour()
	{
		_childrenArr = new ArrayList();
	}

	~UGBehaviour()
	{

		_childrenArr.Clear();

//		UnloadResource(true);
	}

	protected void AssetsLoaded()
	{
		print (" Parent AssetsLoaded ");
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
		GameObject.DestroyImmediate(_object_self);
//		Resources.UnloadAsset(_assetBoj);
		Resources.UnloadUnusedAssets();
	}

	public AssetBundle LoadAsset(string path,GameObject target,string callbackFName)
	{

		if(_assetBundle == null)
		{
			_target = target;
			_callBackFName = callbackFName;

				string url="file://"+Application.dataPath+"/"+path+".unity3d";
				print("AssetBundle path: "+url);
				StartCoroutine( this.LoadFromCacheOrDownload(url));
		}

		return _assetBundle;
	}

	private IEnumerator LoadFromCacheOrDownload(string path)
	{
		print(" -----0000000---------- " + Global.Version);
		// 载入 AssetBundle

		if(Caching.IsVersionCached(path,Global.Version))
		{
			Debug.LogWarning("这个版本的资源已经缓存了。为了继续加载资源，要清理缓存！");
		}

		WWW www =  WWW.LoadFromCacheOrDownload(path,Global.Version);
		//等待载入完成
		yield return www;

		_assetBundle = www.assetBundle;

		this.AssetsLoaded();

		_target.SendMessage(_callBackFName,gameObject,SendMessageOptions.DontRequireReceiver);

		print(" --------------- " + _assetBundle);
	}

	public void UnloadAsset(bool removeBunld)

	{
		print("UGBehaviour [UnloadAsset]");
//		GameObject.DestroyImmediate(_object_self);
		_assetBundle.Unload(removeBunld);
	}
}
