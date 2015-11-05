using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestBehavior : MonoBehaviour {

	public GameObject route;

	//void Awake () {
	//	Debug.Log ("Awake");
	//}

	// Use this for initialization
	void Start () {
		//Debug.Log ("Start");
		GetComponent<MoverManager>().WaypointQueue = route.GetComponent<Route>().BuildRouteQueue ();
		Debug.Log ("Created waypoint queue: " + GetComponent<MoverManager>().WaypointQueue.ToString ());
	}
	

}
