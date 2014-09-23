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
	public bool facingLeft = false;

	void Update(){
		manageInputs();
	}
	
	void manageInputs(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		manageJumpInput();

		if (Input.GetKey (KeyCode.LeftArrow)) {
			po.changeSideSpeed (-runSpeed);
			facingLeft = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow)) po.changeSideSpeed(0);
		if (Input.GetKey (KeyCode.RightArrow)) {
			po.changeSideSpeed (runSpeed);
			facingLeft = false;
		}
		if (Input.GetKeyUp(KeyCode.RightArrow)) po.changeSideSpeed(0);

		//	Handle up down movement if we're on a wall
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (!(stw && stw.onWall)) return;
		if (Input.GetKey(KeyCode.UpArrow)){
			po.changeVertSpeed(runSpeed);
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			po.changeVertSpeed(-runSpeed);
		}
		if (Input.GetKeyUp(KeyCode.DownArrow)){
			po.changeVertSpeed(0);
		}
		if (Input.GetKeyUp(KeyCode.UpArrow)){
			po.changeVertSpeed(0);
		}
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
