using UnityEngine;
using System.Collections;

public class UGExitSceneDestroySingleton : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{
		Debug.Log("ExitScene : ExitScene: ");
		UGBodyObjectsManager.CloseManager();
		UGBundlePoolManager.CloseManager();
	}
}
