using UnityEngine;
using System.Collections;

public class UnitMember : MonoBehaviour {
    protected Unit unit;

    public Unit Unit
    {
        get
        {
            return unit;
        }

        set
        {
            unit = value;
        }
    }

    // Use this for initialization
    void Awake () {
		//Debug.Log ("Adding self (" + gameObject + ") to unit.");
		
		//Debug.Log ("Unit members: " + unit.EntityMembers);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
