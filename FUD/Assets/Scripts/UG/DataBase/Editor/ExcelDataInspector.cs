using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ExcelDataSource))]
public class ExcelDataInspector : Editor 
{
	SerializedProperty _dataBaseP;
	ExcelDataSource _excelScprite;

	private string[] FileTypePopSelections = {"xlsx","txt","cfg"};
	private int  fyIndex = 0;

	void OnEnable()
	{
		_dataBaseP = serializedObject.FindProperty("EditorBaseData");
		_excelScprite = target as ExcelDataSource;
	}
	
	public override void  OnInspectorGUI()
	{
		_excelScprite.FilePath = EditorGUILayout.TextField("FilePath",_excelScprite.FilePath);

		fyIndex = EditorGUILayout.Popup("FileType",fyIndex,FileTypePopSelections);
		_excelScprite.FileType = FileTypePopSelections[fyIndex];

		GUI.color = Color.green;
		GUILayout.Space(10.0f);
		GUILayout.Label("------------------ExcelData Display------------------");
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
