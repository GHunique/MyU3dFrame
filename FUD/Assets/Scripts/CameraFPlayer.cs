using UnityEngine;
using System.Collections;

public class CameraFPlayer : MonoBehaviour {

	public float distanceAway;			// distance from the back of the craft
	public float distanceUp;			// distance above the craft
	public float smooth;				// how smooth the camera movement is
	
	private GameObject hovercraft;		// to store the hovercraft
	private Vector3 targetPosition;		// the position the camera is trying to be in
	public float cameraheight = 5.0f;
	Transform follow;

	public float interval_x = 1.01f;
	public float interval_z =  -3.57f;

	void Start(){
		follow = GameObject.FindWithTag ("Player").transform;	
	}

	void Update()
	{

		this.transform.position = new Vector3(follow.position.x + interval_x,cameraheight,follow.position.z + interval_z);
	}
	
	void LateUpdate ()
	{
		return;
		// setting the target position to be the correct offset from the hovercraft
		targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		
		// making a smooth transition between it's current position and the position it wants to be in
		transform.position = targetPosition;
//		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
		
		// make sure the camera is looking the right way!
//		transform.LookAt(follow);
	}
}
