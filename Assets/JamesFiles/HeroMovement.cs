using UnityEngine;
using System.Collections;

public class HeroMovement : MonoBehaviour {


	public float runSpeed = 1f;


	void Update(){
		manageInputs();
	}
	
	void manageInputs(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (Input.GetKeyDown(KeyCode.X)) GetComponent<JumpingObject>().jump();
		if (Input.GetKey(KeyCode.LeftArrow)) po.changeSideSpeed(-runSpeed);
		if (Input.GetKeyUp(KeyCode.LeftArrow)) po.changeSideSpeed(0);
		if (Input.GetKey(KeyCode.RightArrow)) po.changeSideSpeed(runSpeed);
		if (Input.GetKeyUp(KeyCode.RightArrow)) po.changeSideSpeed(0);
	}
	
}
