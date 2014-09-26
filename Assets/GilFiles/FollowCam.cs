using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	static public FollowCam S;

	public bool ___________________;
	public GameObject hero;
	public float RegionBoundaryX;
	public float RegionBoundaryY1;
	public float RegionBoundaryY2;
	public float InitialCameraHeight1;
	public float InitialCameraHeight2;

	bool falling = false; 
	public float CameraOffset; 

	// Use this for initialization
	void Start () {
		
	}
	
	void Awake() {
		S = this;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float FollowY; //used to check if camera should move up
		float InitialCameraHeight;
		//print (CameraOffset);
		//have camera follow the hero on the x axis:
		Vector3 targetPos = hero.transform.position;
		Vector3 destination = transform.position;
		destination.x = targetPos.x;
		transform.position = destination;

		//check if the camera has crossed the hardcoded boundry, and set the Y boundry accordingly:
		if (transform.position.x > RegionBoundaryX) { 
			FollowY = RegionBoundaryY2;
			InitialCameraHeight = InitialCameraHeight2;
		}
		else { 
			FollowY = RegionBoundaryY1;
			InitialCameraHeight = InitialCameraHeight1;

		}

		//check if the target destination crosses the Y boundry, and follow if so:
		 
		 if (targetPos.y > FollowY) {
			Vector3 destinationY = transform.position;
			destinationY.y = (targetPos.y - FollowY) + InitialCameraHeight;
			transform.position = destinationY;
			falling = true;
		} else if (falling == true) {
			print(targetPos);
			Vector3 destinationY = transform.position;
			destinationY.y = InitialCameraHeight;
			transform.position = destinationY;
			falling = false;

		}
	}
}