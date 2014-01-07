using UnityEngine;
using System.Collections;

public  class UGAssetObjectManager 
{
	static UGAssetObjectManager _assManager = null;
	ArrayList _objectsArr;

	public static UGAssetObjectManager instance()
	{
		if(_assManager == null)
		{
			_assManager = new UGAssetObjectManager();
			_assManager.UGInit();

			Debug.Log(" UGAssetObjectManager : [instance] ");
		}else
		{
			Debug.Log(" UGAssetObjectManager : [instance] Done !! ");
		}

		return _assManager;
	}

	void UGInit()
	{
		_objectsArr = new ArrayList();
	}

	public void AddObject(UGAssetObject aObject)
	{
		if(!_objectsArr.Contains(aObject))
		{
			_objectsArr.Add(aObject);
		}
	}

	public void ReloadAllAssets(bool destroyInstantiate)
	{
		foreach(UGAssetObject assetObject in _objectsArr)
		{
			assetObject.UnloadAsset(destroyInstantiate);
		}
	}
}
