using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	static public FollowCam S;

	public bool ___________________;
	public GameObject hero = GameObject.Find("Hero");
	public float RegionBoudryX;
	public float RegionBoudryY1;
	public float RegionBoundryY2;
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
		Vector3 destination = hero.transform.position;
		Vector3 destinationX = new Vector3 (destination.x, 0, 0);
		transform.position += destinationX;

		//check if the camera has crossed the hardcoded boundry, and set the Y boundry accordingly:
		if (transform.position.x > RegionBoudryX) { FollowY = RegionBoundryY2; }
		else { FollowY = RegionBoundryY2; }

		//check if the target destination crosses the Y boundry, and follow if so:
		if (destination.y > FollowY) {
		Vector3 destinationY = new Vector3 (0, destination.y, 0);
		transform.position += destinationY;
		}
		
	}
}