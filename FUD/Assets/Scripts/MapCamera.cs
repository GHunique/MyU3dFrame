using UnityEngine;
using System.Collections;

public class MapCamera: MonoBehaviour {

	public float cameraheight = 5.0f;
	Transform follow;

	public float interval_x = 1.0f;
	public float interval_z =  1.0f;

	// Use this for initialization
	void Start () {
	
		follow = GameObject.FindWithTag ("Player").transform;

//		Quaternion q = new Quaternion();
//		q.SetLookRotation(new Vector3(1,0,0));
//		this.transform.rotation = q;
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.position = new Vector3(follow.position.x + interval_x,cameraheight,follow.position.z + interval_z);
	}
}
