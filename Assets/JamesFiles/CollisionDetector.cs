﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum relationToOther{
	ONTOP,
	TOLEFT,
	TORIGHT,
	UNDERNEATH
}

//	CollisionDetector Class
//================================================
//================================================
/*	This class allows objects to react normally to 
 * 	collisions with things. If this is enabled on 
 *  an object, that object will
 * 		1. Not be able to move sideways through other objects
 * 		2. Not be able to move up or down through other objects
 * 
 * 
 * 	REQUIRES PHYSICS OBJECT 
 * 		OPTIONALLY REQUIRES JUMPING OBJECT IF INFINITE JUMP IS DESIRED
 */

public class CollisionDetector : MonoBehaviour {

	public float forgiveness = .3f;
	public float sinkamt = .001f;
	public bool infinite_jump = false;
	public float offset = .01f;
	public ShroomCube onShroom = null;

	public bool ____________________________;
	
	public Dictionary<Collider, relationToOther> touchingObjects 
		= new Dictionary<Collider, relationToOther>();
		

	//	Trigger Enter/Stay functions
	//==========================================

	void OnTriggerEnter(Collider other){
		touchingObjects.Add(other, getRelationToObject(other));
		if (touchingObjects[other] == relationToOther.ONTOP)
			land (other);
		if (other.GetComponent<ShroomCube>() && !onShroom)
			onShroom = other.GetComponent<ShroomCube>();
		//print(touchingObjects[other] + " of " + other.name);
		switchOnRelation(other);
	}	

	
	void OnTriggerStay(Collider other){
		//print(touchingObjects[other] + " of " + other.name);
		switchOnRelation(other);
	}



	void switchOnRelation(Collider other){
		PhysicsObject po = GetComponent<PhysicsObject>();
		switch(touchingObjects[other]){
			case relationToOther.ONTOP:
				changeY(other.transform.position.y + getTouchingDistanceY(other.gameObject));
				break;
			case relationToOther.TOLEFT:	
			case relationToOther.TORIGHT:
				respondToSideHit(touchingObjects[other], other);
				break;
			case relationToOther.UNDERNEATH:
				respondToBottomHit(other, po);
				break;
		}
	}

	//	Respond to hitting sides/bottoms---------------------------------

	void respondToSideHit(relationToOther relation, Collider other){
		OpenSideObject hasOpenSide = other.GetComponent<OpenSideObject> ();
		if (hasOpenSide) return;
		if (relation == relationToOther.TOLEFT) {
			changeX (other.transform.position.x - getTouchingDistanceX (other.gameObject));
		} else if (relation == relationToOther.TORIGHT) {
			changeX(other.transform.position.x + getTouchingDistanceX(other.gameObject));
		}
	}

	void respondToBottomHit(Collider other, PhysicsObject po){
		OpenBottomObject hasOpenBottom = other.GetComponent<OpenBottomObject>();
		if (hasOpenBottom) return;
		changeY(other.transform.position.y - getTouchingDistanceY(other.gameObject));
		if (po.vel.y > 0) GetComponent<FallingObject>().fall();
	}
		
	public void changeX(float amt){
		Vector3 pos = transform.position;
		pos.x = amt;
		transform.position = pos;
	}
	
	public void changeY(float amt){
		Vector3 pos = transform.position;
		pos.y = amt;
		transform.position = pos;
	}

	
	void land(Collider landing){
		//	land on the object
		GetComponent<PhysicsObject>().land();
		GetComponent<FallingObject>().land();
//		if (landing.GetComponent<ShroomCube>()){
//			currentShroomLayer = landing.GetComponent<ShroomCube>();
//		} else {
//			currentShroomLayer = null;
//		}
		if (!landing.GetComponent<ShroomCube>()) onShroom = null;
		JumpingObject jo = GetComponent<JumpingObject>();
		if (infinite_jump) jo.jump();
		jo.doubleJump = false;

		if (GetComponent<Hero> () != null) {
			GetComponent<Hero> ().landMiniJump ();
		}
		if (infinite_jump) GetComponent<JumpingObject>().jump();
	}
	
	void hitSide(Collider side){
		//	Rub against side

	}

	//	OnTriggerExit Functions
	//===================================================
	
