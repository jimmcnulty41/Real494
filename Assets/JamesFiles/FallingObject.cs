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

	public float fallSpeed = 24;
	public float setTransitionEasing = .05f;
	public bool falling = false;
	public bool ______________________;
	public float transitionEasing;
	
	void FixedUpdate(){
		//	If we're not currently falling, it's not the falling object's job
		//	to manage anything
		if (!falling) return;
		//	James McNulty 9/22/2014 11:18 PM
		//	If we can stick to walls and we are sticking to walls, 
		//	we don't do any falling
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (stw && stw.onWall){
			falling = false;
			return;
		}
		//	Otherwise, if we still have positive velocity, we transition
		if (GetComponent<PhysicsObject>().vel.y > 0) transitionIntoFall();
		//	And we fall
		fall();
	}

	//	Public functions
	//==========================================

	//	When this function is triggered, the object will fall
	public void fall(){

		//	If the object is in the air, and its velocity is negative,
		//	we're already falling. just ensure that the accel is where we want it to be.
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (!po.onGround && GetComponent<PhysicsObject>().vel.y < 0){
			GetComponent<PhysicsObject>().accel = new Vector3(0,-fallSpeed,0);
			return;
		}

		//	Otherwise, we start falling
		po.onGround = false;
		falling = true;
		GetComponent<PhysicsObject>().accel = new Vector3(0,-fallSpeed,0);
		transitionEasing = setTransitionEasing;
	}


	//	When this function is called, the object will immediately fall
	//---------------------------------------------------------------
	public void instantFall(){
		//	If the object is in the air, and its velocity is negative,
		//	we're already falling. do nothing further.
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (!po.onGround && GetComponent<PhysicsObject>().vel.y < 0) return;
		
		//	Otherwise, we start falling
		po.onGround = false;
		falling = true;
		GetComponent<PhysicsObject>().accel = new Vector3(0,-fallSpeed,0);
		transitionEasing = 1;
	}

	//	This function begins the transition into a fall from a jump using "setTransitionEasing"
	//--------------------------------------------------------------
	public void startJumpFall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.accel = new Vector3(0,-fallSpeed,0);
		falling = true;
	}

	//	Does one iteration bringing the velocity closer to 0 using the transitionEasing
	public void transitionIntoFall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		Vector3 vel = po.vel;
		vel.y = Mathf.Lerp(vel.y,0,transitionEasing);
		po.vel = vel;
	}
	
	public void land(){
		falling = false;
	}


	void Awake(){
		transitionEasing = setTransitionEasing;
	}
}
