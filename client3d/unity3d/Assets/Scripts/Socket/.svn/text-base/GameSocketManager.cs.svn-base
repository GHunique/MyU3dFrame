using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameSocketManager : Singleton< GameSocketManager >
{
	public delegate void msgHandler( GameNetMessage.NetMsgHead head );

	public struct MsgHandler
	{
		public Type type;
		public msgHandler handler;
	}

	private GameClientSocket socket = new GameClientSocket();
	private Dictionary< int , MsgHandler > handlerDic = new Dictionary< int , MsgHandler >();

	private bool isConnected = false;

	void Awake ()
	{
		if ( mInstance == null )
		{
			mInstance = this;
			DontDestroyOnLoad( gameObject );
		}
		else
		{
			Destroy( gameObject );
		}
		
	}

	public void regeditMsg( int t , msgHandler handler , Type type )
	{
		MsgHandler h = new MsgHandler();
		h.type = type;
		h.handler = handler;

		handlerDic[ t ] = h;
	}

	public bool connect( string ip , int port )
	{
		if ( socket.isConnected() )
		{
			return true;
		}

		isConnected = socket.connect( ip , port );
		return isConnected;
	}

	public void sendMsg( GameNetMessage.NetMsgHead msg )
	{
		socket.sendMsg( msg );
	}

	public void close()
	{
		socket.close();
	}


	void Update()
	{
		socket.update();

		if ( isConnected && !socket.isConnected() ) 
		{
			// reconnect,,

			isConnected = socket.reconnect();

			if ( !isConnected )
			{
				isConnected = true;
			}
			else
			{
				// connected,,

			}

			return;
		}

		if ( socket.iBuffer.getLen() > 4 ) 
		{
			byte[] ibuffer = socket.iBuffer.getBuffer();
			int offset = socket.iBuffer.getOffset();

			GameNetMessage.NetMsgHead head = ( GameNetMessage.NetMsgHead ) socket.bytesToStruct( ibuffer , offset , typeof( GameNetMessage.NetMsgHead ) ); 
		
			bool b = handlerDic.ContainsKey( head.type );

			if ( b )
			{
				MsgHandler handler = handlerDic[ head.type ];

				GameNetMessage.NetMsgHead msg = ( GameNetMessage.NetMsgHead ) socket.bytesToStruct( ibuffer , offset , handler.type ); 

				handler.handler( msg );

				Debug.Log( "recv net msg " + msg.type + " class " + handler.type ); 
			}
			else
			{
				Debug.LogError( "msg not regedit " + head.type ); 
			}

			socket.iBuffer.removeBuffer( head.size + 2 );
		}
	}


}
