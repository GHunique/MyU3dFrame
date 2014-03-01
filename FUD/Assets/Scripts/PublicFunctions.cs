using UnityEngine;
using System.Collections;

public class PublicFunctions : MonoBehaviour 
{

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		UILabel tip = (GameObject.FindGameObjectWithTag("TipLable")).GetComponent<UILabel>();
		tip.text = Application.dataPath + "\n" +Application.streamingAssetsPath;
		
		tip.text = "Memory Size :" + Profiler.GetTotalAllocatedMemory()/1024/8;
		tip.text = tip.text +  "\nReserved Memory :" + Profiler.GetTotalReservedMemory()/1024/8;

		tip.text = " Players Num : " + Network.connections.Length;
	}

	public void ManagerReleaseAssets()
	{
		CreateBody gm = GameObject.Find("UGCreateBody").GetComponent<CreateBody>();

		GameObject.DestroyImmediate(gm);
	}

	public void DestroyPlayerButt()
	{
		Resources.UnloadUnusedAssets();

		ExcelData ed;
		UGExcelDataManager.TryGet("UserLeve",out ed);
		Debug.Log(" ExcelData: " + ed.GetValue(1,2));
	}

	public void pCreateBody()
	{

		CreateBody ugBody = UGBodyObjectsManager.instance().CreateUGBody("PlayerAssets",1);


//		Debug.Log(" 获得的UG游戏体 " +UGBodyObjectsManager.instance().TryGetUGGameObject(1));
	}


}
