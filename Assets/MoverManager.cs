using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

// Controls movement of entity.
public class MoverManager : MonoBehaviour {
	// Possible values:
	//		"MOVE_TO_DESTINATION" : move towards waypoint regardless of enemy activity
	//		"MOVE_AGGRESSIVELY" : move towards best position to engage current target
	//		"MOVE_IN_FORMATION" : move towards assigned position in formation
	//	Generally, only the leader of a base unit can have one of the first two modes.
	//	All other entities move in formation.
	public string movementMode = "MOVE_TO_DESTINATION";
	protected Queue<Vector3> waypointQueue = new Queue<Vector3>();
	public float waypointTriggerProximity = 0.1f;
    protected float sqrWaypointTriggerProximity;
	public float speed = 0.0f;
	public float catchUpSpeed = 0.0f;
	private Vector3 movementVector = new Vector3 (0, 0, 0);
	public string formationType;
	public GameObject formationLeader;
	protected Formation juniorFormation; 
	protected Formation seniorFormation;
	public int formationPositionNumber;

	public string MovementMode {
		get {
			return movementMode;
		}
		set {
			movementMode = value;
		}
	}

	public Queue<Vector3> WaypointQueue {
		get {
			return waypointQueue;
		}
		set {
			waypointQueue = value;
		}
	}

	//The formation of which the GameObject is a member but not as a leader
	public Formation SeniorFormation {
		get {
			return seniorFormation;
		}
		set {
			seniorFormation = value;
		}
	}

	//The formation of which the GameObject is a member as the leader
	public Formation JuniorFormation {
		get {
			return juniorFormation;
		}
		set {
			juniorFormation = value;
		}
	}

	public bool isFormationLeader() {
        // you can only be the leader of your junior formation
		return JuniorFormation.FormationLeader == gameObject;
	}

	void Awake() {
		if (formationType != "") {
            // See if the given formation matches a defined formation class:
            //Type formationClassType = Type.GetType (formationType);
            // Get and call the constructor of the found class to create a new formation:
            //Debug.Log("Getting class for type: " + formationClassType.ToString());
            //ConstructorInfo formationConstructor = formationClassType.GetConstructor (new Type[] {typeof(GameObject)});
            //MethodInfo createInstance = typeof(ScriptableObject).GetMethod("CreateInstance<" + formationType + ">");
            //JuniorFormation = (Formation)formationConstructor.Invoke (new System.Object[] {gameObject});
            //JuniorFormation = (Formation)createInstance.Invoke(null, null);

            //Tell formation leaders to create their formations
            //  (everyone with a valid formationType is a formation leader)
            JuniorFormation = Formation.CreateInstance(formationType, gameObject);
            Debug.Log (gameObject + "Created junior formation: " + JuniorFormation.ToString ());
		}
	}

	// Use this for initialization
	void Start () {
		sqrWaypointTriggerProximity = waypointTriggerProximity * waypointTriggerProximity;
		if (formationLeader != null) {
            // I'm in a formation where I'm not the leader. It must be my senior formation.
			SeniorFormation = formationLeader.GetComponent<MoverManager> ().JuniorFormation;
			SeniorFormation.Assign (formationPositionNumber, gameObject);
			speed = formationLeader.GetComponent<MoverManager> ().speed;
			Debug.Log("Assigned self (" + gameObject + ") to senior formation: " + SeniorFormation.ToString());
		} else {
			//formation leader must be myself
		}
	}

	void Update () {
		//Debug.Log ("Update: " + gameObject);
		if (JuniorFormation != null) JuniorFormation.DrawFormationIndicator ();
		switch (movementMode) {
		case "MOVE_TO_DESTINATION":
			MoveTowardsDestination();
			Route.DrawRouteLines(waypointQueue);
			break;
		case "MOVE_AGGRESSIVELY":
			throw new NotImplementedException("Have not yet implemented MOVE_AGGRESSIVELY");
			//break;
		case "MOVE_IN_FORMATION":
			MoveInFormation();
			break;
		}
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("ControllerColliderHit");
        if (hit.collider.GetType() == typeof(CharacterController))
        {
            //Debug.Log("Hit another character controller");
            //Move in a semi-random direction to (hopefully) avoid getting stuck
            Vector3 reflection = Vector3.Reflect(hit.moveDirection, hit.normal) * catchUpSpeed * (1f + UnityEngine.Random.value);
            /*
            Vector3 xzNormal = new Vector3(hit.normal.x, 0, hit.normal.z);
            Vector3 rightOffset = Vector3.Cross(xzNormal, Vector3.up).normalized * speed;
            Vector3 randomizedRightOffset = new Vector3(rightOffset.x * (0.5f + UnityEngine.Random.value), 0f, rightOffset.z * (0.5f + UnityEngine.Random.value));
            Vector3 randomizedReverseOffset = Vector3.Rehit.moveDirection. * (0.5f + UnityEngine.Random.value);
            */
            GetComponent<CharacterController>().SimpleMove(reflection);
        }
    }

