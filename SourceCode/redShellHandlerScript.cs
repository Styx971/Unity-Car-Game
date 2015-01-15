using UnityEngine;
using System.Collections;

public class redShellHandlerScript : MonoBehaviour {
	// This script can be used with any object that is supposed to follow a
	// route marked out by waypoints. adapted from initial framework for the red shell

	
	public float vitesseAngulaire = 20.0f;
	public float vitesseDeProjection = 50.0f;
	public float tempsDeVieApresCibleAcquise = 0.75f;
	
	private WaypointCircuit circuit;         // A reference to the waypoint-based route we should follow
	
	[SerializeField] float lookAheadForTargetOffset = 5;		// The offset ahead along the route that the we will aim for
	[SerializeField] float lookAheadForTargetFactor = .1f;      // A multiplier adding distance ahead along the route to aim for, based on current speed
	[SerializeField] float lookAheadForSpeedOffset = 10;		// The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)
	[SerializeField] float lookAheadForSpeedFactor = .2f;  		// A multiplier adding distance ahead along the route for speed adjustments
	
	[SerializeField] ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute; // whether to update the position smoothly along the route (good for curved paths) or just when we reach each waypoint.
	[SerializeField] float pointToPointThreshold = 4;  // proximity to waypoint which must be reached to switch target to next waypoint : only used in PointToPoint mode.
	
	public enum ProgressStyle
	{
		SmoothAlongRoute,
		PointToPoint,
	}
	
	// these are public, readable by other objects - i.e. for an AI to know where to head!
	public WaypointCircuit.RoutePoint targetPoint { get; private set; }		
	public WaypointCircuit.RoutePoint speedPoint { get; private set; }	
	public WaypointCircuit.RoutePoint progressPoint { get; private set; }
	
	public Transform target;
	
	private float progressDistance;			// The progress round the route, used in smooth mode.
	private int progressNum;				// the current waypoint number, used in point-to-point mode.
	private Vector3 lastPosition;			// Used to calculate current speed (since we may not have a rigidbody component)
	private float speed;					// current speed of this object (calculated from delta since last frame)
	
	// setup script properties
	void Start ()
	{
		circuit = GameObject.Find("Path A").GetComponent<WaypointCircuit>();
		// we use a transform to represent the point to aim for, and the point which
		// is considered for upcoming changes-of-speed. This allows this component 
		// to communicate this information to the AI without requiring further dependencies.
		
		// You can manually create a transform and assign it to this component *and* the AI,
		// then this component will update it, and the AI can read it.
		if (target == null)
		{
			target = new GameObject(name+" Waypoint Target").transform;
		}
		
		Reset ();
		
	}
	
	// reset the object to sensible values
	public void Reset ()
	{
		progressDistance = 0;
		progressNum = 0;
		if (progressStyle == ProgressStyle.PointToPoint)
		{
			float distance = Mathf.Infinity;
			// find the closest waypoint
			for (int i=0; i<circuit.Waypoints.Length;i++)
			{
				float difference = (circuit.Waypoints[i].position - this.transform.position).sqrMagnitude;
				if (difference < distance)
				{
					// One waypoint further then the closest one 
					if ((i+1)!=circuit.Waypoints.Length)
					{
						distance = difference;
						target.position = circuit.Waypoints[ i+1 ].position;
						target.rotation = circuit.Waypoints[ i+1 ].rotation;
						progressNum=i+1;
					}
					else
					{
						distance = difference;
						target.position = circuit.Waypoints[ i ].position;
						target.rotation = circuit.Waypoints[ i ].rotation;
						progressNum=i;
					}
					
				}
			}
		}
	}
	
	
	bool checkIfCloseFromFirstPlace()
	{
		Transform firstPlace = findTarget ();
		float difference = (firstPlace.position - this.transform.position).sqrMagnitude;
		if (difference < 1000.0f) 
		{
			return true;
		}
		return false;
	}
	
	
	void Update()
	{
		if (!checkIfCloseFromFirstPlace ()) 
		{
			if (progressStyle == ProgressStyle.SmoothAlongRoute )
			{
				// determine the position we should currently be aiming for
				// (this is different to the current progress position, it is a a certain amount ahead along the route)
				// we use lerp as a simple way of smoothing out the speed over time.
				if (Time.deltaTime > 0)
				{
					speed = Mathf.Lerp (speed, (lastPosition-transform.position).magnitude / Time.deltaTime, Time.deltaTime);
				}
				target.position = circuit.GetRoutePoint( progressDistance + lookAheadForTargetOffset + lookAheadForTargetFactor * speed ).position;
				target.rotation = Quaternion.LookRotation( circuit.GetRoutePoint( progressDistance + lookAheadForSpeedOffset + lookAheadForSpeedFactor * speed ).direction );
				
				
				// get our current progress along the route
				progressPoint = circuit.GetRoutePoint( progressDistance );
				Vector3 progressDelta = progressPoint.position-transform.position;
				if (Vector3.Dot(progressDelta,progressPoint.direction) < 0) {
					progressDistance += progressDelta.magnitude * 0.5f;
				}
				
				lastPosition = transform.position;
			} else {
				// point to point mode. Just increase the waypoint if we're close enough:
				
				Vector3 targetDelta = target.position-transform.position;
				if (targetDelta.magnitude < pointToPointThreshold)
				{
					progressNum = (progressNum+1) % circuit.Waypoints.Length;
				}
				
				
				target.position = circuit.Waypoints[ progressNum ].position;
				target.rotation = circuit.Waypoints[ progressNum ].rotation;
				
				// get our current progress along the route
				progressPoint = circuit.GetRoutePoint( progressDistance );
				Vector3 progressDelta = progressPoint.position-transform.position;
				if (Vector3.Dot(progressDelta,progressPoint.direction) < 0) {
					progressDistance += progressDelta.magnitude;
				}
				lastPosition = transform.position;
			}
			
			this.rigidbody.velocity = this.transform.forward * vitesseDeProjection;
			this.rigidbody.MoveRotation(Quaternion.RotateTowards(this.transform.rotation,Quaternion.LookRotation(target.position-this.transform.position),vitesseAngulaire));
		}
		else
		{
			Transform target = findTarget();
			this.rigidbody.velocity = this.transform.forward * vitesseDeProjection;
			this.rigidbody.MoveRotation(Quaternion.RotateTowards(this.transform.rotation,Quaternion.LookRotation(target.position-this.transform.position),vitesseAngulaire));
			Destroy(this.gameObject, tempsDeVieApresCibleAcquise);
		}
	}
	
	Transform findTarget () 
	{
		GameObject[] targets_go = GameObject.FindGameObjectsWithTag("Player");
		Transform theTarget = this.transform;
		float distance = Mathf.Infinity;
		foreach (GameObject aTarget in targets_go) 
		{
			// we don't want the missible to find the player firing
			if (!(aTarget.name.Equals("Joueur 1")))
			{
				float difference = (aTarget.transform.position - this.transform.position).sqrMagnitude;
				if (difference < distance)
				{
					distance = difference;
					theTarget = aTarget.transform;
				}
			}
			
		}
		return theTarget;
	}

	void OnCollisionEnter (Collision hit)
	{
		if ((hit.transform.tag.Equals ("Player"))) 
		{
			//If we found a player on collision, send a message to the carDamagedController script.
			hit.transform.SendMessage("objectTouchedRedShell",SendMessageOptions.DontRequireReceiver);
		}
	}
}
