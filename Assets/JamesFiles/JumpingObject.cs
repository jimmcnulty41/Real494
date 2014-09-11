using UnityEngine;
using System.Collections;

public class JumpingObject : MonoBehaviour {

	//	Moving objects have the ability to jump at a certain speed,
	//	to a certain height, and fall down again
	//	also have the ability to move sideways at a certain speed
	
	public float jumpVelocity = 6;
	public float fallSpeed = 30;
	public float jumpHeight = 2;
	public float jumpEasing = .2f;
	public float runSpeed = 1f;
	Vector3 jumpBottom;
	
	//	Public functions to call
	//==============================================
	public void jump(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		// No jumping in the air
		if (!po.onGround) return;
		//	Make velocity equal to the jump velocity
		po.vel += new Vector3(0,jumpVelocity,0);
		po.onGround = false;
		jumpBottom = transform.position;		
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
