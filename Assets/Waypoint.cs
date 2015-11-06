using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Collections;

public class Waypoint : MonoBehaviour, IPointerUpHandler {

    public void Start()
    {
        SnapToGroundLevel();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!EditorApplication.isPlaying)
        {
            SnapToGroundLevel();
        }
        
    }
    public void SnapToGroundLevel()
    {
        
            //snap waypoint to ground elevation 
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            transform.position = pos;
        
    }
}
