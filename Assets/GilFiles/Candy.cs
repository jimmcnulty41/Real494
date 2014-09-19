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

	void OnTriggerEnter(Collider other){

		if (other.gameObject.GetComponent<AnimalBehavior> () != null) {
			if (other.gameObject.GetComponent<AnimalBehavior>().friendable == false) {
				other.gameObject.GetComponent<AnimalBehavior>().GetStunned();
			}
			else { 
				other.gameObject.GetComponent<AnimalBehavior>().candyCount++;
				if (other.gameObject.GetComponent<AnimalBehavior>().candyCount >= 2)
					other.gameObject.GetComponent<AnimalBehavior>().friendly = true;
			}
		}
		Destroy (gameObject);
	}
}
