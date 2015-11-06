using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {
	protected long ammo;
	protected float shortRangeMax;
	protected float mediumRangeMax;
	protected float longRangeMax;

	public abstract float getProbHit(float range);

	public abstract float getProbKill(string platform);

	public abstract float getShotDelay();

	public abstract float getFlightTime(float distance);

	public void fire(GameObject target) {
		float range = (target.transform.position - transform.position).magnitude;
		float flightTime = getFlightTime (range);
		Debug.DrawLine (transform.position, target.transform.position, Color.red, flightTime);
        float rnd = Random.value;
        //Debug.Log(rnd + " < " + getProbHit(range) + " ?");
		if (rnd < getProbHit (range)) {
            //Debug.Log("yes");
			StartCoroutine (hitCoroutine (target, flightTime));
		}
	}

	private IEnumerator hitCoroutine(GameObject target, float flightTime) {
        //Debug.Log("hitCoroutine");
		yield return new WaitForSeconds (flightTime);
        //Debug.Log("flightTime complete");
		target.GetComponent<EntityStatus> ().hit (this);
	}
}
