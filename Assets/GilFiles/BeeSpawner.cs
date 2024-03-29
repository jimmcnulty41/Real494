﻿using UnityEngine;
using System.Collections;

public class BeeSpawner : MonoBehaviour {

	public GameObject BeePreFab;
	public float SpawnDelay;
	Time startTime;
	float delay = .5f;

	GameObject bee = null; 
	public bool canSpawn = true;

	// Use this for initialization
	void Start () {
	
	}

	IEnumerator Spawn() {

		canSpawn = false;
		GameObject hero = GameObject.Find ("Hero_wSprite");

		yield return new WaitForSeconds (SpawnDelay);

		Vector3 pos = hero.transform.position;
		float camHeight = 2.0f * Camera.main.orthographicSize;
		float camWidth = camHeight * Camera.main.aspect;
		
		pos.y += (camWidth / 4);
		pos.x += ((camWidth / 2) - 0.5f);
		pos.z = 0f;

		bee = Instantiate (BeePreFab) as GameObject;		
		bee.transform.position = pos;
		canSpawn = true;

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
//			GameObject hero = GameObject.Find ("Hero");
//			pos = hero.transform.position;
//			pos.y += (camWidth / 4);
//			pos.x += ((camWidth / 2) - 0.5f);
//			pos.z = -0.2f;
//			bee = Instantiate (BeePreFab) as GameObject;
//			bee.transform.position = pos;
//		}
			if (canSpawn == false) {
				return;
			}
			StartCoroutine (Spawn ());
		}
	}
}
