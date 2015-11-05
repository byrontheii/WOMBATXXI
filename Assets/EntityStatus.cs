using UnityEngine;
using System.Collections;

public class EntityStatus : MonoBehaviour {
	public string platform = "Infantry";
	protected string deadTag;

	void Start() {
		deadTag = tag + "Dead";
	}

	// Call this when the entity gets hit by a weapon to determine and apply the result
	public void hit(Weapon weapon) {
		if (Random.value < weapon.getProbKill (platform)) {
			//dead
			kill ();
		}
	}

	public void kill() {
		Debug.Log(this.ToString() + " was killed.");
		//remove entity from its formation
		GetComponent<MoverManager> ().removeFromFormation ();
		//remove self from unit
		GetComponent<UnitMember> ().Unit.EntityMembers.Remove (gameObject);
		//prevent entity from getting hit by raycasts
		GetComponent<Collider> ().enabled = false;
		//make entity invisible
		GetComponent<MeshRenderer> ().enabled = false;
		//avoid raycast checks by sensors
		tag = deadTag;
		Debug.Log (this.ToString () + " object deactivated.");
		gameObject.SetActive (false);
		//Object.Destroy (gameObject);
		//Debug.Log(this.ToString() + " object destroyed.");

	}
}
