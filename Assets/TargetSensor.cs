using UnityEngine;
using System.Collections;
using System;

public class TargetSensor : MonoBehaviour {
	public string sideToSense = "RedSide";
	public float maxSensorRange = Mathf.Infinity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/*GameObject closestVisible = FindClosestVisible ();
		if (closestVisible != null) {
			//Debug.DrawLine (transform.position, closestVisible.transform.position);
		}
		*/
		/*
		GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag (sideToSense);
		foreach (GameObject possibleTarget in possibleTargets) {
			//Vector3 direction = (possibleTarget.transform.position - transform.position).normalized;
			RaycastHit hitInfo;
			if (Physics.Raycast(transform.position, possibleTarget.transform.position - transform.position, out hitInfo, maxSensorRange)) { 
				GameObject hit = hitInfo.transform.gameObject;
				if ((hit.GetComponent<UnitMember>() != null) && (hit.tag == sideToSense)) {
					Debug.DrawLine(transform.position, hit.transform.position);
				}
			}
		}
		*/

	}

	public GameObject FindClosestVisible() {
		GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag (sideToSense);
		Array.Sort (possibleTargets, delegate(GameObject x, GameObject y) {
			return (x.transform.position - transform.position).sqrMagnitude 
				 < (y.transform.position - transform.position).sqrMagnitude ? -1 : 1;
		});
		foreach (GameObject possibleTarget in possibleTargets) {
			RaycastHit hitInfo;
			if (Physics.Raycast (transform.position, possibleTarget.transform.position - transform.position, out hitInfo, maxSensorRange)) { 
				GameObject hit = hitInfo.transform.gameObject;
				if ((hit.GetComponent<UnitMember>() != null) && (hit.tag == sideToSense)) {
					return hit;
				}
			}
		}
		return null;
	}
}
