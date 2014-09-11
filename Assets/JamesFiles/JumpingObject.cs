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
	bool falling = false;
	
	//	Public functions to call
	//==============================================
	public void jump(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		// No jumping in the air
		if (!po.onGround) return;
		//	Make velocity equal to the jump velocity
		po.vel += new Vector3(0,jumpVelocity,0);
		po.onGround = false;
		falling = false;
		jumpBottom = transform.position;		
	}
	
	public void startFalling(){
		falling = true;
	}
	
	//	During update
	//=============================
	
	void Update(){
		managePhysics();
	}
	
	void managePhysics(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (po.onGround){
			falling = false;
			return;
		}
		if (falling) transitionToFallingVelocity();
		if (atJumpTop()) endjump();
	}	
	
	void transitionToFallingVelocity(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		Vector3 vel = po.vel;
		if (vel.y < 0) return;
		vel.y = Mathf.Lerp(vel.y,0,jumpEasing);
		po.vel = vel;
	}
	
	bool atJumpTop(){
		if (falling) return false;
		return (transform.position.y - jumpBottom.y >= jumpHeight);
	}
		
	void endjump(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.accel = new Vector3(0,-fallSpeed,0);
		falling = true;
	}

}
