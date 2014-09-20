using UnityEngine;
using System.Collections;

public class BeeSpawner : MonoBehaviour {

	public GameObject BeePreFab;

	GameObject bee = null; 


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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

		if (bee == null) {
			GameObject hero = GameObject.Find("Hero");
			pos = hero.transform.position;
			pos.y += (camWidth / 4);
			pos.x += ((camWidth / 2) - 0.5f);
			pos.z = 0;
			bee = Instantiate (BeePreFab) as GameObject;

			bee.transform.position = pos;
			
		}

		if (bee != null) {
			if ((bee.transform.position.x <= (cameraPos.x - ((camWidth / 2) - 0.5f)))
			 ||
				(bee.transform.position.y <= (cameraPos.y - ((camHeight / 2) - 0.5f)))
			   ) {
				Vector3 vel = bee.GetComponent<PhysicsObject>().vel;
				bee.GetComponent<PhysicsObject>().vel = vel * -1;
			}
		}

	}
}
