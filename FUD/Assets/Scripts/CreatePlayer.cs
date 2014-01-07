using UnityEngine;
using System.Collections;
using System.Threading;

public class CreatePlayer : UGAssetObject ,AssetsEventsInterFace
{

	bool flag = false;

	// Use this for initialization
	void Start () 
	{
		_loadSucDelegate += new AssetsLoadedEventHandle(this.AssetsLoaded);
		_loadFaicDelegate += new AssetsLoadedEventHandle(this.AssetsLoadFaild);
//		this.Init();

		Global.Version = 0;
		LoadAsset("PlayerAsset");
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void ReleaseMyAssetBundle()
	{
		if(flag) UnloadResource(true);
			else UnloadAsset(false);
//		print(" Player ReleaseAsset ");
	}

	public void DestroyPlayer()
	{
		Destroy(GameObject.FindGameObjectWithTag("Player"));
		Resources.UnloadUnusedAssets();
	}

	void OnDestroy()
	{
		print(" CreatePlayer Script Destroied ");
	}

	public void AssetsLoaded()
	{
		GameObject player;

		if(flag){
			player = LoadResource("Characters/U_Character/U_Character_REF_FB");
			player.transform.position = new Vector3(30f,0f,45f);
		}else
		{
			if(_assetBundle != null)
			{
				print("LoadAsset Succesfully");
//				Object obj = _assetBundle.Load("U_Character_REF_FB");
				Object obj = _assetBundle.mainAsset;
				if(obj == null)
				{
					Debug.LogError(" 你所加载的资源为NULL ");
					UnloadAsset(true);
				}else
				{
					player = Instantiate(obj) as GameObject;
					player.transform.position = new Vector3(30f,0f,45f);
//					UnloadAsset(false);
				}

			}else
			{
				Debug.LogError("LoadAsset Failed !!");
			}
		}
	}

	public void AssetsLoadFaild()
	{

	}
}
