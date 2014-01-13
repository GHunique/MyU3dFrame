using System.Collections;
using UnityEngine;

public class ChangePlayerEquipment : MonoBehaviour {

	public UIPopupList m_popList = null;
	GameObject m_matObj = null;
	System.Collections.Generic.List<string> m_popItmes = null;
	// Use this for initialization
	UILabel m_infoTexture = null;


	void Start () 
	{
		EventDelegate.Add(m_popList.onChange,OnSelectionChange);
		m_matObj = GameObject.FindGameObjectWithTag("PlayerMaterial");
		m_popItmes = m_popList.items;
		m_infoTexture = (GameObject.FindGameObjectWithTag("InfoTexture")).GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnSelectionChange () 
	{

//		const string selectiont = "option";
//
//		int string_num = 0;
//		string_num = m_popItmes.Count;
//		const string[] selection = new string[string_num];
//
//		for(int i = 0;i < string_num;i++)
//		{
//			selection[i] = m_popItmes[i];
//		}
		Material[] materials = m_matObj.renderer.materials;

//		print("Application.dataPath :"+ Application.dataPath);
		m_infoTexture.text = Application.dataPath;
		switch(m_popList.value)
		{

			case "Body1":
			for(int i = 0;i < materials.Length;i++)
			{
			if(materials[i].ToString().Equals("U_Character_Body-DIF (Instance) (UnityEngine.Material)"))
			{
				print(" name " + materials[i].ToString() +" index:  "+i);
//				materials[i].mainTexture = Resources.Load("Materials/U_Character_Body-DIF") as Texture2D;
					materials[i].mainTexture = Resources.Load("Materials/U_Character_Body-DIF") as Texture2D;

			}
			}
				break;

		case "Body2":
			for(int i = 0;i < materials.Length;i++)
			{
				if(materials[i].ToString().Equals("U_Character_Body-DIF (Instance) (UnityEngine.Material)"))
				{
					print(" name " + materials[i].ToString() +" index:  "+i);
					materials[i].mainTexture = Resources.Load("Materials/bmbody_color_map") as Texture2D;
				}
//				
			}
				break;
		
		case "Body3":
			for(int i = 0;i < materials.Length;i++)
			{
			if(materials[i].ToString().Equals("U_Character_Body-DIF (Instance) (UnityEngine.Material)"))
			{
				print(" name " + materials[i].ToString() +" index:  "+i);
					materials[i].mainTexture = Resources.Load("Materials/Teddy_DIF") as Texture2D;
			}
			}
				break;
		
			case "Head1":
			for(int i = 0;i < materials.Length;i++)
			{
			if(materials[i].ToString().Equals("U_Character_Head-DIF (Instance) (UnityEngine.Material)"))
			{
				print(" name " + materials[i].ToString() +" index:  "+i);
					materials[i].mainTexture = Resources.Load("Materials/U_Character_Head-DIF") as Texture2D;
			}
			}
			break;

			case "Head2":
			for(int i = 0;i < materials.Length;i++)
			{
			if(materials[i].ToString().Equals("U_Character_Head-DIF (Instance) (UnityEngine.Material)"))
			{
				print(" name " + materials[i].ToString() +" index:  "+i);
					materials[i].mainTexture = Resources.Load("Materials/face_color_map") as Texture2D;
			}
			}
			break;

		case "unLoadAll":
//			Resources.UnloadAsset(materials[1].mainTexture);
//			Resources.UnloadAsset(materials[0].mainTexture);

			GameObject TerrainLayer = GameObject.Find("TerrainLayer");
			if(TerrainLayer != null)
			{

				SpriteRenderer[] children = TerrainLayer.GetComponentsInChildren<SpriteRenderer>();

				for(int i = 0; i < children.Length;i++) GameObject.DestroyImmediate(children[i]);

//				TerrainLayer.SetActive(false);
//				GameObject.Destroy(TerrainLayer);

				print(" Resources.UnloadAsset(TerrainLayer) Not null ");
//				Resources.UnloadAsset(TerrainLayer);
			}

			break;
		case "testAddAsset":

//			StartCoroutine( LoadGameObject());
			AssetBundle assetBundle = AssetBundle.CreateFromFile("TestBuildAsset.unity3d");

			GameObject gObj = (GameObject)Instantiate(assetBundle.mainAsset);
			assetBundle.Unload(false);
				break;
		}
	}

	private IEnumerator LoadGameObject()
	{

		WWW w3w = WWW.LoadFromCacheOrDownload("/Users/xiangshouyong/Documents/u3Test/Test2.5D/TestBuildAsset.unity3d",1);

		yield return w3w;

		if(w3w.error != null)
		{
			throw new System.Exception("Download had an error: " + w3w.error);
		}

		AssetBundle assetBundle = w3w.assetBundle;
		GameObject gObj = (GameObject)Instantiate(assetBundle.mainAsset);
		assetBundle.Unload(false);
	}

	void OnDestory()
	{
		EventDelegate.Remove(m_popList.onChange,OnSelectionChange);
	}

}
