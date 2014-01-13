using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(loadingController))]
public class MyEditor : Editor
{

	void OnEnable()
	{
		loadingController loading = (loadingController)target;
		loading.mValue =  EditorGUILayout.Slider(loading.mValue,1, 100);
	}

	public override void OnInspectorGUI()
	{
		loadingController loading = (loadingController)target;

		loading.isCalculate = EditorGUILayout.Toggle("自定义进度",loading.isCalculate);

		if(loading.isCalculate){
			loading.mValue = EditorGUILayout.Slider("Progress",loading.mValue,0f,1f);

			float increased = EditorGUILayout.FloatField("PerIncrease",loading.mPerAdd);
			if(increased >= 0 && increased <= 10)
			loading.mPerAdd = increased;

			//添加触发函数
//			loading.TrigTarget = EditorGUILayout.ObjectField("Target",loading.TrigTarget,typeof(GameObject)) as GameObject;
//			loading.TriggerFunName = EditorGUILayout.TextField("FunctionName",loading.TriggerFunName);
		}

		loading.SceneName = EditorGUILayout.TextField("SceneName",loading.SceneName);

	}
}
