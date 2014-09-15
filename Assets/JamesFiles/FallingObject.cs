using UnityEngine;
using System.Collections;

public class FallingObject : MonoBehaviour {

	public float fallSpeed = 28;
	public float transitionEasing = .1f;
	public bool falling = false;
	
	void FixedUpdate(){
		if (!falling) return;
		if (GetComponent<PhysicsObject>().vel.y > 0) transitionIntoFall();
		fall();
	}

	//	Public functions
	public void fall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (!po.onGround) return;
		po.onGround = false;
		falling = true;
		GetComponent<PhysicsObject>().accel = new Vector3(0,-fallSpeed,0);
	}
	
	public void startJumpFall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.accel = new Vector3(0,-fallSpeed,0);
		falling = true;
	}
	
	public void transitionIntoFall(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		Vector3 vel = po.vel;
		vel.y = Mathf.Lerp(vel.y,0,transitionEasing);
		po.vel = vel;
	}
	
	public void land(){
		falling = false;
	}
	
}
