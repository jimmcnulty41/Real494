using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum relationToOther{
	ONTOP,
	TOLEFT,
	TORIGHT,
	UNDERNEATH,
	ONSLOPE
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
		
	//========================================================================
	//	Trigger Enter/Stay functions
	//========================================================================

	void OnTriggerEnter(Collider other){
		touchingObjects.Add(other, getRelationToObject(other));
		if (touchingObjects[other] == relationToOther.ONTOP)
			land (other);
		if (touchingObjects[other] == relationToOther.ONSLOPE && !onShroom){
			onShroom = other.GetComponent<ShroomCube>();
			other.GetComponent<ShroomCube>().modifyPosition(gameObject);
			land(other);
		}
		print(touchingObjects[other] + " of " + other.name);
		switchOnRelation(other);
	}	

	
	void OnTriggerStay(Collider other){
		//print(touchingObjects[other] + " of " + other.name);
		switchOnRelation(other);
	}

	void Update(){
		if (onShroom){
			onShroom.modifyPosition(gameObject);
			if (onShroom.offShroom(gameObject)) {
				GetComponent<FallingObject>().fall();
				onShroom = null;
			}
		}
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
		//	Handle sticking to walls if we should, otherwise, don't
		if (manageWallSticking(relation, other)) return;
		//	If it has open sides, we go through them, so we don't adjust the position,
		//	unless we're sticking to the wall, in which case we do
		OpenSideObject hasOpenSide = other.GetComponent<OpenSideObject> ();
		if (hasOpenSide) return;
		moveToWallEdge(relation, other);
	}

	//	Manages the collisions for objects which stick to walls,
	//	If the object will react as if it does not stick to walls, returns false
	bool manageWallSticking(relationToOther relation, Collider other){
		PhysicsObject po = GetComponent<PhysicsObject>();
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (!stw) return false;
		if (po.onGround) return false;
		// Possible rotation solution -- however, needs to calculate touching distance differently
		//transform.rotation = Quaternion.Euler(0,0,90);
		if (checkAlreadyInBlock(relation, other)) return false;
		if (!checkCorrectVelocity(relation, other)) return false;
		//	If this is the case, we're in the air, we haven't already crossed the plane
		//	and we're headed in the direction of the wall. 
		stickToEdge(relation, other);
		return true;
	}

	//	Returns true if we have already crossed the plane of a block
	bool checkAlreadyInBlock(relationToOther relation, Collider other){
		if (collider.bounds.max.x > other.bounds.min.x + forgiveness &&
		    collider.bounds.min.x < other.bounds.min.x) return true;
		if (collider.bounds.min.x < other.bounds.max.x - forgiveness &&
		    collider.bounds.max.x > other.bounds.max.x) return true;
		if (collider.bounds.max.x < other.bounds.max.x &&
		    collider.bounds.min.x > other.bounds.min.x) return true;
		return false;
	}

	
	//	Returns true if the velocity is correct for sticking to walls 
	//	i.e. the hero is jumping toward the wall
	bool checkCorrectVelocity(relationToOther relation, Collider other){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (relation == relationToOther.TOLEFT &&
		    po.vel.x <= 0) return false;
		if (relation == relationToOther.TORIGHT &&
		    po.vel.x >= 0) return false;
		return true;
	}


	void stickToEdge(relationToOther relation, Collider other){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.negateVertAcceleration();
		//	Update the sticks to walls object
		SticksToWalls stw = GetComponent<SticksToWalls>();
		stw.onWall = true;
		if (relation == relationToOther.TOLEFT) stw.stickingToLeftSideOfObject = true;
		else stw.stickingToLeftSideOfObject = false;
		//	Move to the edge of the wall
		moveToWallEdge(relation, other);
	}

	void moveToWallEdge(relationToOther relation, Collider other){
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
		if (po.vel.y > 0)
			GetComponent<FallingObject>().instantFall();
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
		if (GetComponent<PhysicsObject>() != null) GetComponent<PhysicsObject>().land();
		if (GetComponent<FallingObject>() != null) GetComponent<FallingObject>().land();
//		if (landing.GetComponent<ShroomCube>()){
//			currentShroomLayer = landing.GetComponent<ShroomCube>();
//		} else {
//			currentShroomLayer = null;
//		}
		if (GetComponent<SticksToWalls>()) GetComponent<SticksToWalls>().onWall = false;
		if (!landing.GetComponent<ShroomCube>()) onShroom = null;
		if (GetComponent<JumpingObject> () != null) {
			JumpingObject jo = GetComponent<JumpingObject> ();
			if (infinite_jump)
				jo.jump ();
			jo.doubleJump = false;
		}
		if (GetComponent<Hero> () != null) {
			GetComponent<Hero> ().landMiniJump ();
		}
		if (infinite_jump) GetComponent<JumpingObject>().jump();
	}
	
	void hitSide(Collider side){
		//	Rub against side

	}

	//=====================================================================
	//	OnTriggerExit Functions
	//=====================================================================
	
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

	public relationToOther getRelationToObject(Collider other){
		float topDist = Mathf.Abs(collider.bounds.min.y - other.bounds.max.y);
		float bottomDist = Mathf.Abs(collider.bounds.max.y - other.bounds.min.y);
		float rightDist = Mathf.Abs(collider.bounds.min.x - other.bounds.max.x);
		float leftDist = Mathf.Abs(collider.bounds.max.x - other.bounds.min.x);
		float minDist = Mathf.Min(
			Mathf.Min(topDist, bottomDist), Mathf.Min(leftDist, rightDist));
		if (other.GetComponent<ShroomCube>() && (minDist != bottomDist))
			return relationToOther.ONSLOPE;
		else if (minDist == topDist)
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
		return (collider.bounds.min.x < other.bounds.max.x 
		        && collider.bounds.max.x > other.bounds.min.x);
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
