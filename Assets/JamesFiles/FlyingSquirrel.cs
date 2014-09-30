using UnityEngine;
using System.Collections;

public class FlyingSquirrel : MonoBehaviour {

	public float currentSpeed;
	public float floatSpeed = 1f;
	public float fallSpeed;
	public float groundSpeed;
	public float airSpeed;
	public bool xHeld = false;
	public bool killMode = false;

	void Awake(){
		FallingObject fo = GetComponent<FallingObject>();
		fallSpeed = fo.fallSpeed;
		if (GetComponent<HeroMovement>())
			groundSpeed = GetComponent<HeroMovement>().runSpeed;
		airSpeed = 2 * groundSpeed;
	}

	void Update(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		FallingObject fo = GetComponent<FallingObject>();
		if (po.vel.y >= 0f || po.onGround){
			manageStdJumps();
		} else if (Input.GetKey(KeyCode.X) || xHeld){
			maintainFloat();
		} else {
			normalDrop();
		}
		currentSpeed = fo.fallSpeed;
		float currentMovementSpeed = GetComponent<HeroMovement>().runSpeed;
		if (currentMovementSpeed == airSpeed) killMode = true;
		else killMode = false;
	}

	void manageStdJumps(){
		FallingObject fo = GetComponent<FallingObject>();
		fo.fallSpeed = fallSpeed;
		currentSpeed = fo.fallSpeed;
		changeSpeed(groundSpeed);
		return;
	}

	void maintainFloat(){
		FallingObject fo = GetComponent<FallingObject>();
		PhysicsObject po = GetComponent<PhysicsObject>();
		//	maintain slow fall speed
		changeSpeed(airSpeed);
		Vector3 vel = po.vel;
		if (currentSpeed != floatSpeed) {
			po.negateVertMovement();
		}
		fo.fallSpeed = floatSpeed;
	}

	void normalDrop(){
		FallingObject fo = GetComponent<FallingObject>();
		changeSpeed(groundSpeed);
		//	Return to normal fall speed
		fo.fallSpeed = fallSpeed;
	}

	void changeSpeed(float speed){
		if (GetComponent<HeroMovement>())
			GetComponent<HeroMovement>().runSpeed = speed;
	}

}
