using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour 
{

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
		Debug.Log(" StartServer  " + " port:" + Global.ConnectPort);

//		UIInput input_Port = (GameObject.FindGameObjectWithTag("Input-Port")).GetComponent<UIInput>();
//
//		if(input_Port.value.Length != 0)
//		Global.ConnectPort = int.Parse( input_Port.value);

		Network.useNat = false;
		Network.InitializeServer(10, Global.ConnectPort);
	}

	void OnNetworkInstantiate(NetworkMessageInfo info) {
		Debug.Log("New object instantiated by " + info.sender +"  server");
	}


	
	void OnDestroy()
	{
		UIButton butt = gameObject.GetComponent<UIButton>();
		EventDelegate.Remove(butt.onClick,StartServer);
	}
}