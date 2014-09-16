using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

	public float velocityX;


	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		GameObject hero = GameObject.Find("Hero");
		if (hero.GetComponent<HeroMovement> ().facingLeft) {
			GetComponent<PhysicsObject> ().vel.x = -velocityX;
			print (GetComponent<PhysicsObject> ().vel);
		} else {
			GetComponent<PhysicsObject> ().vel.x = velocityX;
			print (GetComponent<PhysicsObject> ().vel);

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	bool checkValidTarget(Collider other){
		if (other.gameObject.tag == "Frog" || other.gameObject.tag == "Mole" || other.gameObject.tag == "Lizard"
			|| other.gameObject.tag == "Snail" || other.gameObject.tag == "Bee") {
			return true;
		} 
		else {
			return false;
		}
	}
	void OnTriggerEnter(Collider other){

		print (transform.position.x);
		Destroy (gameObject);
	}
}
