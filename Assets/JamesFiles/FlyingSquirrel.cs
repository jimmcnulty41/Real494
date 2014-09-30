using UnityEngine;
using System.Collections;

public class FlyingSquirrel : MonoBehaviour {

	public float currentSpeed;
	public float floatSpeed = 1f;
	public float fallSpeed;
	public float groundSpeed;
	public float airSpeed;
	public float currentSideSpeed;
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
		if (currentSideSpeed == airSpeed) killMode = true;
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
		else GetComponent<PhysicsObject>().changeSideSpeed(speed);
		currentSideSpeed = speed;
	}

	void OnTriggerEnter(Collider other){
		if (GetComponent<Hero>()) return;
		if (!other.GetComponent<AnimalBehavior>()) return;
		//	If we're at this point, it's colliding with an animal
		other.gameObject.GetComponent<AnimalBehavior> ().health--;
		if (other.gameObject.GetComponent<AnimalBehavior> ().health <= 0){
			Destroy (other.gameObject);
		}
	}

}
