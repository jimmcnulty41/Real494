using UnityEngine;
using System.Collections;

public class HeroMovement : MonoBehaviour {

	public float jumpVelocity = 6;
	public float fallSpeed = -6;
	public float jumpHeight = 2;
	public float jumpEasing = .2f;
	public float runSpeed = 1f;
	Vector3 jumpBottom;
	bool falling = false;

	void Update(){
		manageInputs();
		managePhysics();
	}
	
	void manageInputs(){
		if (Input.GetKeyDown(KeyCode.X)) jump();
		if (Input.GetKey(KeyCode.LeftArrow)) changeLateralSpeed(-runSpeed);
		if (Input.GetKeyUp(KeyCode.LeftArrow)) changeLateralSpeed(0);
		if (Input.GetKey(KeyCode.RightArrow)) changeLateralSpeed(runSpeed);
		if (Input.GetKeyUp(KeyCode.RightArrow)) changeLateralSpeed(0);
	}
	
	void jump(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.vel += new Vector3(0,jumpVelocity,0);
		po.onGround = false;
		jumpBottom = transform.position;
	}
	
	void changeLateralSpeed(float speed){
		PhysicsObject po = GetComponent<PhysicsObject>();
		Vector3 vel = po.vel;
		vel.x = speed;
		po.vel = vel;
	}
	
	void managePhysics(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (po.onGround)
			falling = false;
		if (falling) fallingVelocity();
		if (atJumpTop()) endjump();
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
	
	void fallingVelocity(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		Vector3 vel = po.vel;
		if (vel.y < 0) return;
		vel.y = Mathf.Lerp(vel.y,0,jumpEasing);
		po.vel = vel;
	}	
	
}
