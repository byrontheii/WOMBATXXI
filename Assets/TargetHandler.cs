using UnityEngine;
//using UnityEngine.Events;
using System.Collections;

// This behavior module selects a target for the attached entity, selects a weapon to engage it with, and engages.
// Wait times are enforced to represent realistic decisionmaking, aiming, etc.
public class TargetHandler : MonoBehaviour {
	GameObject target;

	// Use this for initialization
	void Start () {
		StartCoroutine(targetingCycle ());
	}

	protected IEnumerator targetingCycle() {
		// Choose the target to shoot at (or none) from the list of possible targets
		while (true) {
			yield return new WaitForSeconds (GetComponent<Weapon> ().getShotDelay ());
			chooseTarget();
			if (target != null)
				fireOnTarget ();

		}
	}

	protected void chooseTarget() {
		target = GetComponent<TargetSensor> ().FindClosestVisible ();

	}

	protected void fireOnTarget() {
		// Compute and schedule results of a shot/burst on the selected target
		GetComponent<Weapon> ().fire (target);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
