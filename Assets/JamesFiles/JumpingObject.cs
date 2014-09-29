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
	public bool wallJumping = false;
	public float activeJumpHeight;

	void Awake(){ activeJumpHeight = jumpHeight; }
	

	//	Public functions to call
	//==============================================
	public void jump(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		// No jumping in the air
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (!po.onGround && !doubleJump && (!stw || !stw.onWall)) return;
		//	Determine the jump Velocity
		setJumpVelocity();
		//	Update information about grounding
		po.onGround = false;
		GetComponent<CollisionDetector>().onShroom = null;
		if (stw) stw.onWall = false;
		//	Set up end of jump
		jumpBottom = transform.position;	
		doubleJump = false;
	}

	void setJumpVelocity(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		SticksToWalls stw = GetComponent<SticksToWalls>();
		Vector3 jumpVelocityVec = Vector3.zero;
		bool onWall = stw && stw.onWall;
		if (onWall || wallJumping) jumpVelocityVec = jumpFromWall();
		else jumpVelocityVec += jumpFromGround();
		po.vel = jumpVelocityVec;
	}

	Vector3 jumpFromGround(){
		activeJumpHeight = jumpHeight;
		return new Vector3(0,jumpVelocity,0);
	}

	IEnumerator allowSpeedAfterDelay(){
		yield return new WaitForSeconds(delay);
		GetComponent<PhysicsObject>().enableSpeedChange = true;
	}

	// 	Changes the state of the object to reflect it is jumping off a wall
	//	and returns a Vector3 with the jump that the object will take
	Vector3 jumpFromWall(){
		wallJumping = true;
		Vector3 jumpVelocityVec = wallJumpVel;
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (stw.stickingToLeftSideOfObject)
			jumpVelocityVec.x *= -1;
		GetComponent<PhysicsObject>().disableDirection(
			stw.stickingToLeftSideOfObject? PhysicsObject.direction.RIGHT: PhysicsObject.direction.LEFT
			, delay);
		activeJumpHeight = wallJumpHeight;
		GetComponent<PhysicsObject>().killHorVelocity = true;
		return jumpVelocityVec;
	}
	
	//	During update
	//=============================
	
	void FixedUpdate(){
		if (GetComponent<PhysicsObject>().onGround) return;
		managePhysics();
	}

	int herolayer = 11;
	int immunelayer = 13;
	void managePhysics(){
		FallingObject fo = GetComponent<FallingObject>();
		if (wallJumpVelocityActive){
			GetComponent<PhysicsObject>().killHorVelocity = true;
		}
		bool onWall = GetComponent<SticksToWalls>() && GetComponent<SticksToWalls>().onWall;
		if (atJumpTop() && !onWall){
//			if (gameObject.layer == immunelayer){
//				gameObject.layer = herolayer;
//				
//			}
			wallJumping = false;
			fo.startJumpFall();
		}
		wallJumpVelocityActive = false;
	}	
	
	bool atJumpTop(){
		if (GetComponent<FallingObject>().falling) return false;
		return (transform.position.y - jumpBottom.y >= activeJumpHeight);
	}


}
