using UnityEngine;
using System.Collections;
using System.Data;
using Excel;
using System;

public  class ExcelData : AnalyseEXML 
{
	public bool _ready = false;

	ArrayList _excelRowsArr;

	protected override  void  OnReadFile(DataSet data)
	{
		_ready = true;

		_excelRowsArr = new ArrayList();

		foreach(DataTable table in data.Tables)
		{
			int rows = 0;
			int cols  = 0;

			foreach(DataRow dRow in table.Rows)
			{
				ArrayList colsArr = new ArrayList();
				foreach(DataColumn dColumn in table.Columns)
				{
//					atrributes = atrributes +"   "+ dRow[dColumn];
					colsArr.Add(dRow[dColumn]);
					cols++;
				}

				_excelRowsArr.Add(colsArr);
				cols = 0;
				rows++;
//				Debug.Log(atrributes);
			}
		}

	}

	public string getValue(int row,int col)
	{

		if(row >= _excelRowsArr.Count) return "Out of  Row Range: " + row + "/" +( _excelRowsArr.Count -1);
		ArrayList colArr = _excelRowsArr[row] as ArrayList;
		if(col > colArr.Count) return "Out of Col Range: "+col+"/"+ (colArr.Count -1);

		string col_str = colArr[col].ToString();
		if(col_str != null)
		{
			return col_str;
		}else
		{
			return "";
		}
	}

}