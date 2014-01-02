using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

	public GameObject			particle;
	protected NavMeshAgent		agent;
	protected Animator			animator;

	protected Locomotion locomotion;
	protected Object particleClone;
	int NGUILayerMask = 0;
//	public Camera ngui_camera = null;


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;

		animator = GetComponent<Animator>();

		using(locomotion = new Locomotion(animator)){locomotion.Dispose();}

		particleClone = null;

		NGUILayerMask = LayerMask.NameToLayer("Default");

//		print("NGUI layerDepth :  " + NGUILayerMask);
	}

	protected void SetDestination()
	{
		Vector3 inputPosition = Input.mousePosition ;
		// Construct a ray from the current mouse coordinates
		foreach(Touch t in Input.touches)
		{
			if(t.phase == TouchPhase.Began)
			{
				Vector2 v2 = t.position;
				inputPosition.Set(v2.x,v2.y,0);
			}
		}

//		print(" this.transform: "+transform.position.x);

		Camera ngui_camera = (GameObject.FindWithTag("NGUICamera") as GameObject).camera ;
		var ngui_ray = ngui_camera.ScreenPointToRay(inputPosition);
		if(Physics.Raycast(ngui_ray,/*out NGUI_Hit,Mathf.Infinity,*/Mathf.Infinity))
		{
//			print("NGUILayerMask  collision:!!!!!! ");
			return;
		}

		var ray = Camera.main.ScreenPointToRay(inputPosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{
			if (particleClone != null)
			{
				GameObject.Destroy(particleClone);
				particleClone = null;
			}

			// Create a particle if hit
			Quaternion q = new Quaternion();
			q.SetLookRotation(hit.normal, Vector3.forward);
//			particleClone = Instantiate(particle, hit.point, q);

			agent.destination = hit.point;
		}
	}

	protected void SetupAgentLocomotion()
	{
		if (AgentDone())
		{
			locomotion.Do(0, 0);
			if (particleClone != null)
			{
				GameObject.Destroy(particleClone);
				particleClone = null;
			}
		}
		else
		{
			float speed = agent.desiredVelocity.magnitude;

			Vector3 velocity = Quaternion.Inverse(transform.rotation) * agent.desiredVelocity;

			float angle = Mathf.Atan2(velocity.x, velocity.z) * 180.0f / 3.14159f;

			locomotion.Do(speed, angle);
		}
	}

    void OnAnimatorMove()
    {
        agent.velocity = animator.deltaPosition / Time.deltaTime;
		transform.rotation = animator.rootRotation;
    }

	protected bool AgentDone()
	{
		return !agent.pathPending && AgentStopping();
	}

	protected bool AgentStopping()
	{
		return agent.remainingDistance <= agent.stoppingDistance;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown ("Fire1")) 
			SetDestination();

		SetupAgentLocomotion();
	}

	void OnDestroy()
	{
		print(" Agent OnDestroy ");
	}
}
