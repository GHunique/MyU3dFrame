using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ly
{
	public class AssetDependencyManager
	{
		static public AssetDependencyManager GetInstance()
		{
			if (msInstance == null)
				msInstance = new AssetDependencyManager();
			return msInstance;
		}
		static protected AssetDependencyManager msInstance;
		
		static public string GetPackageDir()
		{
			string dir = "";
			if (Application.platform == RuntimePlatform.Android)
				dir = "/Package/Android";
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
				dir = "/Package/iOS";
			else
				dir = "/Package/Win32";
			return dir;
		}
		
		static public string RealPath2PackagePath(string path)
		{
			// 去掉asset目录
			string fileOri = path;
			// 后缀换成.asset
			//string ext = System.IO.Path.GetExtension(fileOri);
			int idx = fileOri.LastIndexOf(".");
			if(idx < 0)
				idx = fileOri.Length;
			string noextend = fileOri.Substring(0, idx);
			string file = noextend + ".asset";
			return file;
		}
		
		// 得到打包后资源的相对路径
		static public string GetPackagePath(string path)
		{
			path = AssetManager.ConfirmAssetName(path);
			string packagePath = GetPackageDir()+path;
			// 去掉asset目录
			return RealPath2PackagePath(packagePath);
			
			/*
			string fileName = System.IO.Path.GetFileName(file);
			
			if (ext.ToLower() == ".mat")
			{
				packagePath += "/Materials/"+fileName;
			}
			else if (ext.ToLower() == ".shader")
			{
				packagePath += "/Shaders/"+fileName;
			}
			else if (ext.ToLower() == ".fbx")
			{
				packagePath += "/Models/"+fileName;
			}
			else if (ext.ToLower() == ".anim")
			{
				packagePath += "/Animations/"+fileName;
			}
			else if (ext.ToLower() == ".mp3")
			{
				packagePath += "/Music/"+fileName;
			}
			else if (ext.ToLower() == ".prefab")
			{
				packagePath += "/Prefabs/"+fileName;
			}
			else if (ext.ToLower() == ".unity")
			{
				packagePath += "/Maps/"+fileName;
			}
			else if (ext.ToLower() == ".bytes" || ext.ToLower() == ".xml" || ext.ToLower() == ".csv")
			{
				packagePath += "/"+fileName;
			}
			else
			{
				packagePath += "/Textures/"+fileName;
			}
			return packagePath;
			*/
		}

		//文件依赖
		public class FileDepend
		{
			public string mFile;
			public List<string> mDependFiles = new List<string>();
		}
		//存储依赖文件
		public Dictionary<string, FileDepend> mFileDependencies;
		
		public AssetDependencyManager ()
		{
			mFileDependencies = new Dictionary<string, FileDepend>();
		}
		
		public void Init(string xml)
		{
			//if (!Game.GetInstance().IsUsePackage())
				//return;
			
			// 读取xml文件
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);   
			
			XmlNode node = xmlDoc.SelectSingleNode("Root");
			if (node != null)
			{	
				foreach (XmlNode child in node)
				{
					XmlElement elem = child as XmlElement;
					if (elem != null)
					{
						if (elem.Name.ToLower() == "file")
						{
							FileDepend f = new FileDepend();
							AddChildren(child, f);
							mFileDependencies.Add(f.mFile, f);
						}
					}
				}
			}
		}
		
		void AddChildren(XmlNode parentNode, FileDepend fd)
		{
			XmlElement parent = parentNode as XmlElement;
			if (parent == null)
				return;
			
			fd.mFile = parent.GetAttribute("path");
			foreach (XmlNode child in parentNode.ChildNodes)
			{
				XmlElement elem = child as XmlElement;
				if (elem != null)
				{
					if (elem.Name.ToLower() == "file")
					{
						string file = elem.GetAttribute("path");
						//string packagePath = GetPackagePath(file);

						fd.mDependFiles.Add(file);
					}
				}
			}
		}
		
		void GetChildren(List<string> files, List<string> children, string[] filters)
		{
			// 广度优先遍历，保证读取包的顺序正确
			List<string> cache = new List<string>();
			foreach (string f in files)
			{
				FileDepend depend = null;
				if (this.mFileDependencies.TryGetValue(f, out depend))
				{
					cache.AddRange(depend.mDependFiles);
					
					/*
					foreach(string d in depend.mDependFiles)
					{
						// 判断文件后缀
						bool match = false;
						string ext = System.IO.Path.GetExtension(d).ToLower();
						foreach(string filter in filters)
						{
							if (filter.ToLower() == ext)
							{
								match = true;
								break;
							}
						}
						if (match)
							cache.Add(d);
					}
					*/
				}
			}
			
			if (cache.Count > 0)
			{
				GetChildren(cache, children, filters);
				
				foreach (string c in cache)
				{
					// 判断文件后缀
					bool match = false;
					if (filters == null || filters.Length == 0)
					{
						match = true;
					}
					else
					{
						string ext = System.IO.Path.GetExtension(c).ToLower();
						foreach(string filter in filters)
						{
							if (filter.ToLower() == ext)
							{
								match = true;
								break;
							}
						}
					}
					if (match)
					{
//						string packagePath = GetPackagePath(c);
//						children.Add(packagePath);
						children.Add(c);
					}
				}
			}
		}
		
		public void GetDependTextures(string file, List<string> dependList)
		{
			FileDepend depend = null;
			if (this.mFileDependencies.TryGetValue(file, out depend))
			{
				foreach(string d in depend.mDependFiles)
				{
					string ext = System.IO.Path.GetExtension(d).ToLower();
					if (ext == ".jpg" ||
						ext == ".tga" ||
						ext == ".bmp" ||
						ext == ".png" ||
						ext == ".dds")
					{
						string packagePath = GetPackagePath(d);
						dependList.Add(packagePath);
					}
					else
					{
						GetDependTextures(d, dependList);
					}
				}
			}
		}
		
		public void GetDependShaders(string file, List<string> dependList)
		{
			FileDepend depend = null;
			if (this.mFileDependencies.TryGetValue(file, out depend))
			{
				foreach(string d in depend.mDependFiles)
				{
					string ext = System.IO.Path.GetExtension(d).ToLower();
					if (ext == ".shader")
					{
						string packagePath = GetPackagePath(d);
						dependList.Add(packagePath);
					}
					else
					{
						GetDependShaders(d, dependList);
					}
				}
			}
		}
		
		public void GetDependMaterials(string file, List<string> dependList)
		{
			FileDepend depend = null;
			if (this.mFileDependencies.TryGetValue(file, out depend))
			{
				foreach(string d in depend.mDependFiles)
				{
					string ext = System.IO.Path.GetExtension(d).ToLower();
					if (ext == ".mat")
					{
						string packagePath = GetPackagePath(d);
						dependList.Add(packagePath);
					}
					else
					{
						GetDependMaterials(d, dependList);
					}
				}
			}
		}
		
		public void GetDependModels(string file, List<string> dependList)
		{
			FileDepend depend = null;
			if (this.mFileDependencies.TryGetValue(file, out depend))
			{
				foreach(string d in depend.mDependFiles)
				{
					string ext = System.IO.Path.GetExtension(d).ToLower();
					if (ext == ".fbx")
					{
						string packagePath = GetPackagePath(d);
						dependList.Add(packagePath);
					}
					else
					{
						GetDependModels(d, dependList);
					}
				}
			}
		}
		
		public void GetDependPrefabs(string file, List<string> dependList)
		{
			FileDepend depend = null;
			if (this.mFileDependencies.TryGetValue(file, out depend))
			{
				foreach(string d in depend.mDependFiles)
				{
					string ext = System.IO.Path.GetExtension(d).ToLower();
					if (ext == ".prefab")
					{
						string packagePath = GetPackagePath(d);
						dependList.Add(packagePath);
					}
					else
					{
						GetDependPrefabs(d, dependList);
					}
				}
			}
		}
		
		public void GetDependList(string file, List<string> dependList)
		{
			List<string> files = new List<string>();
			files.Add(file);
			GetChildren(files, dependList, null);
			
			/*
			List<string> files = new List<string>();
			files.Add(file);
			
			string[] texExt = {".tga",".png",".jpg",".bmp","dds"};
			GetChildren(files, dependList, texExt);
			
			string[] shaderExt = {".shader"};
			GetChildren(files, dependList, shaderExt);
			
			string[] matExt = {".mat"};
			GetChildren(files, dependList, matExt);
			
			string[] modelExt = {".fbx"};
			GetChildren(files, dependList, modelExt);
			
			string[] preExt = {".prefab"};
			GetChildren(files, dependList, preExt);
			*/
			
			/*
			GetDependTextures(file, dependList);
			GetDependShaders(file, dependList);
			GetDependMaterials(file, dependList);
			GetDependModels(file, dependList);
			GetDependPrefabs(file, dependList);
			*/
			
//			string self = GetPackagePath(file);
//			dependList.Add(self);
			dependList.Add(file);
		}
	}
}

