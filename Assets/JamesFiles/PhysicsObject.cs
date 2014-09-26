using UnityEngine;
using System.Collections;

//	PhysicsObject Class
//================================================
//================================================
/*	The PhysicsObject Class allows objects to react to a limited
 * 	physics simulation.
 * 
 * 	On Updates the physics object will have its position modified by its 
 * 	velocity and its velocity modified by its acceleration (vertical only)
 * 
 * 	While up-down movement responds to acceleration, 
 * 	side-side movement only factors velocity. 
 * 	Side-side velocity can be changed 
 * 
 * 	Provides a function to change side speed and a function to 
 * 	be called when an object lands
 * 
 */

public class PhysicsObject : MonoBehaviour {

	//public Vector3 gravity = new Vector3(0,-9.8f,0);

	public Vector3 accel = Vector3.zero;
	public Vector3 vel = Vector3.zero;
	public bool immovable;
	public bool onGround;
	public float killEasing = .5f;
	public bool ___________________;
	public bool killHorVelocity = false;
	public bool enableSpeedChange = true;

	public enum direction{
		LEFT,
		RIGHT,
	};

	public bool leftDisabled = false;
	public bool rightDisabled = false;
	direction disableDir;

	
	//	Public functions
	//=============================================

	//	Changes the x velocity of the object to speed
	//-----------------------------------------------------
	public void changeSideSpeed(float speed){
		if (!enableSpeedChange) return;
		//	Nice big special case for wall sticking
		if (leftDisabled && speed < 0) return;
		if (rightDisabled && speed > 0) return;
//		if (killHorVelocity && (Mathf.Sign(speed) == Mathf.Sign(vel.x)))
//			killHorVelocity = false;
//		if (killHorVelocity &&
//			Mathf.Sign(speed) != Mathf.Sign(vel.x) && 
//			vel.x != 0) speed = vel.x;
		Vector3 curVel = vel;
		curVel.x = speed;
		vel = curVel;
	}

	public void disableDirection(direction dir, float time){
		StartCoroutine(disableDirectionForTime(dir, time));
	}

	IEnumerator disableDirectionForTime(direction dir, float time){
		if (dir == direction.RIGHT) rightDisabled = true;
		else leftDisabled = true;
		yield return new WaitForSeconds(time);
		if (dir == direction.RIGHT) rightDisabled = false;
		else leftDisabled = false;
	}

	public void changeVertSpeed(float speed){
		Vector3 curVel = vel;
		curVel.y = speed;
		vel = curVel;
	}
	
	//	Updates state of object so it knows it is on the ground; stops vertical movement
	//------------------------------------------
	public void land(){
		onGround = true;
		if (!GetComponent<SlidingObject>())	changeSideSpeed(0);
		negateVertMovement();
	}
	
	public void negateVertAcceleration(){
		Vector3 curAccel = accel;
		curAccel.y = 0;
		accel = curAccel;
	}

	public void negateVertMovement(){
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
		SticksToWalls stw = GetComponent<SticksToWalls>();
		bool onWall = stw && stw.onWall;
		bool onShroom = GetComponent<CollisionDetector>().onShroom;
		if (onGround && !onShroom && !onWall) return;
		manageUpDownMovement();
	}
	
	void manageSideMovement(){
//		SticksToWalls stw = GetComponent<SticksToWalls>();
//		if (stw && stw.onWall) vel.x = 0;
		if (killHorVelocity) vel.x = Mathf.Lerp(vel.x, 0, killEasing); 
		if (Mathf.Abs(vel.x) < .001f) killHorVelocity = false;
		Vector3 pos = transform.position;
		pos.x += vel.x * Time.deltaTime;
		transform.position = pos;
	}
	
	void manageUpDownMovement(){
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (stw && stw.onWall) accel.y = 0;
		vel += accel * Time.deltaTime;
		Vector3 pos = transform.position;
		pos.y += vel.y * Time.deltaTime;
		transform.position = pos;
	}

}
