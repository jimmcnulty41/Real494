using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour {

	public Vector3 gravity = new Vector3(0,-9.8f,0);
	public Vector3 accel = Vector3.zero;
	public Vector3 vel = Vector3.zero;
	public bool immovable;
	public bool onGround = false;
	public float touchRange = .1f;
	
	public bool ___________________;
	

	
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
		accel += gravity * Time.deltaTime;
		vel += accel * Time.deltaTime;
		Vector3 pos = transform.position;
		pos.y += vel.y * Time.deltaTime;
		transform.position = pos;
	}
	
	void OnTriggerEnter(Collider other){
		//checkSideMovement(other);
		if (onGround) return;
		checkLanding (other);
	}
	
	void OnTriggerStay(Collider other){
		//checkSideMovement(other);
		if (onGround) return;
		checkLanding (other);
	}
	
	void checkSideMovement(Collider other){
		//	If we're moving right, check the right edge
		//	against other's left edge
		if (vel.x > 0){
			if (getRightEdge(gameObject) >= getLeftEdge(other.gameObject))
				changeHorPos(getLeftEdge(other.gameObject));
		}
		
		//	If we're moving left, check the left edge
		//	against other's right edge
		if (vel.x < 0){
			if (getLeftEdge(gameObject) <= getRightEdge(other.gameObject))
				changeHorPos(getRightEdge(other.gameObject));
		}
		
	}
	
	void changeHorPos(float horPos){
		Vector3 pos = transform.position;
		pos.x = horPos;
		transform.position = pos;
	}
	
	void checkLanding(Collider other){
		if (onGround) return;
	
		//	Get the top edge of the colliding object		
		if (vel.y < 0 
		    && (kindofTouching(getTopEdge(other.gameObject), (getBottomEdge(gameObject))))
			&& transform.position.y > other.gameObject.transform.position.y){
			land (other);
		}
		
	}
	
	bool kindofTouching(float topRightEdge, float bottomLeftEdge){
		if (topRightEdge - touchRange <= bottomLeftEdge) return true;
		return false;
	}
	
	void land(Collider other){
//		vel = Vector3.zero;
		negateVertAcceleration();
		
		//	Move box to ground position
		Vector3 curPos = transform.position;
		curPos.y = getTopEdge(other.gameObject) + (transform.lossyScale.y / 2);
		transform.position = curPos;
		
		onGround = true;
	}
	
	void negateVertAcceleration(){
		Vector3 curAccel = accel;
		curAccel.y = 0;
		accel = curAccel;
		Vector3 curVel = vel;
		curVel.y = 0;
		vel = curVel;
	}
	
	//	Functions for getting boundaries
	//=========================================
	
	float getTopEdge(GameObject go){
		float yScale = go.transform.lossyScale.y;
		float yPos = go.transform.position.y;
		return yPos + (yScale / 2);
	}
	
	float getBottomEdge(GameObject go){
		float yScale = go.transform.lossyScale.y;
		float yPos = go.transform.position.y;
		return yPos - (yScale / 2);
	}
	
	float getRightEdge(GameObject go){
		float xScale = go.transform.lossyScale.x;
		float xPos = go.transform.position.x;
		return xPos + (xScale / 2);
	}
	
	float getLeftEdge(GameObject go){
		float xScale = go.transform.lossyScale.x;
		float xPos = go.transform.position.x;
		return xPos - (xScale / 2);
	}

}
