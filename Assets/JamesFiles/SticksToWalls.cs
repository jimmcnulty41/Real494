using UnityEngine;
using System.Collections;

public class SticksToWalls : MonoBehaviour {

	public bool onWall;
	public bool stickingToLeftSideOfObject;
	Vector3 normalShape;
	Vector3 onWallShape;

	void Awake(){
		normalShape = transform.localScale;
		onWallShape = normalShape;
		float x = onWallShape.x;
		onWallShape.x = onWallShape.y;
		onWallShape.y = x;
	}

	void FixedUpdate(){
		if (onWall){ 
			PhysicsObject po = GetComponent<PhysicsObject>();
			po.changeSideSpeed(0);
			transform.localScale = onWallShape;
		} else {
			transform.localScale = normalShape;
		}
	}
}
