using UnityEngine;
using System.Collections;
using System.Threading;


/**
 * 初始化时调用 Init() ; 脚本被摧毁时调用 UGRelease()，这将摧毁掉所创建的GameObject
 * 如要摧毁脚本而不摧毁所创建的 GameObject 需调用 UGReleaseWithoutGameObject() ,
 * 并设置为自动管理资源 UGGameSeting::AutoColletAssets  = true;
 * 
 * 
 */

public class UGAssetBody : UGBody
{

	bool flag = false;

	public void ReleaseMyAssetBundle()
	{
		if(flag) UnloadResource();
	}

	public void DestroyPlayer()
	{
		Destroy(GameObject.FindGameObjectWithTag("Player"));
		Resources.UnloadUnusedAssets();
	}

	public override void AssetsLoadedSuccessful(UGAssetsBundle bundle)
	{

		if(flag){
			_body = LoadResource("Characters/U_Character/U_Character_REF_FB");
			_body.transform.position = new Vector3(30f,0f,45f);
		}else
		{
			if(bundle.assetbundle != null)
			{
				Object obj;
				if(string.IsNullOrEmpty(willLoadAssetName)|| willLoadAssetName == "Empty")
					obj = bundle.assetbundle.mainAsset;
				else 
					obj = bundle.assetbundle.Load(willLoadAssetName);
				if(obj == null)
				{
					Debug.LogError(" 资源包中没有名为"+willLoadAssetName+"的资源");
				}else
				{
					_body = Instantiate(obj) as GameObject;
				}
			}else
			{
				Debug.LogError("LoadAsset Failed !!");
			}
		}
	}

	public override void AssetsLoadedFailed()
	{

	}
}
