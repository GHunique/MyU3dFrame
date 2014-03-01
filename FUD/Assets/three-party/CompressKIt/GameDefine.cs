using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip; 
using ICSharpCode.SharpZipLib.Zip; 
using System.IO;


public class GameDefine {

	public static int INVALID_ID = -1;

	public static int MAX_COLOR = 256;

	public static int MAX_MOVEPATH = 128;

		
	public static Transform getTransform( Transform check , string name )   
	{   
		for ( int i = 0 ; i < check.childCount ; i++ )
		{
			Transform t = check.GetChild( i );
			if ( t.name == name )
			{
				return t;
			}
			
			if ( t.childCount != 0 )
			{
				Transform t1 = getTransform( t , name );
				
				if ( t1 ) 
				{
					return t1;
				}
			}
		}
		
		return null;   
	}  

	
	public static byte[] Compress( byte[] bytesToCompress ) 
	{ 
		byte[] rebyte = null; 
		MemoryStream ms = new MemoryStream(); 
		
		GZipOutputStream s = new GZipOutputStream(ms); 
		s.Write( bytesToCompress , 0 , bytesToCompress.Length ); 
		s.Close(); 
		rebyte = ms.ToArray(); 
		
		ms.Close(); 
		return rebyte;
	}
	
	public static byte[] DeCompress( byte[] bytesToDeCompress ) 
	{ 
		byte[] rebyte = new byte[ bytesToDeCompress.Length * 20 ];

		MemoryStream ms = new MemoryStream( bytesToDeCompress ); 
		MemoryStream outStream = new MemoryStream();

		GZipInputStream s = new GZipInputStream( ms ); 
		int read = s.Read( rebyte , 0 , rebyte.Length ); 
		while ( read > 0 )
		{
			outStream.Write( rebyte, 0 , read );
			read = s.Read( rebyte , 0, rebyte.Length );
		}

		s.Close(); 

		return outStream.ToArray();
	}

}
