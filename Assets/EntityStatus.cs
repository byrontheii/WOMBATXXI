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
        float rnd = Random.value;
        //Debug.Log(rnd + " < " + weapon.getProbKill(platform) + " ?");
		if (rnd < weapon.getProbKill (platform)) {
            //Debug.Log("yes");
			//dead
			kill ();
		}
	}

	public void kill() {
		Debug.Log(this.ToString() + " was killed.");
		//remove entity from its formation
		GetComponent<MoverManager> ().removeFromFormation ();
		//remove self from unit
        GetComponent<UnitMember>().RemoveFromUnit();
        //prevent entity from getting hit by raycasts or objects
        GetComponent<Collider> ().enabled = false;
        GetComponent<CharacterController>().enabled = false;
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
