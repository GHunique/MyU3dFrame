using UnityEngine;
using System.Collections;

public class UGObject {

	/**
	 *		保存子类信息
	 */
	private ArrayList _childrenArr = null;
	private GameObject _object_self = null;

	public UGObject()
	{
		_childrenArr = new ArrayList();
	}

	~UGObject()
	{

		_childrenArr.Clear();
	}

	GameObject LoadResource(string path)
	{
		if(_object_self == null)
		_object_self = Resources.Load(path,typeof(GameObject)) as GameObject ;

		return _object_self;
	}

	void UnloadResource(bool removeBunld)
	{
		Resources.UnloadAsset(_object_self);
	}

}
