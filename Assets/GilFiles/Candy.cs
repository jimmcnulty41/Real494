using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

	public float velocityX;


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
				other.gameObject.GetComponent<AnimalBehavior>().candyCount++;
				if (other.gameObject.GetComponent<AnimalBehavior>().candyCount >= 2)
					other.gameObject.GetComponent<AnimalBehavior>().friendly = true;
			}
		}
		//if (other.gameObject.GetComponent<OpenSideObject> () == null) {
	//	if (other.GetComponent<CollisionDetector> ().getRelationToObject (other) == relationToOther.UNDERNEATH
	//	    && other.GetComponent<OpenBottomObject>())
	//		return;
			Destroy (gameObject);
		
	}
}
