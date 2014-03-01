using UnityEngine;
using System.Collections;

public class UGGameSeting : MonoBehaviour 
{

	public bool AutoColletAssets = false;

	void OnStart()
	{

	}

	void Update()
	{
		UGBodyObjectsManager.instance().SetAutoUnloadAssets(AutoColletAssets);
	}
}
