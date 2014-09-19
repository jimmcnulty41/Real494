using UnityEngine;
using System.Collections;

public class AnimalBehavior : MonoBehaviour {
	public GameObject Message = null;

	public bool friendable;
	public int health;
	public float stunTime;
	public bool liveOffScreen = false;

	public bool friendly = false;
	public int candyCount = 0;
	bool stunned = false;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (liveOffScreen) {
			return;
		}
		//destroy this game object if it is outside the camera frame:
		Vector3 pos = transform.position;
		Vector3 cameraPos = Camera.main.transform.position;
		float camHeight = 2.0f * Camera.main.orthographicSize;
		float camWidth = camHeight * Camera.main.aspect;
		if (//above or below camera:
			pos.y > (cameraPos.y + (camHeight / 2)) || pos.y < (cameraPos.y - (camHeight / 2))
			//left or right of camera:
			|| pos.x > (cameraPos.x + (camWidth / 2)) || pos.x < (cameraPos.x - (camWidth / 2))
		    ) {
			Destroy (this.gameObject);
		}
	}

	IEnumerator Stun() {
		stunned = true;
		Vector3 oldVel = GetComponent<PhysicsObject> ().vel;
		GetComponent<PhysicsObject> ().vel = Vector3.zero;
		yield return new WaitForSeconds(stunTime);
		GetComponent<PhysicsObject> ().vel = oldVel;
		stunned = false;

	}

	public void GetStunned(){
		if (stunned == false) {
			StartCoroutine (Stun ());
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Map") {
			GetComponent<PhysicsObject> ().vel *= -1;
		}
	}



}
