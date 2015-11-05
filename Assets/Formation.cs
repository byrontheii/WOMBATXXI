using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// An instance of this class is the formation of a specific unit.
// It provides the offsets for each member of the formation.
public abstract class Formation : ScriptableObject {
	protected GameObject formationLeader;

	// Map of position to entity assigned
	protected GameObject[] entityMap;

	// Map of position to offsetPosition
	protected static int[] offsetPositionMap;

	// Map of position to [offset from offsetPosition]
	protected static Vector3[] offsetMap;

    public static Formation CreateInstance(string formationType, GameObject formationLeader)
    {
        Formation newFormation = (Formation)ScriptableObject.CreateInstance(formationType);
        newFormation.initialize(formationLeader);
        return newFormation;
    }

    protected abstract void initialize(GameObject formationLeader);

	public GameObject FormationLeader {
		get {
			return formationLeader;
		}
		set {
			formationLeader = value;
		}
	}

    public virtual void Assign(int position, GameObject entity) {
		entityMap [position] = entity;
	}

	public abstract int Assign (GameObject entity);

	// Assign entities to the 0th through (n-1)st positions, in the order appearing in the enumerable collection
	public virtual void Assign(IEnumerable<GameObject> entityEnum) {
		int i = 0;
		foreach (GameObject entity in entityEnum) {
			entityMap[i] = entity;
			i++;
		}
	}

    // This will look okay for column formations, but others might need bounding volumes for better visualization
    public virtual void DrawFormationIndicator()
    {
        for (int i=0; i<=entityMap.Length-2; i++)
        {
            if (entityMap[i] == null) continue;
			//Debug.Log("Drawing formation indicator from " + entityMap[i]);
            int j;
            for (j=i+1; j<=entityMap.Length-1; j++)
            {
				if (entityMap[j] != null)
                {
					//Debug.Log("Drawing formation indicator to " + entityMap[j]);
                    Debug.DrawLine(entityMap[i].transform.position, entityMap[j].transform.position, Color.grey);
                    i = j;
                    break;
                }
            }    
            if (i != j) {//didn't find any more formation members, so we're done drawing
                break;
            }     
        }        
    }

    // return an updated Formation with the argument entity removed, and subordinate formations adjusted as needed
	public abstract Formation Remove (GameObject entity);

	public Vector3 getTargetPosition(int spotNumber) {
		// compute and return the ideal position for the given spot in the formation,
		// relative to the position and directional facing of the offset position
		return entityMap[offsetPositionMap[spotNumber]].transform.TransformPoint (offsetMap [spotNumber]);
	}

	public Vector3 getTargetPosition(GameObject entity) {
		// formations have relatively small sizes (less than 10), so a linear search should be efficient enough
		for (int i=0; i<entityMap.Length; i++) {
			if (entityMap [i] == entity)
				return getTargetPosition (i);
		}
		throw new KeyNotFoundException ("Entity " + entity.ToString () + " not found in formation " + this.ToString ());
	}

	public override string ToString ()
	{
		System.Text.StringBuilder s = new System.Text.StringBuilder ("Formation: ");
		for (int i=0; i<entityMap.Length; i++) {
			s.Append (i + ". " + entityMap [i] + ", ");
		}
		return s.ToString();
	}

}