    protected void MoveTowardsDestination() {
		if (waypointQueue.Count > 0) {
			Vector3 lastWaypoint = waypointQueue.Peek ();
			if ((lastWaypoint - transform.position).sqrMagnitude < sqrWaypointTriggerProximity) {
				// close enough to waypoint to call it done
				waypointQueue.Dequeue ();
				//Debug.Log(transform.position.ToString() + " is close enough to " + lastWaypoint.ToString());
				//if (waypointQueue.Count > 0) Debug.Log("Switching to waypoint " + waypointQueue.Peek());
			}
			Vector3 velocityVector;
			if (waypointQueue.Count == 0) {
				// no waypoints left; move to final position
				velocityVector = lastWaypoint - transform.position;
			}
			else {
				velocityVector = (waypointQueue.Peek () - transform.position).normalized * speed;
			}
			velocityVector.y = 0f; // ignore the y velocity. Y will be determined by gravity.
            Debug.Log("MoveTowardsDestination with velocity " + velocityVector + ", sqrMagnitude " + velocityVector.sqrMagnitude);
			if (velocityVector.sqrMagnitude > 0f) {
				transform.rotation = Quaternion.LookRotation(velocityVector);
				GetComponent<CharacterController> ().SimpleMove (velocityVector);
			}
			//transform.position += movementVector;
		}
	}

	protected void MoveInFormation() {
        // Calculate target position based on the formation of which I'm a member
        //Debug.Log("Calling getTargetPosition(" + gameObject);
		Vector3 targetPosition = SeniorFormation.getTargetPosition(gameObject);
		// Don't even bother moving if close enough to target position
		if ((targetPosition - transform.position).sqrMagnitude > sqrWaypointTriggerProximity) {
			//for debugging:
			//transform.position = targetPosition;
			// how do we decide if we need to use catch-up speed?
			// calculate both, and use the one that gets closer!
			// there might be an ideal speed between the two--in that case, snap to ideal position
			// calculate normal speed result:
			
			Vector3 directionVector = (targetPosition - transform.position).normalized;
			Vector3 standardVelocityVector = directionVector * speed;
			standardVelocityVector.y = 0f;
			Vector3 catchUpVelocityVector = directionVector * catchUpSpeed;
			catchUpVelocityVector.y = 0f;
			Vector3 standardResult = transform.position + standardVelocityVector;
			Vector3 catchUpResult = transform.position + catchUpVelocityVector;
			float standardSqrError = (targetPosition - standardResult).sqrMagnitude;
			float catchUpSqrError = (targetPosition - catchUpResult).sqrMagnitude;
			if (standardSqrError < catchUpSqrError) {
				//transform.position = standardResult;
				if (standardVelocityVector.sqrMagnitude > 0f) {
					transform.rotation = Quaternion.LookRotation(standardVelocityVector);
					GetComponent<CharacterController>().SimpleMove(standardVelocityVector);
				}

			}
			else {
				//transform.position = catchUpResult;
				if (catchUpVelocityVector.sqrMagnitude > 0f) {
					transform.rotation = Quaternion.LookRotation(catchUpVelocityVector);
					GetComponent<CharacterController>().SimpleMove(catchUpVelocityVector);
				}

			}
		}
	}

	public void removeFromFormation() {
		//an entity can be in two formations; it can be the leader in only one of them
		//first remove it from the junior formation (the one in which it's a leader)
		if (JuniorFormation != null) {
			//Debug.Log ("Removing from junior formation");
			bool wasFormationLeader = isFormationLeader();
			JuniorFormation = JuniorFormation.Remove (gameObject); //Formation object must deal with shuffling members at its level and below
			//if removed leader of base formation, give movement orders to the new leader:
			if (JuniorFormation != null && wasFormationLeader) {
				MoverManager newLeaderMM = JuniorFormation.FormationLeader.GetComponent<MoverManager>();
				switch (MovementMode) {
				case "MOVE_TO_DESTINATION":
					newLeaderMM.MovementMode = "MOVE_TO_DESTINATION";
					newLeaderMM.waypointQueue = this.waypointQueue;
					break;
				case "MOVE_AGGRESSIVELY":
					throw new NotImplementedException("Have not yet implemented MOVE_AGGRESSIVELY");
					//break;
				}
			}

		}
		//Debug.Log ("Junior " + JuniorFormation.ToString ());
		//now remove it from the senior formation (in which it was just a member), if there is one
		if (SeniorFormation != null) {
			//Debug.Log ("Removing from senior formation");
			SeniorFormation.Remove (gameObject);
		}
		//Debug.Log ("Senior " + SeniorFormation.ToString ());
		//put the new subordinate leader (if there is one) into the senior formation (if there is one)
		if (JuniorFormation != null && SeniorFormation != null) {
			//Debug.Log ("Adding " + JuniorFormation.FormationLeader + " to senior formation");
			SeniorFormation.Assign (JuniorFormation.FormationLeader);
			MoverManager juniorLeaderMM = JuniorFormation.FormationLeader.GetComponent<MoverManager>();
			juniorLeaderMM.formationLeader = SeniorFormation.FormationLeader;
			juniorLeaderMM.SeniorFormation = SeniorFormation;
		}
		//Debug.Log ("Senior " + SeniorFormation.ToString ());
		formationLeader = null; //can't have a formation leader if you're not in a formation
        SeniorFormation = JuniorFormation = null;
	}

	// Probably need to remove this. Unity has its own MoveTowards method!
	public void MoveTowards(Vector3 destination, float speed) {
		Vector3 directionVector = (waypointQueue.Peek () - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation(directionVector);
		movementVector = directionVector * speed;
		transform.position += movementVector;

	}
}