using UnityEngine;
using System.Collections;

public class SnailSpawner : MonoBehaviour {

	public GameObject SnailPreFab;

	GameObject snail = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//check if spawning snails or bees
		Vector3 pos = transform.position;

		Vector3 cameraPos = Camera.main.transform.position;
		float camHeight = 2.0f * Camera.main.orthographicSize;
		float camWidth = camHeight * Camera.main.aspect;

		//check if its off screen and return if so
		if (pos.y > (cameraPos.y + (camHeight / 2)) || pos.y < (cameraPos.y - (camHeight / 2))
			|| pos.x > (cameraPos.x + (camWidth / 2)) || pos.x < (cameraPos.x - (camWidth / 2))
		    ) {
			return;
		}
		//otherwise check if a snail has been made and instai
		if (snail == null) {

			snail = Instantiate (SnailPreFab) as GameObject;
			snail.transform.position = pos;

		}
	}
}
