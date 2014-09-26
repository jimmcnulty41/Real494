using UnityEngine;
using System.Collections;

public class SticksToWalls : MonoBehaviour {

	public bool onWall;
	public bool stickingToLeftSideOfObject;
	public bool ___________________;
	Vector3 normalShape;
	public Vector3 onWallShape;

	void Awake(){
		normalShape = transform.localScale;
		onWallShape = normalShape;
		float x = onWallShape.x;
		onWallShape.x = onWallShape.y;
		onWallShape.y = x;
	}

	void FixedUpdate(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (onWall){ 
			po.changeSideSpeed(0);
			po.enableSpeedChange = false;
			transform.localScale = onWallShape;
		} else {
			landOnWallTop();
		}
	}

	public void landOnWallTop(){
		PhysicsObject po = GetComponent<PhysicsObject>();
		po.enableSpeedChange = true;
		transform.localScale = normalShape;
	}

}
