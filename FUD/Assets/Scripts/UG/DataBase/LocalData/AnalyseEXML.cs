
using System.IO;
using UnityEngine;
using System.Data;
using Excel;
using System.Data.Odbc;
using System.Data.OleDb;
using Mono.Data.Sqlite;
using Mono.Data;
using System;


public abstract class AnalyseEXML 
{
	FileStream stream;
	public DataSet result;

	private string filePath;

	static  OleDbConnection m_conn = null; 
	static OleDbCommand m_cmd = null;

	public bool OpenFile(string path,string fileType)
	{
		filePath = Application.dataPath + Global.UGLocalData + path + "."+fileType;

		try{
			stream = File.Open(filePath,FileMode.Open,FileAccess.Read);
		}catch(System.Exception e)
		{
			Debug.Log("AnalyseEXML :" +e );
			NGUIDebug.Log("AnalyseEXML :"+e);
		}

		if (fileType == "xls")
		{
			IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
			result = excelReader.AsDataSet();
			excelReader.Close();

			OnReadFile(result);
			return true;
		}
		else if (fileType == "xlsx")
		{
			IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
			result = excelReader.AsDataSet();
			excelReader.Close();

			OnReadFile(result);
			return true;
		}else if(fileType == "xml" || fileType == "txt")
		{
			IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
			result = excelReader.AsDataSet();
			excelReader.Close();
			
			OnReadFile(result);
			return true;
		}

		return false;
	}

	/**
	 * Write Data to Excel by Ole DB
	*/
	public static void SaveExcelByOleDBC(string path)
	{
		m_conn = new OleDbConnection();
		m_cmd = new OleDbCommand();

		if(m_conn != null && m_cmd != null)
		{
			OpenConnection(path,false);
			InsertToExcel();
			CloseConnection();
		}

	}

	static void OpenConnection(string excelFileName,bool addIMEX)
	{
		if(m_conn.State == ConnectionState.Closed)
		{
			string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;"+
				"Datasource=" +excelFileName +
					";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\"";

			if(excelFileName.ToLower().IndexOf(".xlsx") > -1)
			{
				strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+excelFileName;

				if(addIMEX)
				{
					strCon += ";Extended Properties=\"Excel12.0;HDR=YES;HDR=NO;IMEX=1\"";
				}else
				{
					strCon += ";Extended Properties=\"Excel12.0;HDR=YES\"";
				}
			}

			m_conn.ConnectionString = strCon;
			m_cmd.Connection = m_conn;

			try
			{
				Debug.Log(strCon);
				m_conn.Open();

			}catch(Exception e)
			{
				throw e;
			}
		}
	}

	static void  CloseConnection()
	{
		if(m_conn.State == ConnectionState.Open)
		{
			m_conn.Close();
			m_conn.Dispose();
			m_cmd.Dispose();
		}
	}

	static void  InsertToExcel()
	{

		string sqlStr = "INSERT INTO [Sheet1$](colname1,colname2,colname3) VALUES (value1,value2,value3);";//需要注意的是SQL中，colname是不能存在空格的。

		m_cmd.CommandType = CommandType.Text;
		m_cmd.CommandText = sqlStr;
		m_cmd.ExecuteNonQuery();
	}

	// Write Data to Excel by Ole DB   ---end

	/**
	 * 	 Excel.Application的Cell by Cell
	 * 
	 */

	void SaveExcelByCell(string path)
	{

	}

	protected abstract void OnReadFile(DataSet data);

}


 
