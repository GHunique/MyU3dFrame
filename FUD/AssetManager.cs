using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using zthm;
// 资源管理类，挂载到每个场景的主摄像机
// 负责管理资源包
public class AssetManager : MonoBehaviour
{	
	public delegate void DownloadedCB(string name, AssetBundle bundle, Object obj, System.Object userData);
	public delegate void AllDownloadedCB(string lastAsset, System.Object userData);
	
	public class Mission
	{
		List<string> mURLs = new List<string>();
		int mCurIdx = 0;
		WWW mCurDowndloading;

		public Mission(List<string> urls)
		{
			mURLs.AddRange(urls);
		}
		
		public DownloadedCB PerloadedCB;
		public AllDownloadedCB AllLoadedCB;
		
		public System.Object mUserData = null;
		
		public void AddURL(string url)
		{
			mURLs.Add(url);
		}
		
		public void Begin()
		{
			mCurIdx = 0;
		}
		
		public void Done()
		{
			if (IsCompleted())
				return;
			++mCurIdx;
		}
		
		public bool IsCompleted()
		{
			return (mCurIdx >= mURLs.Count);
		}
		
		public string CurrentBundle
		{
			get { return mURLs[mCurIdx]; }
		}
		
		public WWW CurrentDownloading
		{
			get { return mCurDowndloading; }
			set { mCurDowndloading = value; }
		}
		
		public int Count
		{
			get { return mURLs.Count; }
		}
		
		public int Current
		{
			get { return mCurIdx; }
		}
	}

	LinkedList<Mission> mMissionList = new LinkedList<Mission>();
	
	byte[] mRemoteVerisonFileBin;
	byte[] mRemoteDependFileBin;
	
	public Mission CurrentMission
	{
		get { return mCurMission; }
	}
	Mission mCurMission;
	//Mission mCurMission;
	//public WWW Downloading;
	
	public Dictionary<string, string> RemoteFileMd5
	{
		get { return mRemoteFileMd5; }
	}
	
	public Dictionary<string, string> LocalFileMd5
	{
		get { return mLocalFileMd5; }
	}
	
	public bool ClientNeedUpdate {
		get { return mIsClientNeedUpdate; }
	}
	
	//protected bool mIsVersionFileReady = false;
	protected bool mIsClientNeedUpdate = false;
	protected Dictionary<string, AssetBundle> mCacheBundle;	// 缓存的资源包，BundleName -> AssetBundle
	protected Dictionary<string, string> mRemoteFileMd5;	// 最新的（服务器上的）资源md5集合，BundleName -> md5
	protected Dictionary<string, string> mLocalFileMd5;		// 本地的（服务器上的）资源md5集合，BundleName -> md5
	
	public string RemoteClientVersion {
		get { return mRemoteClientVersion; }
	}
	string mRemoteClientVersion;
	
	public static string GetSetupPackName(string ver)
	{
		string ext = "";
		if (Application.platform == RuntimePlatform.Android)
			ext = ".apk";    
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
        	ext = ".ida";
		else
			ext = ".exe";    
			
		return "setup_"+ver+ext;
	}
	
	public static string GetBundleName(string resName)
	{
		string bundleName = "/resources"+AssetManager.ConfirmAssetName(resName).ToLower();
		return bundleName;
	}
	
	public static string GetBundlePath(string resName)
	{
		string path = ly.AssetDependencyManager.GetPackagePath(GetBundleName(resName));
		return path;
	}
	
	public static string GetPathWithNoExtend(string path)
	{
		int idx = path.LastIndexOf(".");
		if (idx < 0)
			return path;
		string noext = path.Substring(0, idx);
		return noext;
	}
	
	public static string ConfirmAssetName(string name)
	{
		name = name.Trim();
		if (name[0] != '/')
			name = "/"+name;
		return name;
	}
	
	public static string GetLocalPath(string name)
	{
		return "file://"+GetLocalPathNoPrefix(name);
	}
	
	public static string GetLocalPathNoPrefix(string name)
	{
		name = ConfirmAssetName(name);
		
		string dir = "";
		if (Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer)
        {
			dir = Application.persistentDataPath;
        }
		else
		{
			dir = Application.dataPath;
		}
		return string.Format("{0}{1}", dir, name);
	}
	
