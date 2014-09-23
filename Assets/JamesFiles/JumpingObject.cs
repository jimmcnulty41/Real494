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
	public float wallJumpHeight = .1f;
	public Vector3 wallJumpVel;
	public bool doubleJump = false;
	public float forgiveness = .1f;
	public float delay = .1f;
	public bool __________________________;
	Vector3 jumpBottom;
	public bool wallJumpVelocityActive = false;
	float activeJumpHeight;

	void Awake(){ activeJumpHeight = jumpHeight; }
	

	//	Public functions to call
	//==============================================
	public void jump(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		// No jumping in the air
		if (!po.onGround && !doubleJump) return;
		print("A Jump");
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

	IEnumerator allowSpeedAfterDelay(){
		yield return new WaitForSeconds(delay);
		GetComponent<PhysicsObject>().enableSpeedChange = true;
	}

	Vector3 jumpFromWall(){
		SticksToWalls stw = GetComponent<SticksToWalls>();
		Vector3 jumpVelocityVec = wallJumpVel;
		float changeFactor = 1;
		if (stw.stickingToLeftSideOfObject)
			changeFactor *= -1;
		jumpVelocityVec.x *= changeFactor;
		GetComponent<PhysicsObject>().enableSpeedChange = false;
		StartCoroutine(allowSpeedAfterDelay());
//		Vector3 pos = transform.position;
//		pos.x += changeFactor * forgiveness;
//		transform.position = pos;
		wallJumpVelocityActive = true;
		activeJumpHeight = wallJumpHeight;
		stw.onWall = false;
		stw.stickingToLeftSideOfObject = false;
		return jumpVelocityVec;
	}
	
	//	During update
	//=============================
	
	void FixedUpdate(){
		if (GetComponent<PhysicsObject>().onGround) return;
		managePhysics();
	}
	
	void managePhysics(){
		FallingObject fo = GetComponent<FallingObject>();
		if (atJumpTop()){
			fo.startJumpFall();
			if (wallJumpVelocityActive){
				GetComponent<PhysicsObject>().killHorVelocity = true;
				activeJumpHeight = jumpHeight;
				wallJumpVelocityActive = false;
			}
		}
	}	
	
	bool atJumpTop(){
		if (GetComponent<FallingObject>().falling) return false;
		return (transform.position.y - jumpBottom.y >= activeJumpHeight);
	}


}
