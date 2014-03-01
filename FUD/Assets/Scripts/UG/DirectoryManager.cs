using UnityEngine;
using System.Collections;
using System.IO;

public class DirectoryManager : MonoBehaviour 
{

	public string fileName = "UserLevel";

	// Use this for initialization
	void Start () 
	{
		DirectoryInfo dire = Directory.CreateDirectory(Application.dataPath);

		FileInfo[]  fileIn = dire.GetFiles();

		for(int i = 0;i < fileIn.Length;i++)
		{
			string[] name  = fileIn[i].Name.Split('.');
			if(fileName == name[0])
			{
				Debug.Log(" 路径: " + fileIn[i].FullName);
				Debug.Log(" 路径 suffix : " + fileIn[i].Name);
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
