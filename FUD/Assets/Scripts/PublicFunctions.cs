using UnityEngine;
using System.Collections;

public class PublicFunctions : MonoBehaviour {

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
}
