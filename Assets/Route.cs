using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Route : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		if (!EditorApplication.isPlaying) {
			DrawRouteLines (BuildRouteQueue ());
		}
	}

	public static void DrawRouteLines(IEnumerable<Vector3> route) {
		IEnumerator<Vector3> wpEnum = route.GetEnumerator ();
		if (wpEnum.MoveNext () == false)
			return;
		Vector3 previousWaypoint = wpEnum.Current;
		while (wpEnum.MoveNext()) {
			Debug.DrawLine(previousWaypoint, wpEnum.Current);
			previousWaypoint = wpEnum.Current;
		}
	}

	public Queue<Vector3> BuildRouteQueue() {
		Queue<Vector3> routeQueue = new Queue<Vector3> ();
		foreach (Transform waypointTransform in transform) {
			routeQueue.Enqueue (waypointTransform.position); 
		}
		return routeQueue;
	}

}