	public static string GetHttpPath(string name)
	{
		name = ConfirmAssetName(name);
		string httpServer = "http://192.168.16.35/Package/";
		if (Application.platform == RuntimePlatform.Android)
        {
			httpServer += "Android";
        }
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			httpServer += "iOS";
		}
		else
		{
			httpServer += "Win32";
		}
		string path = httpServer+name;
		return path;
	}
	
	public static string GetLocalBundle(string name)
	{
		string path = GetLocalPath(name);
		int idx = path.LastIndexOf(".");
		string noextend = path.Substring(0, idx);
		return noextend + ".asset";
	}
	
	public static string BinToUtf8(byte[] total)
	{
		byte[] result = total;
		if (total[0] == 0xef && total[1] == 0xbb && total[2] == 0xbf)
		{
			// utf8文件的前三个字节为特殊占位符，要跳过
			result = new byte[total.Length-3];
			System.Array.Copy(total, 3, result, 0, total.Length-3);
		}
		
		string utf8string = System.Text.Encoding.UTF8.GetString(result);
		return utf8string;
	}
	
	public static void DefaultDownloadedCB(string name, AssetBundle bundle, Object obj, System.Object userData)
	{
		if (bundle != null)
			bundle.LoadAll();
	}
	
	void Awake()
	{		
		// (!ly.Game.GetInstance().IsUsePackage())
			//mIsVersionFileReady = true;
		
		if (mCacheBundle == null)	mCacheBundle = new Dictionary<string, AssetBundle>();
		if (mRemoteFileMd5 == null) mRemoteFileMd5 = new Dictionary<string, string>();
		if (mLocalFileMd5 == null)	mLocalFileMd5 = new Dictionary<string, string>();
		
		string packDir = AssetManager.GetLocalPathNoPrefix(ly.AssetDependencyManager.GetPackageDir());
		if (!System.IO.Directory.Exists(packDir))
			System.IO.Directory.CreateDirectory(packDir);
	}

	void Start ()
	{
	}

	void Update ()
	{
	}
	
	// 下载最新的文件版本信息
	public void DownloadVersionFile()
	{
		/*if (!ly.Game.GetInstance().IsUsePackage())
		{
			mIsVersionFileReady = true;
			return;
		}*/
		
		//mIsVersionFileReady = false;

		StartCoroutine(_DownloadVersionFileImpl());
	}
	
	public void UpdateLocalVersionFile()
	{
		string localVerFile = AssetManager.GetLocalPathNoPrefix(ly.AssetDependencyManager.GetPackageDir()+"/assetversion.xml");
		string dir = System.IO.Path.GetDirectoryName(localVerFile);
		if (!System.IO.Directory.Exists(dir))
			System.IO.Directory.CreateDirectory(dir);
		
		System.Xml.XmlDocument xmlVerDoc = new System.Xml.XmlDocument();
		System.Xml.XmlDeclaration xmlDecl = xmlVerDoc.CreateXmlDeclaration("1.0", "utf-8", null);
		xmlVerDoc.AppendChild(xmlDecl);	
		
		System.Xml.XmlElement xmlElem = xmlVerDoc.CreateElement("Root");
		xmlElem.SetAttribute("ClientVersion", Game.GetInstance().Version);
		xmlVerDoc.AppendChild(xmlElem);
		
		// 保存md5信息
		foreach(KeyValuePair<string, string> pair in mLocalFileMd5)
		{
			string file = pair.Key;
			string md5 = pair.Value;
			System.Xml.XmlElement elem = xmlVerDoc.CreateElement("FileVersion");
			elem.SetAttribute("file", file);
			elem.SetAttribute("md5", md5);
			System.Xml.XmlNode node = xmlVerDoc.SelectSingleNode("Root");
			node.AppendChild(elem);
		}
		
		xmlVerDoc.Save(localVerFile);
	}
	
	void UpdateLocalMD5(string file, string md5)
	{
		Dictionary<string, string> md5Set = mLocalFileMd5;
		
		file = file.ToLower();
		string nouse = "";
		if (md5Set.TryGetValue(file, out nouse))
		{
			md5Set[file] = md5;
		}
		else
		{
			md5Set.Add(file, md5);
		}
	}
	
