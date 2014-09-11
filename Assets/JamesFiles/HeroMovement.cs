using UnityEngine;
using System.Collections;


public class HeroMovement : MonoBehaviour {


	public float runSpeed = 1f;
	public float jumpHoldTime = .5f;
	
	
	float jumpStartTime;
	bool enableHigher;


	const float kBaseHeight = .6f;
	const float kFullHeight = 1.6f;
	

	void Update(){
		manageInputs();
	}
	
	void manageInputs(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		manageJumpInput();
		if (Input.GetKey(KeyCode.LeftArrow)) po.changeSideSpeed(-runSpeed);
		if (Input.GetKeyUp(KeyCode.LeftArrow)) po.changeSideSpeed(0);
		if (Input.GetKey(KeyCode.RightArrow)) po.changeSideSpeed(runSpeed);
		if (Input.GetKeyUp(KeyCode.RightArrow)) po.changeSideSpeed(0);
	}
	
	void manageJumpInput(){
		if (Input.GetKeyDown(KeyCode.X)){
			GetComponent<JumpingObject>().jump();
		}
		if (Input.GetKeyUp(KeyCode.X)){
			if (GetComponent<PhysicsObject>().onGround) return;
			GetComponent<FallingObject>().startJumpFall();
		}
	}
	
	
	
}
