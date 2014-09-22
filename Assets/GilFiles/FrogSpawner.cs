using UnityEngine;
using System.Collections;

public class FrogSpawner : MonoBehaviour {

	public GameObject FrogPreFab;
	GameObject frog = null;
	Vector3 pos;


	// Use this for initialization
	void Start () {
		pos = transform.position;
		frog = Instantiate (FrogPreFab) as GameObject;
		frog.transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 cameraPos = Camera.main.transform.position;
		float camHeight = 2.0f * Camera.main.orthographicSize;
		float camWidth = camHeight * Camera.main.aspect;

		if (pos.y > (cameraPos.y + (camHeight / 2) + 2) || pos.y < (cameraPos.y - (camHeight / 2) - 2)
		   || pos.x > (cameraPos.x + (camWidth / 2) + 2) || pos.x < (cameraPos.x - (camWidth / 2) - 2)
		   ) {
			if (frog == null){
				frog = Instantiate (FrogPreFab) as GameObject;
				frog.transform.position = pos;
			}
		}
	}
}
