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
		if(_connectingState == 0)
		{
			_connectingState = 1;
		}else if(_connectingState == 2)
		{

		}
	
		Network.useNat = false;
		Network.Connect(Global.ConnectIP,Global.ConnectPort);
	}

	void OnDestroy()
	{
		UIButton butt = gameObject.GetComponent<UIButton>();
		EventDelegate.Remove(butt.onClick,ConnectServer);
	}
}
