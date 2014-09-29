using UnityEngine;
using System.Collections;

public class SticksToWalls : MonoBehaviour {

	public bool onWall = false;
	public bool stickingToLeftSideOfObject  = false;
	public bool ___________________;
	Vector3 normalShape;
	public Vector3 onWallShape;
	public Quaternion normalRotation;
	public Quaternion onLeftWallRotation;
	public Quaternion onRightWallRotation;


	void Awake(){
		normalShape = transform.localScale;
		onWallShape = normalShape;
		float x = onWallShape.x;
		onWallShape.x = onWallShape.y;
		onWallShape.y = x;
		//	Set up for rotations
		normalRotation = transform.rotation;
		onLeftWallRotation = normalRotation;
		onLeftWallRotation.z += 1;	
		onRightWallRotation = normalRotation;
		onRightWallRotation.z -= 1;
	}

	void FixedUpdate(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (onWall){ 
			po.changeSideSpeed(0);
			po.enableSpeedChange = false;
			transform.localScale = onWallShape;
			if (stickingToLeftSideOfObject) changeRotation(onLeftWallRotation);
			else changeRotation(onRightWallRotation);
		} else {
			landOnWallTop();
		}
	}

	public void landOnWallTop(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.enableSpeedChange = true;
		transform.localScale = normalShape;
		changeRotation(normalRotation);
	}

	public void changeRotation(Quaternion rotation){
		Transform child = transform.GetChild(0);
		child.rotation = rotation;
	}

	public void remove(){
		landOnWallTop();
	}
	
}
