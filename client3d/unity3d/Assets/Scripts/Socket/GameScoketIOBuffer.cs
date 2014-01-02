using System.Runtime;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;



[ System.Serializable ]
[ StructLayout( LayoutKind.Sequential , Pack = 1 ) ]
public class GameScoketIOBuffer
{
	int begin;
	int len;
	
	byte[] buffer;
	int maxLen;


	public GameScoketIOBuffer()
	{

	}



	public void	clearBuffer()
	{
		begin = 0;
		len = 0;
	}
	
	
	public void	initBuffer( int m )
	{
		maxLen = m;
		
		if ( buffer == null )
		{
			buffer = new byte[ m ];
		}
		
		begin = 0;
		len = 0;
	}
	
	
	public void	releaseBuffer()
	{
		begin = 0;
		len = 0;
		maxLen = 0;
		buffer = null;
	}
	
	public void	write( int l )
	{
		len += l;
	}
	
	
	public bool	read( byte[] b , int l , int offset )
	{
		if ( l > len - offset )
		{
			return false;
		}

		b.CopyTo( buffer , begin + offset );

		return true;
	}
	
	
	public void	removeBuffer( int l )
	{
		begin += l;
		len -= l;
		
		if ( len == 0 ) 
		{
			begin = 0;
		}
	}
	
	
	public void	write( byte[] b )
	{
		if ( getSpace() < b.Length )
		{
			return;
		}

		b.CopyTo( buffer , begin + len );
		
		len += b.Length;
	}
	

	public int		getLen()
	{
		return len;
	}

	public int		getOffset()
	{
		return begin;
	}
	
	public int		getSpace()
	{
		return maxLen - begin - len;
	}
	
	public byte[]	getBuffer()
	{
		return buffer;
	}

	

}

