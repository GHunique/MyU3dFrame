
using UnityEngine;
using System.Collections;
using System; 
using System.Net.Sockets; 
using System.Net; 
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class GameClientSocket
{
	private Socket socket;
	private bool connected = false;

	public string serverIP;
	public int serverPort;
	public int bufferSize = 4096000;
	public int timeOut = 5000;

	public GameScoketIOBuffer iBuffer;
	public GameScoketIOBuffer oBuffer;



	public GameClientSocket ()
	{
		iBuffer = new GameScoketIOBuffer();
		iBuffer.initBuffer( bufferSize );
		oBuffer = new GameScoketIOBuffer();
		oBuffer.initBuffer( bufferSize );
	}


	public bool isConnected()
	{
		return connected;
	}


	public bool reconnect()
	{
		return connect( serverIP , serverPort );
	}

	public bool connect( string ip , int port )
	{
		serverIP = ip;
		serverPort = port;

		if ( serverIP.Length == 0 ) 
		{
			Debug.LogError( "IP is invalid." );
			return false;
		}

		if ( socket == null )
		{
			socket = new Socket ( AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp );
		}

	
		socket.Blocking = true;

		IPAddress ipAddress = IPAddress.Parse( ip );
		IPEndPoint ipEndpoint = new IPEndPoint( ipAddress, port );

		try 
		{
			socket.Connect( ipEndpoint );
		}
		catch ( Exception ex ) 
		{
			Debug.LogError( ex );
			return false;
		}

		socket.Blocking = false;

		connected = socket.Connected;
//		IAsyncResult result = socket.BeginConnect( ipEndpoint , new AsyncCallback( connectCallback ) , socket );
//
//		bool success = result.AsyncWaitHandle.WaitOne( timeOut , true );
//
		if ( !connected )
		{
			Debug.LogError( "Connect timeout." );
		}
		else
		{
			Debug.Log( "Connect Host " + ip + ":" + port );
		}

		return connected;
	}


	public void	close()
	{
		if ( !connected )
		{
			return;
		}

		socket.Close();
		socket = null;
		connected = false;

		iBuffer.clearBuffer();
		oBuffer.clearBuffer();
	}


	public void	sendData()
	{
		int len = oBuffer.getLen();

		if ( len == 0 )
		{
			return;
		}

		SocketError error;
		int tmp = socket.Send( oBuffer.getBuffer() , oBuffer.getOffset() , len , SocketFlags.None , out error );
		
		if ( tmp <= 0 )
		{
			if ( error == SocketError.NoBufferSpaceAvailable || 
			    error == SocketError.Interrupted || 
			    error == SocketError.Success ||
			    error == SocketError.WouldBlock  )
			{
				return;
			}
			else
			{
				close();
				
				return;
			}
		}
		
		oBuffer.removeBuffer( tmp );
	}


	public void   recvData()
	{
		if ( iBuffer.getSpace() < 10240 )
		{
			return;
		}

		SocketError error;
		int count = socket.Receive( iBuffer.getBuffer() , iBuffer.getOffset() , iBuffer.getSpace() , SocketFlags.None , out error );

		if ( count > 0 )
		{
			iBuffer.write( count );   
		}
		else if ( count <= 0 )
		{
			if ( error == SocketError.NoBufferSpaceAvailable || 
			    error == SocketError.Interrupted || 
			    error == SocketError.Success ||
			    error == SocketError.WouldBlock )
			{
				// No data available to read (and socket is non-blocking)
				count = 0;
			}
			else
			{
				Debug.LogError( "Socket error " + error + (int)error );

				close();
				
				return;
			}
		}
	}


	public void update()
	{
		if ( !connected )
		{
			return;
		}
		
		sendData();
		recvData();
	}

	
	bool	sendMsgBlock( GameNetMessage.NetMsgHead msg )
	{
		socket.Blocking = true;

		byte[] b = structToBytes( msg );

		int count = socket.Send( b );
		
		if ( count < 0 )
		{
			return false;
		}
		
		return true;
	}


	public void sendMsg( GameNetMessage.NetMsgHead msg )
	{
		byte[] b = structToBytes( msg );
		oBuffer.write ( b );
	}

	
	public byte[] structToBytes( object structObj )
	{
		int size = Marshal.SizeOf( structObj );
		IntPtr buffer = Marshal.AllocHGlobal( size );

		try
		{
			Marshal.StructureToPtr( structObj , buffer , false );
			byte[] bytes = new byte[ size ];
			Marshal.Copy( buffer , bytes , 0 , size );
			return bytes;
		}
		finally
		{
			Marshal.FreeHGlobal( buffer );
		}
	}


	public object bytesToStruct( byte[] bytes , int index , Type strcutType )
	{
		int size = Marshal.SizeOf( strcutType );
		IntPtr buffer = Marshal.AllocHGlobal( size );

		try
		{
			Marshal.Copy( bytes , index , buffer , size );
			return  Marshal.PtrToStructure( buffer , strcutType );
		}
		finally
		{
			Marshal.FreeHGlobal( buffer );
		}
	}


	void connectCallback( IAsyncResult asyncConnect )
	{

	}
}
