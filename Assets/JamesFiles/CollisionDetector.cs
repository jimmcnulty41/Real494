using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {

	public float forgiveness;
	
	void OnTriggerEnter(Collider other){
		
	}
	
	void OnTriggerStay(Collider other){
	
	}
	
	void determineMovement(){
		
	}
	
	void OnTriggerExit(Collider other){
		PhysicsObject po = gameObject.GetComponent<PhysicsObject>();
		
	}

	
}