//	public bool IsVersionFileReady()
//	{
//		return mIsVersionFileReady;
//	}
	
	IEnumerator _DownloadVersionFileImpl()
	{
		string xml = "";
		WWW downloading = null;
		
		// 解析本地存储的文件并且获取本地文件md5
		string verFile = GetLocalPathNoPrefix(ly.AssetDependencyManager.GetPackageDir()+"/assetversion.xml");
		if (System.IO.File.Exists(verFile))
		{
			verFile = GetLocalPath(ly.AssetDependencyManager.GetPackageDir()+("/assetversion.xml"));
			downloading = new WWW(verFile);
			yield return downloading;
			
			xml = BinToUtf8(downloading.bytes);
			
			ParseVersionFile(xml, mLocalFileMd5);
		}
		
		// 解析服务器存储md5版本文件
		if (!Game.INNER_INTERNET)
		{
			verFile = GetHttpPath("assetversion.xml");
			downloading = new WWW(verFile);
			yield return downloading;
			
			// 解析文件版本信息（md5）		
			xml = BinToUtf8(downloading.bytes);
			
			mRemoteClientVersion = ParseVersionFile(xml, mRemoteFileMd5);
			if (mRemoteClientVersion != Game.GetInstance().Version)
				mIsClientNeedUpdate = true;
			
			mRemoteVerisonFileBin = new byte[downloading.bytes.Length];
			System.Array.Copy(downloading.bytes, mRemoteVerisonFileBin, mRemoteVerisonFileBin.Length);
			
//			// 更新本地的版本文件
//			string localVerFile = AssetManager.GetLocalPathNoPrefix(ly.AssetDependencyManager.GetPackagePath("assetversion.xml"));
//			System.IO.FileStream stream = new System.IO.FileStream(localVerFile, System.IO.FileMode.Create);
//			stream.Write(downloading.bytes, 0, downloading.bytes.Length);
//			stream.Flush();
//			stream.Close();
		}
		
		// 更新依赖文件
		/*if (!Game.INNER_INTERNET)
			verFile = GetHttpPath("assetdepend.xml");
		else
			verFile = GetLocalPath(ly.AssetDependencyManager.GetPackageDir()+"/assetdepend.xml");
		downloading = new WWW(verFile);
		yield return downloading;
		xml = BinToUtf8(downloading.bytes);
		ly.AssetDependencyManager.GetInstance().Init(xml);
		
		// 更新本地的依赖文件
		if (!Game.INNER_INTERNET)
		{
			this.mRemoteDependFileBin = new byte[downloading.bytes.Length];
			System.Array.Copy(downloading.bytes, mRemoteDependFileBin, mRemoteDependFileBin.Length);
			
//			string localVerFile = AssetManager.GetLocalPathNoPrefix(ly.AssetDependencyManager.GetPackagePath("assetdepend.xml"));
//			System.IO.FileStream stream = new System.IO.FileStream(localVerFile, System.IO.FileMode.Create);
//			stream.Write(downloading.bytes, 0, downloading.bytes.Length);
//			stream.Flush();
//			stream.Close();
		}
		
		//mIsVersionFileReady = true;*/
	}
	
	static public string GetFileMD5(string path)
	{
		System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
		byte[] content = new byte[stream.Length];
        stream.Read(content, 0, (int)stream.Length);
        stream.Close();	
		
		return GetMD5(content);
	}
	
	static public string GetMD5(string v)
	{
		byte[] data = System.Text.Encoding.UTF8.GetBytes(v);
		return GetMD5(data);
	}
	
	static public string GetMD5(byte[] bin)
	{
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] encode = md5.ComputeHash(bin);
		
		string md5Code = System.BitConverter.ToString(encode).ToLower();
		md5Code = md5Code.Replace("-", "");
		return md5Code;
	}
	
	// 返回客户端版本
	string ParseVersionFile(string xml, Dictionary<string, string> md5Set)
	{
		string clientVersion = "";
		
		System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
		doc.LoadXml(xml);
		
		System.Xml.XmlNode node = doc.SelectSingleNode("Root");
		System.Xml.XmlElement elemNode = node as System.Xml.XmlElement;
		if (elemNode != null)
			clientVersion = elemNode.GetAttribute("ClientVersion");
			
		foreach(System.Xml.XmlNode child in node.ChildNodes)
		{
			System.Xml.XmlElement elem = child as System.Xml.XmlElement;
			if (elem != null && elem.Name.ToLower() == "fileversion")
			{
				string fileName = elem.GetAttribute("file");
				string md5 = elem.GetAttribute("md5");
				
				string v = "";
				if (!md5Set.TryGetValue(fileName, out v))
					md5Set.Add(fileName, md5);
				else
				{
					UIHelper.ShowMessage("File existed already! "+fileName);
				}
			}
		}
		
		return clientVersion;
	}
	
	public bool IsLocalNewVersion(string bundleName)
	{
		string localMd5 = "";
		string remoteMd5 = "";

		//资源服务器上没有这个文件，认为不需要更新
		if (!RemoteFileMd5.TryGetValue(bundleName, out remoteMd5))
			return true;	
		
		bool isNeedUpdate = true;
		if (LocalFileMd5.TryGetValue(bundleName, out localMd5))
		{
			if (localMd5 == remoteMd5)
				isNeedUpdate = false;
		}
		return !isNeedUpdate;
	}
	
	public AssetBundle GetCached(string bundleName)
	{
		AssetBundle result = null;
		if (mCacheBundle.TryGetValue(bundleName, out result))
			return result;
		return null;
	}
	
	public void FreeCache(string bundleName)
	{
		AssetBundle bundle = GetCached(bundleName);
		if (bundle != null)
		{
			// 仅仅清除bundle占用的内存，不释放已经实例化的对象
			bundle.Unload(false);
		}
	}
	
	public void DownloadAssetList(List<string> bundleNames, DownloadedCB cb_PerBundleLoaded, AllDownloadedCB cb_all)
	{
		DownloadAssetList(bundleNames, cb_PerBundleLoaded, cb_all, null);
	}
	
	public void DownloadAssetList(List<string> bundleNames, DownloadedCB cb_PerBundleLoaded, AllDownloadedCB cb_all, System.Object userData)
	{
		Mission mission = new Mission(bundleNames);
		mission.mUserData = userData;
		mission.PerloadedCB = cb_PerBundleLoaded;
		mission.AllLoadedCB = cb_all;
		mMissionList.AddLast(mission);
		
		if (mMissionList.Count == 1)
			StartCoroutine(_DownloadAssetImpl(mission));
	}
	
	IEnumerator _DownloadAssetImpl(Mission mission)
	{
		mCurMission = mission;
		
		string bundleName = mission.CurrentBundle.ToLower();
		DownloadedCB cb_PerBundleLoaded = mission.PerloadedCB;
		AllDownloadedCB cb_all = mission.AllLoadedCB;
		AssetBundle bundle = GetCached(bundleName);
		
		string url = "";
		if (bundle == null)
		{
			// 尝试从本地读取文件
			bool bLocalNew = IsLocalNewVersion(bundleName);
			if (bLocalNew)
				url = GetLocalPath(ly.AssetDependencyManager.GetPackagePath(bundleName));
			else
				url = GetHttpPath(ly.AssetDependencyManager.RealPath2PackagePath(bundleName));

			// 如果本地已经更新，并且没有设置回调，则直接跳过
			if (!bLocalNew || cb_PerBundleLoaded != null)
			{
				WWW downloading = new WWW(url);
				mission.CurrentDownloading = downloading;
				yield return downloading;
				
				if (downloading.error != null)
				{
					UIHelper.ShowMessage("Download error:"+downloading.error+", file:"+url+", make sure your asset is in the main bundle.");
				}
				else
				{
					Debug.Log("Bundle has been downloaded! file:"+url);
					
					// 有回调函数设置，才认为需要立即加载到内存
					if (cb_PerBundleLoaded != null)
						bundle = downloading.assetBundle;
					
					// 如果远程下载了文件，则保存到本地
					if (!bLocalNew)
					{
						string path = GetLocalPathNoPrefix(ly.AssetDependencyManager.GetPackagePath(bundleName));
						string dir = System.IO.Path.GetDirectoryName(path);
						if (!System.IO.Directory.Exists(dir))
							System.IO.Directory.CreateDirectory(dir);
						System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
				        stream.Write(downloading.bytes, 0, downloading.bytes.Length);
				        stream.Flush();
				        stream.Close();
						
						string MD5 = GetMD5(downloading.bytes);
						UpdateLocalMD5(bundleName, MD5);
						
						// 每下载一个资源文件，都更新一下本地的VersionFile，防止中途退出又要重新下载
						UpdateLocalVersionFile();
					}
					
					if (bundle != null)
						mCacheBundle.Add(bundleName, bundle);
				}
			}
		}
		else
		{
			Debug.Log("Bundle has been loaded! file:"+bundleName);
		}
		
		if (cb_PerBundleLoaded != null && bundle == null)
		{
			Debug.LogError("No bundle!"+url);
		}
			
		
		mission.Done();
		if (cb_PerBundleLoaded != null)
			cb_PerBundleLoaded(bundleName, bundle, null, mission.mUserData);
		
		if (!mission.IsCompleted())
		{
			this.StartCoroutine(_DownloadAssetImpl(mission));
		}
		else
		{
			if (cb_all != null)
				cb_all(bundleName, mission.mUserData);
			
			mMissionList.RemoveFirst();
			if (mMissionList.Count > 0)
			{
				mission = mMissionList.First.Value;
				this.StartCoroutine(_DownloadAssetImpl(mission));
			}
		}
	}

	byte[] LoadFile(string name)
	{
		if (name[0] == '/')
			name = name.Substring(1);
		name = GetPathWithNoExtend(name);
		Object obj = Resources.Load(name);
		TextAsset textObj = obj as TextAsset;
		byte[] total = textObj.bytes;
		if (total[0] == 0xef && total[1] == 0xbb && total[2] == 0xbf)
		{
		// utf8文件的前三个字节为特殊占位符，要跳过
			byte[] result = new byte[total.Length-3];
			System.Array.Copy(total, 3, result, 0, total.Length-3);
			return result;
		}
		return total;
	}
	/*先暂时屏蔽有关下载依赖文件的逻辑
	public void LoadAssetAndDepend(string asset, DownloadedCB cb_PerBundleLoaded, AllDownloadedCB cb_all)
	{
		LoadAssetAndDepend(asset, cb_PerBundleLoaded, cb_all, null);
	}

	public void LoadAssetAndDepend(string asset, DownloadedCB cb_PerBundleLoaded, AllDownloadedCB cb_all, System.Object userData)
	{
		if (ly.Game.GetInstance().IsUsePackage())
		{
			asset = ConfirmAssetName(asset);
			asset = "/resources" + asset; 
			List<string> dependList = new List<string>();
			string localPath = asset.ToLower();
			ly.AssetDependencyManager.GetInstance().GetDependList(localPath, dependList);
			
			DownloadAssetList(dependList, cb_PerBundleLoaded, cb_all, userData);
		}
		else
		{
			StartCoroutine(_WaitOneFrame(asset, cb_PerBundleLoaded, cb_all, userData));

//			string name = asset;
//			if (name[0] == '/')
//				name = name.Substring(1);
//			string nameNoExtend = GetPathWithNoExtend(name);
//			Object obj = Resources.Load(nameNoExtend);
//			if (cb_PerBundleLoaded != null)
//				cb_PerBundleLoaded(GetBundleName(asset), null, obj, userData);
//			if (cb_all != null)
//				cb_all(GetBundleName(asset), userData);
		}
	}
	
	IEnumerator _WaitOneFrame(string asset, DownloadedCB cb_PerBundleLoaded, AllDownloadedCB cb_all, System.Object userData)
	{
		yield return null;
		
		string name = asset;
		if (name[0] == '/')
			name = name.Substring(1);
		string nameNoExtend = GetPathWithNoExtend(name);
		Object obj = Resources.Load(nameNoExtend);
		if (cb_PerBundleLoaded != null)
			cb_PerBundleLoaded(GetBundleName(asset), null, obj, userData);
		if (cb_all != null)
			cb_all(GetBundleName(asset), userData);
	}*/
	//先暂时屏蔽掉加载图片资源的逻辑
	/*public void LoadAsset(string asset, DownloadedCB cb)
	{
		LoadAsset(asset, cb, null);	
	}

	public void LoadAsset(string asset, DownloadedCB cb, Object userData)
	{
		if (ly.Game.GetInstance().IsUsePackage())
		{
			List<string> allAsset = new List<string>();
			allAsset.Add("/resources"+asset);
			
			DownloadAssetList(allAsset, cb, null, userData);
		}
		else
		{
			if (asset[0] == '/')
				asset = asset.Substring(1);
			string nameNoExtend = GetPathWithNoExtend(asset);
			Object obj = Resources.Load(nameNoExtend);
			//byte[] data = LoadFile(asset);
			cb(GetBundleName(asset), null, obj, userData);
		}
	}*/
}
