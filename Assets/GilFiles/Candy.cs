﻿using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

	public float velocityX;
	int candyCountNeeded = 2;


	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		GameObject hero = GameObject.Find("Hero_wSprite");
		PhysicsObject po = GetComponent<PhysicsObject>();
		Vector3 accel = po.accel;
		if (hero.GetComponent<HeroMovement> ().facingLeft) {
			accel.x = velocityX * -1;
		} else {
			accel.x = velocityX;
		}
		po.accel = accel;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.GetComponent<AnimalBehavior>() != null) {
			print ("here");
			if (other.gameObject.GetComponent<AnimalBehavior>().friendable == false) {
				other.gameObject.GetComponent<AnimalBehavior>().GetStunned();
			}
			else { 
				other.GetComponent<AnimalBehavior>().candyCount++;
				if (other.gameObject.GetComponent<AnimalBehavior>().candyCount >= candyCountNeeded)
					other.GetComponent<AnimalBehavior>().makeFriendly();
			}
		}
		//if (other.gameObject.GetComponent<OpenSideObject> () == null) {
	//	if (other.GetComponent<CollisionDetector> ().getRelationToObject (other) == relationToOther.UNDERNEATH
	//	    && other.GetComponent<OpenBottomObject>())
	//		return;
		print ("Candy hitting " + other.name + other.tag);
		if (GetComponent<CollisionDetector>().getRelationToObject(other) == relationToOther.ONTOP
		    || destroyOnHit(other.tag)){
			Destroy (gameObject);
		}
		
	}

	bool destroyOnHit(string tag){
		if (tag == "Bee") return true;
		if (tag == "Ground") return true;
		if (tag == "Snail") return true;
		if (tag == "Frog") return true;
		if (tag == "Lizard") return true;
		if (tag == "Squirrel") return true;
		return false;
	}
}
