using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		print("IP :" + Network.player.ipAddress);

		UIButton butt = gameObject.GetComponent<UIButton>();

		EventDelegate.Add(butt.onClick,StartServer);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void StartServer()
	{
		Debug.Log(" StartServer  ");
		Network.useNat = false;
		Network.InitializeServer(10, Global.ConnectPort);
	}


	
	void OnDestroy()
	{
		UIButton butt = gameObject.GetComponent<UIButton>();
		EventDelegate.Remove(butt.onClick,StartServer);
	}
}