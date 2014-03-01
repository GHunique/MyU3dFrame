using UnityEngine;
using System.Collections;

public abstract class UGBody : UGObject 
{

	/**
	 *  属性
	 */
	public string assetsPackage = "PlayerAsset";//资源包名字
	public int packageVersion = 0;
	public string willLoadAssetName = "Empty";//U_Character_REF_FB
	public string BodyName = "MainPlayer";
	public int    ugTag = 0;
	protected GameObject _body = null;
	public GameObject UGGameObject {get{return _body;}}

	public UGAssetsBundle ugAssets;
	//属性

	public abstract void AssetsLoadedSuccessful(UGAssetsBundle bundle);
	public abstract void AssetsLoadedFailed();

	public virtual void UGRelease()
	{
		Debug.Log("Body UGReleased ");
		ugAssets.Leave();
		DestroyCreatedGameObject();
		UGBodyObjectsManager.instance().RemoveObject(this);
	}

	public virtual void UGReleaseWithoutGameObject()
	{
		Debug.Log("Body UGReleaseWithoutGameObject ");
		UGBodyObjectsManager.instance().RemoveObject(this);
	}

	public void DestroyCreatedGameObject()
	{
		GameObject.DestroyImmediate(_body);
	}

	//Resource
	private GameObject _object_self = null;
	private Object _assetBoj = null;

	public void init()
	{
		Global.Version = packageVersion;
		UGBodyObjectsManager.instance().AddObject(this);
		LoadAsset();
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
	
	public void UnloadResource()
	{
		print("UGBody [UnloadResource]");
		Resources.UnloadAsset(_assetBoj);
	}


	public bool LoadAsset( )
	{

		if(!UGBundlePoolManager.instance().ContainsUGAssetsBundle(assetsPackage))
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
			
			PathURL = PathURL+assetsPackage + ".unity3d";
			
			//			if(File.Exists(PathURL))
			//			{
			print("AssetBundle path: "+PathURL + "  \n Gloable:" + Global.PathURL);
			StartCoroutine( this.LoadFromCacheOrDownload(PathURL));

			Debug.Log(" StartCoroutine Done!!!! ");

			if(ugAssets != null)
			{
				ugAssets.Keep();
				AssetsLoadedSuccessful(ugAssets);
				return true;
			}else
			{
				return false;
			}
			
		}else
		{
			Debug.Log(" This Assets had Loaded!! ");

			ugAssets = UGBundlePoolManager.instance().TryGetUGAssetsBundle(assetsPackage);
			if(ugAssets != null)
			{
				ugAssets.Keep();
				AssetsLoadedSuccessful(ugAssets);
				return true;
			}
			else 
			{
				AssetsLoadedFailed();
				return false;
			}
		}
	}
	
	private IEnumerator LoadFromCacheOrDownload(string path)
	{

		//		print(" AssetVersion : " + Global.Version);
		// 载入 AssetBundle

		
		WWW www =  WWW.LoadFromCacheOrDownload(path,Global.Version);

		
		if(string.IsNullOrEmpty(www.error))
		{
			if(Caching.IsVersionCached(path,Global.Version))
			{ 
				Debug.LogWarning("这个版本的资源已经缓存了!!! :" + www.error+"wo");

				if(UGBundlePoolManager.instance().ContainsUGAssetsBundle(assetsPackage))
				{
					ugAssets = UGBundlePoolManager.instance().TryGetUGAssetsBundle(assetsPackage);
				}else
				{
					ugAssets = new UGAssetsBundle(www.assetBundle);
					ugAssets.AssetsName = assetsPackage;
					UGBundlePoolManager.instance().AddUGAssetsBundle(assetsPackage,ugAssets);
				}

			}else
			{
				ugAssets = new UGAssetsBundle(www.assetBundle);
				ugAssets.AssetsName = assetsPackage;

				if(UGBundlePoolManager.instance().ContainsUGAssetsBundle(assetsPackage))
				{
					UGBundlePoolManager.instance().ReplaceUGAssetsBundle(assetsPackage,ugAssets);
				}else
				{
					UGBundlePoolManager.instance().AddUGAssetsBundle(assetsPackage,ugAssets);
				}
			}
		}else
		{
			Debug.LogError("UGBody: "+www.error);
		}

		//等待载入完成
		yield return www;

	}

}
