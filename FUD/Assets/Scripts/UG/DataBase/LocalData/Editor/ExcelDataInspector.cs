using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UGExcelDataSource))]
public class ExcelDataInspector : Editor 
{
	SerializedProperty _dataBaseP;
	UGExcelDataSource _excelScprite;

	private string[] FileTypePopSelections = {"xlsx","txt","cfg"};
	private int  fyIndex = 0;

	void OnEnable()
	{
		_dataBaseP = serializedObject.FindProperty("EditorBaseData");
		_excelScprite = target as UGExcelDataSource;
	}
	
	public override void  OnInspectorGUI()
	{
		_excelScprite.FileName = EditorGUILayout.TextField("FileName",_excelScprite.FileName);

		fyIndex = EditorGUILayout.Popup("FileType",fyIndex,FileTypePopSelections);
		_excelScprite.FileType = FileTypePopSelections[fyIndex];

		GUI.color = Color.green;
		GUILayout.Space(10.0f);
		GUILayout.Label("通过 UGExcelDataManager.TryGet() 可以获得本地数据");
		GUI.color = Color.white;
		EditorGUILayout.PropertyField(_dataBaseP,true);

		GUILayoutOption[] options = {GUILayout.Width(100),GUILayout.Height(20),GUILayout.ExpandHeight(false)};
		GUILayout.BeginHorizontal();

		GUI.color = Color.yellow;
		GUILayout.Label("Click to xxxx");
		GUI.color = Color.white;

		if(GUILayout.Button("Not used",options)) SubmitModify();

		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();

	}

	void SubmitModify()
	{
		_excelScprite.SubmitModify();
	}

}
