using UnityEngine;
using System.Collections;

public class SnailMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Hero" || other.gameObject.tag == "Candy") {
			return;
		}

		relationToOther relation = GetComponent<CollisionDetector> ().getRelationToObject (other);
		if ((relation == relationToOther.TOLEFT)|| (relation == relationToOther.TORIGHT)) {
			if (other.gameObject.GetComponent<OpenSideObject>() != null){
				return;
			}
			Vector3 vel = GetComponent<PhysicsObject> ().vel;
			GetComponent<PhysicsObject> ().vel = vel * -1;
		}
	}
}
