﻿using UnityEngine;
using UnityEditor;

public class IEAssetsBundle  {
/***Create Assetbundles Main : 分开打包，会生成两个Assetbundle*/
[MenuItem("Custom Editor/Create AssetBunldes Main")]
static void CreateAssetBunldesMain ()
{
	//获取在Project视图中选择的所有游戏对象
	Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
	
	//遍历所有的游戏对象
	foreach (Object obj in SelectedAsset) 
	{
		string sourcePath = AssetDatabase.GetAssetPath (obj);
		//本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
		//StreamingAssets是只读路径，不能写入
		//服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
		string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";
		if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies)) {
			Debug.Log(obj.name +"资源打包成功");
		} 
		else 
		{
			Debug.Log(obj.name +"资源打包失败");
		}
	}
	//刷新编辑器
	AssetDatabase.Refresh ();	
	
}

/***Create AssetBundles All：将所有对象打包在一个Assetbundle中*/
[MenuItem("Custom Editor/Create AssetBunldes ALL")]
static void CreateAssetBunldesALL ()
{
	
	Caching.CleanCache ();
	
	string Path = Application.dataPath + "/StreamingAssets/ALL.assetbundle";
	
	Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
	
	foreach (Object obj in SelectedAsset) 
	{
		Debug.Log ("Create AssetBunldes name :" + obj);
	}
	
	//这里注意第二个参数就行
	if (BuildPipeline.BuildAssetBundle (null, SelectedAsset, Path, BuildAssetBundleOptions.CollectDependencies)) {
		AssetDatabase.Refresh ();
	} else {
		
	}

	
}


	//有窗口的 导出，导入
	[MenuItem("Assets/BuildAssetBundle FromSelection - Track dependencies")]
	static void ExportResource()
	{
		string path = EditorUtility.SaveFilePanel("Save Resource","","New Resource","unity3d");
		if(path.Length != 0)
		{
			Object [] selection = Selection.GetFiltered(typeof(Object),SelectionMode.DeepAssets);
			if(BuildPipeline.BuildAssetBundle(Selection.activeObject,selection,path,BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.CompleteAssets,BuildTarget.StandaloneOSXIntel64))
			{
				Debug.Log("资源打包成功");
			}else
			{
				Debug.Log("资源打包失败");
			}

			Selection.objects = selection;
		}

	}

	[MenuItem("Assets/BuildAssetBundle FromSelection - No dependency tracking")]
	static void ExportResourceNoTrack()
	{
		string path = EditorUtility.SaveFilePanel("Save Resource","","New Resource","unity3d");
		if(path.Length != 0)
		{
			if( BuildPipeline.BuildAssetBundle(Selection.activeObject,Selection.objects,path))
			{
				Debug.Log("资源打包成功");
			}else
			{
				Debug.Log("资源打包失败");
			}
		}
	}

}