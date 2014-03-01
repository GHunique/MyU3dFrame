using UnityEngine;
using System.IO;
using System.Data;


public class UGExcelDataSource : MonoBehaviour 
{
	public BaseData[] EditorBaseData; //修改此数据结构以适应不同的需求
	ExcelData _excelData;
	public ExcelData data{get{return _excelData;}}


	[SerializeField][HideInInspector] string _fileName = "UserLevel";
	public string FileName {set{_fileName = value;} get{return _fileName;}}
	[SerializeField][HideInInspector] string _fileType = "xlsx";
	public string FileType{set{_fileType = value;} get{return _fileType;}}

	// Use this for initialization
	void Start () 
	{
		_excelData = new ExcelData();
		_excelData.OpenFile(_fileName,_fileType);
//		DisplayDataToEditor();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void DisplayDataToEditor()
	{
//		ExcelData _excelData = new ExcelData();
//		_excelData.OpenFile("UserLevel","xlsx");
		int tableCount = _excelData.result.Tables.Count;
		if(tableCount <= 0) return;
		DataTable tableData = _excelData.result.Tables[tableCount - 1];

		Debug.Log("Rows :" + tableData.Rows.Count + "  Cols :" + tableData.Columns.Count);

		int tableRows =  tableData.Rows.Count;
		int tableCols  = tableData.Columns.Count;

		EditorBaseData = new BaseData[tableRows - 1];

		for(int i = 1;i < tableRows;i++)
		{
			int j = 0;

			EditorBaseData[i-1]   = new BaseData();
			EditorBaseData[i - 1].value = new string[tableCols];
			foreach(DataColumn dCol in tableData.Columns)
			{
				EditorBaseData[i-1].value[j++] = tableData.Rows[i][dCol].ToString();
			}
		}
	}

	public  void SubmitModify()
	{
		Debug.Log("Print: " + _excelData.GetValue(12,2));

//		ExcelData tempExcelData = new ExcelData();
//		tempExcelData.OpenFile("UserLevel","xlsx");
//
//		if(File.Exists(Application.dataPath+"/UserLevel.xlsx"))
//		{
//			ExcelData.SaveExcelByOleDBC(Application.dataPath+"/UserLevel.xlsx");
//			Debug.Log(" Text Show ");
//		}
	}
}
