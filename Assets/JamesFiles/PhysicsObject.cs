using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour {

	//public Vector3 gravity = new Vector3(0,-9.8f,0);

	public Vector3 accel = Vector3.zero;
	public Vector3 vel = Vector3.zero;
	public bool immovable;
	public bool onGround;
	
	public bool ___________________;
	
	//	Public functions
	//=============================================
	public void changeSideSpeed(float speed){
		Vector3 curVel = vel;
		curVel.x = speed;
		vel = curVel;
	}
	
	public void changeX(float amt){
		Vector3 pos = transform.position;
		pos.x = amt;
		transform.position = pos;
	}
	
	public void changeY(float amt){
		Vector3 pos = transform.position;
		pos.y = amt;
		transform.position = pos;
	}
	
	//	Updates state of object so it knows it is on the ground; stops vertical movement
	public void land(){
		onGround = true;
		negateVertAcceleration();
	}
	
	void negateVertAcceleration(){
		Vector3 curAccel = accel;
		curAccel.y = 0;
		accel = curAccel;
		Vector3 curVel = vel;
		curVel.y = 0;
		vel = curVel;
	}
		
		
	//	Update
	//===========================================
	void FixedUpdate(){
		if (immovable) return;
		manageSideMovement();
		if (onGround) return;
		manageUpDownMovement();
	}
	
	void manageSideMovement(){
		Vector3 pos = transform.position;
		pos.x += vel.x * Time.deltaTime;
		transform.position = pos;
	}
	
	void manageUpDownMovement(){
		//accel += gravity * Time.deltaTime;
		vel += accel * Time.deltaTime;
		Vector3 pos = transform.position;
		pos.y += vel.y * Time.deltaTime;
		transform.position = pos;
	}

}
