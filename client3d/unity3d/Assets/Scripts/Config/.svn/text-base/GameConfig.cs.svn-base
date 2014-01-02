using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;


#if UNITY_EDITOR

[ Serializable ]
public class DebugGameConfigData
{
	public string[] values;
}
#endif

public abstract class GameConfig< T > : Singleton< T >
{

	public void load( string path )
	{
		clearConfig();

		FileStream stream = File.Open( Application.dataPath + GameSetting.ConfigPath + path + ".cfg" , FileMode.Open , FileAccess.Read );

		if ( stream == null )
		{
			return;
		}


		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader( stream );
		
		DataSet result = excelReader.AsDataSet();

		onParse( result );

#if UNITY_EDITOR
		int tabel = 0;
		int rows = result.Tables[ tabel ].Rows.Count;
		int columns = result.Tables[ tabel ].Columns.Count;

		debugConfigData = new DebugGameConfigData[ rows ];

		for( int i = 0 ;  i < rows ; i++ )
		{
			debugConfigData[ i ] = new DebugGameConfigData();
			debugConfigData[ i ].values = new string[ columns ];

			for ( int j = 0 ; j < columns ; j++ )
			{
				debugConfigData[ i ].values[ j ] = result.Tables[ tabel ].Rows[ i ][ j ].ToString();
			}

		}	
#endif
	}

	#if UNITY_EDITOR

	
	public DebugGameConfigData[] debugConfigData;
	


	#endif

	abstract public void initConfig();
	abstract public void clearConfig();
	abstract public void onParse( DataSet result );
}


