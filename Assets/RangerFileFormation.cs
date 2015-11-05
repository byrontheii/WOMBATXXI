using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RangerFileFormation : Formation
{
    protected override void initialize(GameObject formationLeader)
	{
		offsetPositionMap = new int[] {0,0};
		offsetMap = new Vector3[] {new Vector3 (0, 0, 0), new Vector3 (0, 0, -5)};
		entityMap = new GameObject[offsetMap.Length];
        entityMap[0] = formationLeader;
        this.formationLeader = formationLeader;
	}

	public override int Assign(GameObject entity) {
		if (entityMap [0] == null) {
			FormationLeader = entity;
			entityMap [0] = entity;
			return 0;
		} else if (entityMap [1] == null) {
			entityMap [1] = entity;
			return 1;
		} else {
			throw (new UnityException ("Tried to Assign entity " + entity.ToString () + " to a full formation."));
		}
	}

    public override Formation Remove(GameObject entity) {
		if (entityMap[0] == entity) {
			if (entityMap[1] == null) {
                //nothing left in this formation, and therefore no junior formation to worry about
                //Object.Destroy(this);
                return null;
			}
			else { // entityMap[1] contains an entity
                // It will be more efficient (for long ranger files) to just replace the current formation with the one behind it
				Formation replacementFormation = entityMap [1].GetComponent<MoverManager>().JuniorFormation;
                //make the follower the new leader
                //Object.Destroy(this);
				return replacementFormation;
			}
		}
		else if (entityMap[1] == entity) {
            // We don't worry about subordinate formations in this case 
            entityMap[1] = null;
			return this;
		}
		// didn't find entity in formation; do nothing
		return this;
	}
}


