using UnityEngine;
using System.Collections;

public class BeeMovement : MonoBehaviour {
	bool changedDirection = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 cameraPos = Camera.main.transform.position;
		float camHeight = 2.0f * Camera.main.orthographicSize;
		float camWidth = camHeight * Camera.main.aspect;

		if ((transform.position.x <= (cameraPos.x - ((camWidth / 2) - 1f)))
		    ) {
			if (changedDirection == false){
				Vector3 vel = GetComponent<PhysicsObject>().vel;
				GetComponent<PhysicsObject>().vel = vel * -1;
				changedDirection = true;
			}
		}
	}
}
