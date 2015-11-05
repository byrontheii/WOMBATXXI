using UnityEngine;
using System.Collections;

public class M16A2Weapon : Weapon {
	public override float getProbHit (float range)
	{
		return 0.2f;
	}

	public override float getProbKill (string platform)
	{
		return 0.2f;
	}

	public override float getShotDelay ()
	{
		return Random.value + 0.5f;
	}

	public override float getFlightTime (float distance)
	{
		return 0.1f;
	}
}
