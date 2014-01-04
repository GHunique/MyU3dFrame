using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	int _connectingState = 0;

	// Use this for initialization
	void Start () 
	{
		UIButton butt = gameObject.GetComponent<UIButton>();
		EventDelegate.Add(butt.onClick,ConnectServer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ConnectServer()
	{
		Debug.Log(" ConnectServer -- ");

		if(_connectingState == 0)
		{
			_connectingState = 1;
		}else if(_connectingState == 2)
		{

		}

		UIInput input_IP = (GameObject.FindGameObjectWithTag("Input-IP")).GetComponent<UIInput>();
		UIInput input_Port = (GameObject.FindGameObjectWithTag("Input-Port")).GetComponent<UIInput>();

		Global.ConnectIP = input_IP.value;

//		if(input_Port.value.Length != 0)
			Global.ConnectPort = int.Parse( input_Port.value);
	
		Network.useNat = false;
		Network.Connect(Global.ConnectIP,Global.ConnectPort);
	}

	void OnFailedToConnect(NetworkConnectionError error) 
	{
		Debug.LogWarning("Could not connect to server: " + error);
	}

	void OnNetworkInstantiate(NetworkMessageInfo info) {
		Debug.Log("New object instantiated by " + info.sender);
	}

	void OnDestroy()
	{
		UIButton butt = gameObject.GetComponent<UIButton>();
		EventDelegate.Remove(butt.onClick,ConnectServer);
	}
}
