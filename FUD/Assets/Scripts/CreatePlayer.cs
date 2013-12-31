using UnityEngine;
using System.Collections;

public class CreatePlayer : UGBehaviour {

	// Use this for initialization
	void Start () 
	{
		Global.Version = 0;
		LoadAsset("PlayerAsset",gameObject,"AssetsLoaded");
	}
	
	// Update is called once per frame
	void Update ()
	{
//		print(" CreatPlayer update");
	}

	public void ReleaseAsset()
	{

//		GameObject.DestroyImmediate(gameObject);
		UnloadAsset(true);
//		print(" Player ReleaseAsset ");
	}

	void OnDestroy()

	{
		print(" CreatePlayer Script Destroied ");	
	}

	void AssetsLoaded()
	{
//		print("   AssetsLoaded =======  ");

		bool flag = false;
		GameObject player;
		
		//		print("haracters/U_Character/U_Character_REF_FB");
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
				}

			}else
			{
				print("LoadAsset Failed !!");
			}
		}
	}
}
