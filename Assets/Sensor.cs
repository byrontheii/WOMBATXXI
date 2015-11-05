using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {
    public string targetedSide;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetedSide);
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if (Physics.Raycast(transform.position, possibleTarget.transform.position - transform.position))
            {
                Debug.DrawLine(transform.position, possibleTarget.transform.position);
            }
        }
    }
}
