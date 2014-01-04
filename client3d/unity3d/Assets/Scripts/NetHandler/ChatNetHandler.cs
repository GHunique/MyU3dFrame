using UnityEngine;
using System.Collections;
using Excel;
using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;



public class ChatNetHandler : Singleton< ChatNetHandler > 
{
	
	void Awake ()
	{
		if ( mInstance == null )
		{
			mInstance = this;
			initHandler();
		}
		
	}
	
	public void initHandler()
	{
//		GameSocketManager.instance.regeditMsg( (int)GameNetMessage.MsgType.GMT_HEART , recvMsgHeart , typeof( GameNetMessage.NetMsgHeart ) );
//		
//		GameSocketManager.instance.connect( "192.168.16.169" , 8001 );
//		
//		GameNetMessage.NetMsgHeart msg = new GameNetMessage.NetMsgHeart();
//		//		msg.n[0] = 123;
//		//		msg.n[1] = 456;
//		
//		//msg.name = "sdfdfdsfd";
//		
//		GameSocketManager.instance.sendMsg( msg );
	}
	
	public void recvMsgHeart( GameNetMessage.NetMsgHead head )
	{
		//GameNetMessage.NetMsgHeart heart = ( GameNetMessage.NetMsgHeart )head;
		
		//Debug.Log( "recv msg " +  heart.name ); 
	}
	
	
}


