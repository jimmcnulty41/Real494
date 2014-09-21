using UnityEngine;
using System.Collections;

public class ShroomCube : MonoBehaviour {


	public bool _______________________;
	public GameObject upShroom = null;
	public GameObject downShroom = null;
	public float xDiff;
	public float yDiff;
	public float minX;
	public Vector3 baseloc;
	public Vector3 baseScale;
	public Vector3 bottomLeftCorner;
	public Vector3 topLeftCorner;
	public Vector3 topRightCorner;
	public Vector3 bottomRightCorner;
	public Vector3 vLeftToCenter;
	public Vector3 vLeftToCenterNormX;
	public Vector3 vRightToCenter;
	public Vector3 vRightToCenterNormX;
	public int number;
	static public int numberCounter = 0;

	public void setNum(){
		number = numberCounter++;
	}

	void Start(){
		bottomLeftCorner = new Vector3((baseloc.x - (baseScale.x / 2)), (baseloc.y + (baseScale.y / 2)),0);
		bottomRightCorner = new Vector3((baseloc.x + (baseScale.x / 2)), (baseloc.y + (baseScale.y / 2)),0);
		float cornerXDiff = .5f * (baseScale.x - minX);
		float numBlocksish = cornerXDiff / xDiff;
		topLeftCorner = new Vector3((baseloc.x - (minX / 2)), baseloc.y + (numBlocksish * yDiff), 0);
		topRightCorner = new Vector3((baseloc.x + (minX / 2)), baseloc.y + (numBlocksish * yDiff), 0);
		vLeftToCenter = (topLeftCorner - bottomLeftCorner);
		vLeftToCenterNormX = vLeftToCenter / vLeftToCenter.x;
		vRightToCenter = (topRightCorner - bottomRightCorner);
		vRightToCenterNormX = vRightToCenter / vRightToCenter.x;

	}

	public void modifyVel(GameObject mover, float speed){
		PhysicsObject po = mover.GetComponent<PhysicsObject>();
		Vector3 newVelocity = po.vel;
		newVelocity.y = 0;
		po.vel = newVelocity;
		if (mover.collider.bounds.max.x >= bottomLeftCorner.x 
			&& mover.collider.bounds.max.x <= topLeftCorner.x
			&& speed > 0)
		{

		}
		if (mover.collider.bounds.min.x <= bottomRightCorner.x 
		    && mover.collider.bounds.min.x >= topRightCorner.x
		    && speed < 0)
		{

		}
	}


	public void modifyPosition(GameObject mover){
//		if (mover.collider.bounds.max.x >= bottomLeftCorner.x 
//		   		&& mover.collider.bounds.max.x <= topLeftCorner.x)
//		{	//	On the left side
//			float difference = mover.collider.bounds.max.x - bottomLeftCorner.x;
//			float newY = (difference * vLeftToCenterNormX).y;
//			Vector3 pos = mover.transform.position;
//			pos.y = newY;
//			mover.transform.position = pos;
//		}
//		if (mover.collider.bounds.min.x <= bottomRightCorner.x 
//		    		&& mover.collider.bounds.min.x >= topRightCorner.x)
//		{	// On the right side
//			float difference = mover.collider.bounds.min.x - bottomRightCorner.x;
//			float newY = (difference * vRightToCenterNormX).y + bottomRightCorner.y;
//			Vector3 pos = mover.transform.position;
//			pos.y = newY;
//			mover.transform.position = pos;
//		}
		mover.transform.position = GetComponentInParent<MakeShroom>().getCorrectPos(mover);
		mover.GetComponent<PhysicsObject>().onGround = true;
	}

	public bool offShroom(GameObject go){
		return GetComponentInParent<MakeShroom>().offShroom(go);
	}
}
