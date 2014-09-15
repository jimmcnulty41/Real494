using UnityEngine;
using System.Collections;

//	FallingObject Class
//================================================
//================================================
/*	This class allows objects to react to a more constant version
 * 	of gravity.
 * 		-fallSpeed sets the falling speed
 * 		-transitionEasing changes how long it takes an object to transition
 * 			from upward movement to a fall. Number should be less than 1.
 * 			Lower numbers -> longer transitions, higher jumps, smoother movement
 * 			Higher numbers -> fast transition, lower jumps, more rigid movement
 * 
 * 	REQUIRES PHYSICS OBJECT
 */

public class FallingObject : MonoBehaviour {

	public float fallSpeed = 28;
	public float transitionEasing = .1f;
	public bool falling = false;
	
	void FixedUpdate(){
		if (!falling) return;
		if (GetComponent<PhysicsObject>().vel.y > 0) transitionIntoFall();
		fall();
	}

	//	Public functions
	//==========================================

	//	When this function is triggered, the object will fall
	public void fall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (!po.onGround) return;
		po.onGround = false;
		falling = true;
		GetComponent<PhysicsObject>().accel = new Vector3(0,-fallSpeed,0);
	}

	//	This function begins the transition into a fall from a jump
	public void startJumpFall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.accel = new Vector3(0,-fallSpeed,0);
		falling = true;
	}
	
	public void transitionIntoFall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		Vector3 vel = po.vel;
		vel.y = Mathf.Lerp(vel.y,0,transitionEasing);
		po.vel = vel;
	}
	
	public void land(){
		falling = false;
	}
	
}
