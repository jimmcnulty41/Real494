using UnityEngine;
using System.Collections;

//	JumpingObject Class
//================================================
//================================================
/*	This class allows objects to have normal "jumping" upward movement
 * 	
 * 	This class controls how quickly the object will "jump" as well as the 
 *  point in the jump where the object will begin to fall
 * 
 * 	REQUIRES PHYSICS OBJECT
 */

public class JumpingObject : MonoBehaviour {

	//	Moving objects have the ability to jump at a certain speed,
	//	to a certain height, and fall down again
	//	also have the ability to move sideways at a certain speed
	
	public float jumpVelocity = 6;
	public float jumpHeight = 1.2f;
//	public float detachAmt = .01f;
	public Vector3 wallJumpVel;
	public bool doubleJump = false;
	public bool __________________________;
	Vector3 jumpBottom;
	

	//	Public functions to call
	//==============================================
	public void jump(){
		PhysicsObject po = GetComponent<PhysicsObject>();
//		GetComponent<CollisionDetector>().currentShroomLayer = null;
		// No jumping in the air
		if (!po.onGround && !doubleJump) return;
		//	Determine the jump Velocity
		Vector3 jumpVelocityVec = Vector3.zero;
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (stw && stw.onWall) jumpVelocityVec = jumpFromWall();
		else jumpVelocityVec = new Vector3(0,jumpVelocity,0);
		po.vel = jumpVelocityVec;
		//	Update information about grounding
		po.onGround = false;
		GetComponent<CollisionDetector>().onShroom = null;
		//	Set up end of jump
		jumpBottom = transform.position;	
		doubleJump = false;
	}
	
	Vector3 jumpFromWall(){
		SticksToWalls stw = GetComponent<SticksToWalls>();
		stw.onWall = false;
		stw.stickingToLeftSideOfObject = false;
		Vector3 pos = transform.position;
//		float detAmt = detachAmt;
		Vector3 jumpVelocityVec = wallJumpVel;
		if (stw.stickingToLeftSideOfObject){
			jumpVelocityVec.x *= -1;
//			detAmt *= -1;
		}
//		pos.x += detAmt;
//		transform.position = pos;
		return jumpVelocityVec;
	}
	
	//	During update
	//=============================
	
	void FixedUpdate(){
		if (GetComponent<PhysicsObject>().onGround) return;
		managePhysics();
	}
	
	void managePhysics(){
		if (atJumpTop()) GetComponent<FallingObject>().startJumpFall();
	}	
	
	bool atJumpTop(){
		if (GetComponent<FallingObject>().falling) return false;
		return (transform.position.y - jumpBottom.y >= jumpHeight);
	}


}
