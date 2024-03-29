﻿using UnityEngine;
using System.Collections;

public class SquirrelFlyBackAndForthDrop : MonoBehaviour {

	public Vector3 startPos;
	public float backDist = 3f;
	public float fallDist = 3f;
	public bool goingBack = true;
	public bool goingForth = true;
	public bool done = false;
	public bool ________________;
	float airSpeed;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		airSpeed = GetComponent<FlyingSquirrel>().airSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (done) return;
		if (goingBack){
			goBack();
		}
		else if (goingForth){
			goForth();
		} else drop();
	}

	void goBack(){
		if (transform.position.x <= startPos.x - backDist){
			startGoingForth();
			return;
		}
		GetComponent<FlyingSquirrel>().xHeld = true;
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.changeSideSpeed(-airSpeed);

	}

	void startGoingForth(){
		goingBack = false;
		GetComponent<FlyingSquirrel>().xHeld = false;
	}
	
	void goForth(){
		if (transform.position.x >= startPos.x){
			startDrop();
			return;
		}
		GetComponent<FlyingSquirrel>().xHeld = true;
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.changeSideSpeed(airSpeed);
	}

	void startDrop(){
		GetComponent<FlyingSquirrel>().xHeld = false;
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.changeSideSpeed(0);
		goingForth = false;
	}

	void drop(){
		if (transform.position.y <= startPos.y - fallDist){
			startFinal();
			return;
		}
		GetComponent<FlyingSquirrel>().xHeld = false;
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.changeSideSpeed(0);
	}

	void startFinal(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.changeSideSpeed(airSpeed);
		GetComponent<FlyingSquirrel>().xHeld = true;
		done = true;
	}
}
