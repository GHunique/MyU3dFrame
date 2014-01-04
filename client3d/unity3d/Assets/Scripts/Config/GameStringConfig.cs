using UnityEngine;
using System.Collections;
using Excel;
using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;



public class GameStringConfig : GameConfig< GameStringConfig > 
{
	
	void Awake ()
	{
		if ( mInstance == null )
		{
			mInstance = this;
		}

		// init here ,
	}
	
	public Dictionary< string , string > dic = new Dictionary< string , string >();


	public string getString( string id )
	{
		if ( !dic.ContainsKey( id ) )
		{
			return "";
		}

		return dic[ id ];
	}

	public override void initConfig()
	{
		load( "UserLevel" );
	}

	public override void clearConfig()
	{
		dic.Clear();
	}

	public override void onParse( DataSet result )
	{
		int tabel = 0;
		int rows = result.Tables[ tabel ].Rows.Count;
		
		// index 0 is des for the cfg.
		for( int i = 1 ;  i < rows ; i++ )
		{
			string id = result.Tables[ tabel ].Rows[ i ][ 0 ].ToString();
			string des = result.Tables[ tabel ].Rows[ i ][ 1 ].ToString();

			dic.Add( id , des );
		}	
	}


}


