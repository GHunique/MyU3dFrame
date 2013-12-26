using UnityEngine;
using System.Collections;

public class loadingController : MonoBehaviour 
 {
	// Use this for initialization

	UISlider m_slider = null;
	float m_interval = 0.02f;
	float m_totalTime = 0.0f;

	[HideInInspector][SerializeField] GameObject triggerTarget = null;
	[HideInInspector][SerializeField]  string triggerFunctionName = "nextLevel";
	public GameObject TrigTarget{get {return triggerTarget;} set{triggerTarget = value;}}
	public string TriggerFunName{get {return triggerFunctionName;} set{triggerFunctionName = value;}}

	[HideInInspector][SerializeField] float perAdd_progress = 8f;
	public float mPerAdd{get{return perAdd_progress;} set{ perAdd_progress = value;}	}

	[HideInInspector][SerializeField] float m_progress = 0.2f;
	public float mValue{get{return m_progress;} set{m_progress = value;}}

	AsyncOperation MyAsync = null;

	void Start ()
	{
		m_slider = this.GetComponent<UISlider>();
		m_slider.value = 0;

		StartCoroutine(loadScene());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//定时增长进度
//		this.setProgress(m_progress);

		//根据读取场景进度设置进度条
		calculateProgress();

	}



	public void setTriger(GameObject taget,string functionName)
	{
		if (string.IsNullOrEmpty(functionName)) return;
		taget.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
	}

	void nextLevel()
	{
		print(" loaded scene : "+ Global.nextScene + "preScene: " +Global.preScene );

//		Application.LoadLevel("test2.5d");
//		GameObject loadingLayer = GameObject.FindWithTag("LoadingLayer");
//		loadingLayer.SetActive(false);
//		Object.Destroy(loadingLayer);
//
//		GameObject TerrainLayer = GameObject.Find("TerrainLayer");
//		Object.Destroy(TerrainLayer,1f);

	}

	IEnumerator loadScene()
	{

		MyAsync = Application.LoadLevelAsync(Global.nextScene);

		yield return MyAsync;
	}

	//定时增长进度
	void setProgress(float pro)
	{
		if(pro > 1) 
		{
			pro = 1;
		}
		
		m_totalTime += Time.deltaTime;
		
		if(m_slider.value < pro &&m_totalTime > m_interval)
		{
			m_slider.value = perAdd_progress/1000 + m_slider.value;
			m_totalTime = 0.0f;
		}else if(m_slider.value >= 1)
		{
			this.setTriger(triggerTarget,triggerFunctionName);
		}
	}

	//根据读取场景进度设置进度条
	void calculateProgress()
	{
		//在这里计算读取的进度，
		float progress = MyAsync.progress;
		progress = progress + 0.1f;
		if(progress <= 1)
		{
			m_slider.value = progress;
		}else
		{
			m_slider.value = 1f;
		}
	}

}