	void OnTriggerExit(Collider other){
		//print ("Removed " + other.name + " from touchingObjects");
		exitSwitchOnRelation(touchingObjects[other], other);
		touchingObjects.Remove(other);
	}
	

	void exitSwitchOnRelation(relationToOther relation, Collider other){
		switch(relation){
		case relationToOther.ONTOP:
			if (!GetComponent<PhysicsObject>().onGround) return;
			else GetComponent<FallingObject>().fall();
		break;
		}	
	}


	//	General Functions for relationship between objects
	//==========================================================

	relationToOther getRelationToObject(Collider other){
		float topDist = Mathf.Abs(collider.bounds.min.y - other.bounds.max.y);
		float bottomDist = Mathf.Abs(collider.bounds.max.y - other.bounds.min.y);
		float rightDist = Mathf.Abs(collider.bounds.min.x - other.bounds.max.x);
		float leftDist = Mathf.Abs(collider.bounds.max.x - other.bounds.min.x);
		float minDist = Mathf.Min(
			Mathf.Min(topDist, bottomDist), Mathf.Min(leftDist, rightDist));
		if (minDist == topDist)
			return relationToOther.ONTOP;
		else if (minDist == bottomDist)
			return relationToOther.UNDERNEATH;
		else if (minDist == rightDist)
			return relationToOther.TORIGHT;
		else //(minDist == leftDist)
			return relationToOther.TOLEFT;

	}


	float normalizedDistanceRatioY(GameObject go, GameObject other){
		float distance = go.transform.position.y - other.transform.position.y;
		float sumHW = vertHalfWidth(go) + vertHalfWidth(other);
		float ratio = Mathf.Abs(distance / sumHW);
		return 1 - ratio;
	}
	
	
	float normalizedDistanceRatioX(GameObject go, GameObject other){
		float distance = go.transform.position.x - other.transform.position.x;
		float sumHW = horHalfWidth(go) + horHalfWidth(other);
		float ratio = Mathf.Abs(distance / sumHW);
		return 1 - ratio;
	}
	
	relationToOther getVertOrientation(GameObject go, GameObject other){
		if (transform.position.y <= other.transform.position.y)
			return relationToOther.UNDERNEATH;
		else return relationToOther.ONTOP;
//		float topEdgeOther = getTopEdge(other);
//		float bottomEdge = getBottomEdge(go);
//		if ((bottomEdge <= topEdgeOther) && (bottomEdge >= topEdgeOther - forgiveness)) 
//			return relationToOther.ONTOP;
//		else return relationToOther.UNDERNEATH;
	}
	
	relationToOther getHorOrientation(GameObject go, GameObject other){
		if (transform.position.x <= other.transform.position.x)
			return relationToOther.TOLEFT;
		else return relationToOther.TORIGHT;
//		float rightEdgeOther = getRightEdge(other);
//		float leftEdge = getLeftEdge(go);
//		if ((leftEdge <= rightEdgeOther) && (leftEdge >= rightEdgeOther - forgiveness)) 
//			return relationToOther.TORIGHT;
//		else return relationToOther.TOLEFT;
	}
	
	//	Functions for getting boundaries
	//=========================================

	bool inBounds(Collider other){
		return (collider.bounds.min.x > other.bounds.max.x ||
		        collider.bounds.max.x < other.bounds.min.x);
	}
	
	float getTopEdge(GameObject go){
		return go.transform.position.y + vertHalfWidth(go);
	}
	
	float getBottomEdge(GameObject go){
		return go.transform.position.y - vertHalfWidth(go);
	}
	
	float getTouchingDistanceY(GameObject other){
		return vertHalfWidth(gameObject) + vertHalfWidth(other);
	}
		
	float vertHalfWidth(GameObject go){
		float yScale = go.transform.lossyScale.y;
		return (yScale / 2);
	}
	
	float getRightEdge(GameObject go){
		return go.transform.position.x + vertHalfWidth(go);
	}
	
	float getLeftEdge(GameObject go){
		return go.transform.position.x - vertHalfWidth(go);
	}
	
	float getTouchingDistanceX(GameObject other){
		return horHalfWidth(gameObject) + horHalfWidth(other);
	}

	float horHalfWidth(GameObject go){
		float xScale = go.transform.lossyScale.x;
		return (xScale / 2);
	}

	
}
