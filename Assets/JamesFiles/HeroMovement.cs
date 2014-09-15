using UnityEngine;
using System.Collections;


//	HeroMovement Class
//================================================
//================================================
/*	This class allows a player to control the hero using standard keys.
 * 
 * 	The run speed can be set in this class
 * 
 * 
 * 	REQUIRES PHYSICS OBJECT, FALLING OBJECT, AND JUMPING OBJECT
 */

public class HeroMovement : MonoBehaviour {


	public float runSpeed = 1f;

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
