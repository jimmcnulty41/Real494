using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	static public FollowCam S;

	public bool ___________________;
	public GameObject hero;
	public float RegionBoundaryX;
	public float RegionBoundaryY1;
	public float RegionBoundaryY2;
	bool falling = false;
	// Use this for initialization
	void Start () {
		
	}
	
	void Awake() {
		S = this;
	}
	
	// Update is called once per frame
	void Update () {
		float FollowY; //used to check if camera should move up

		//have camera follow the hero on the x axis:
		Vector3 targetPos = hero.transform.position;
		Vector3 destination = transform.position;
		destination.x = targetPos.x;
		transform.position = destination;

		//check if the camera has crossed the hardcoded boundry, and set the Y boundry accordingly:
		if (transform.position.x > RegionBoundaryX) { FollowY = RegionBoundaryY2; }
		else { FollowY = RegionBoundaryY1; }

		//check if the target destination crosses the Y boundry, and follow if so:
		 if (targetPos.y > FollowY) {
			Vector3 destinationY = transform.position;
			destinationY.y = targetPos.y;
			transform.position = destinationY;
			falling = true;
		} else if (falling == true) {
			Vector3 destinationY = transform.position;
			destinationY.y = targetPos.y;
			transform.position = destinationY;
			falling = false;
		}
	}
}