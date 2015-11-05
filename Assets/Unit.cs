using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
	private LinkedList<GameObject> entityMembers = new LinkedList<GameObject>();
	private LinkedList<Unit> subUnits = new LinkedList<Unit>();
	public Unit commandingUnit;
	public GameObject commander;
	private int echelon;
	public Camera assignedCamera;

	public ICollection<GameObject> EntityMembers {
		get {
			return (ICollection<GameObject>)entityMembers;
		}

	}

	public ICollection<Unit> SubUnits {
		get {
			return (ICollection<Unit>)subUnits;
		}

	}

	// Use this for initialization
	void Start () {
        Unit[] units = GetComponentsInChildren<Unit>(true);
        subUnits = new LinkedList<Unit>(units);
		foreach (Unit u in subUnits) {
			Debug.Log (gameObject + " unit assigned sub unit: " + u.gameObject);
        }
        UnitMember[] members = GetComponentsInChildren<UnitMember>(true);
        //automatically set the commander as the first GameObject (with UnitMember script) in the hierarchy
        if (members.Length > 0) commander = members[0].gameObject;
        foreach (UnitMember m in members)
        {
            entityMembers.AddLast(m.gameObject);
            m.Unit = this;
            Debug.Log(gameObject + " unit assigned entity: " + m.gameObject);
        }
        //Entities and units need to move individually in the scene, so now we detach them from the hierarchy:
        transform.DetachChildren();
	}
	
	// Update is called once per frame
	void Update () {
		if (EntityMembers.Count == 0) {
			// no unit members left? Nothing left to do.
			gameObject.SetActive (false);
			return;
		}

		// Move icon to the center of the unit in the xz plane
		float xsum = 0f, zsum = 0f;
		//int memberCount = 0;
		foreach (GameObject entity in EntityMembers) {
			if (!entity.activeInHierarchy) continue;
			//memberCount++;
			//Debug.Log ("Unit member: " + entity);
			xsum += entity.transform.position.x;
			zsum += entity.transform.position.z;
			//Debug.Log(entity + " at position " + entity.transform.position);
			//Debug.Log("xsum, zsum = " + xsum + ", " + zsum);
		}
		//Debug.Log ("Position before setting: " + transform.position);
		//Debug.Log ("Values to set: " + (xsum / (float)EntityMembers.Count) + ", " + (transform.position.y) + ", " + (zsum / (float)EntityMembers.Count));
        //if (memberCount > 0) {
			//transform.position = new Vector3 (xsum / (float)memberCount, transform.position.y, zsum / (float)memberCount);
		//}
		transform.position = new Vector3 (xsum / (float)EntityMembers.Count, transform.position.y, zsum / (float)EntityMembers.Count);
		//point at camera
		transform.rotation = assignedCamera.transform.rotation;
	}
}
