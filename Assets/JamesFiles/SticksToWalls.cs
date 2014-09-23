using UnityEngine;
using System.Collections;

public class SticksToWalls : MonoBehaviour {

	public bool onWall;
	public bool stickingToLeftSideOfObject;

	void Update(){
		if (onWall) GetComponent<PhysicsObject>().onGround = true;
	}
}
