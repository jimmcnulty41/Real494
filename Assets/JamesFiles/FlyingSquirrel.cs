using UnityEngine;
using System.Collections;

public class FlyingSquirrel : MonoBehaviour {

	public float currentSpeed;
	public float floatSpeed = 1f;
	public float fallSpeed;

	void Awake(){
		FallingObject fo = GetComponent<FallingObject>();
		fallSpeed = fo.fallSpeed;
	}

	void Update(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		FallingObject fo = GetComponent<FallingObject>();
		if (po.vel.y >= 0f || po.onGround){
			fo.fallSpeed = fallSpeed;
			currentSpeed = fo.fallSpeed;
			return;
		}
		if (Input.GetKey(KeyCode.X)){
			//	maintain slow fall speed
			Vector3 vel = po.vel;
			if (currentSpeed != floatSpeed) 
				po.negateVertMovement();
			fo.fallSpeed = floatSpeed;
		} else {
			//	Return to normal fall speed
			fo.fallSpeed = fallSpeed;
		}
		currentSpeed = fo.fallSpeed;
	}

}
