using UnityEngine;
using System.Collections;

/*
This script defines hero behavior and traits. It includes some of the core mechanics such as throwing candy, 
and changing type/changing back from type.

Requires: Hero is a physics object, jumping object, falling object, and animal. GUI elements should exist in the scene. Hero
should be have the tag "Hero" & all other GameObjects should be tagged

TODO: make ontriggerenter more specific of hitting the side or the top of an animal and responding accordingly, 
check if the health has decremented <= 0 before killing it etc..
make animal behavior to check wether or not the animal is a friend or not before transforming.
 */

public class Hero : MonoBehaviour {

	public GameObject Candy; //the game object to create

	public bool killOnJump;
	public bool stickToWalls;
	public bool dig;
	public int health;
	public string type;

	public float throwingDelay; //time it takes to initiate throwing
	public float betweenDelay; //time it takes between throws
	public float miniJumpHeight;
	public float miniJumpVelocity;

	static public bool ______________________;

	//GUI text:
	public GUIText livesGT;
	public GUIText typeGT;
	public GUIText healthGT;
	public GUIText keysGT;

	int livesCount;
	int keysCount;
	public int keysMax;

	bool canThrow = true; //flag for enable/disable throw
	bool miniJumped;

	
	float originalJumpHeight;
	float originalJumpVelocity;

	// Use this for initialization
	void Start () {

		//set the variables and GUI text:
		killOnJump = false;
		stickToWalls = false;
		livesCount = 3;
		keysCount = 0;
		type = "Nemo";
		health = 3;

		GameObject livesGO = GameObject.Find ("lives");
		GameObject keysGO = GameObject.Find ("keys");
		GameObject typeGO = GameObject.Find ("type");
		GameObject healthGO = GameObject.Find ("health");


		livesGT = livesGO.GetComponent<GUIText> ();
		livesGT.text = "Lives: " +livesCount;

		keysGT = keysGO.GetComponent<GUIText> ();
		keysGT.text = "Keys: " + keysCount;

		typeGT = typeGO.GetComponent<GUIText> ();
		typeGT.text = "Type: " + type;

		healthGT = healthGO.GetComponent<GUIText> ();
		healthGT.text = "Health: " + health;

		originalJumpHeight = GetComponent<JumpingObject>().jumpHeight;
		originalJumpVelocity = GetComponent<JumpingObject>().jumpVelocity;
	}
	
	// Update is called once per frame
	void Update () {
		manageInputs();
	}

	void manageInputs(){

		if (Input.GetKeyDown(KeyCode.Q)) die();
		if (Input.GetKeyDown(KeyCode.W)) changeBack();
		if (Input.GetKeyDown(KeyCode.C)) throwCandy();
//		if (Input.GetKeyDown(KeyCode.V)) jump();
	}

	IEnumerator Throw() {
		//set canThrow to false to disable another throw
		canThrow = false;

		//freeze if on the ground:
		PhysicsObject po = GetComponent<PhysicsObject>();
		if (po.onGround) {
			po.immovable = true;
		}

		//delay the throw by throwingDelay:
		yield return new WaitForSeconds(throwingDelay);

		//create the candy on top of hero and throw it:
		GameObject candy = Instantiate (Candy) as GameObject;
		Vector3 pos = transform.position;
		pos.y += (transform.lossyScale.y / 2) + (candy.transform.lossyScale.y / 2);
		candy.transform.position = pos;
		//needs to go in -x direction if facing left:

		candy.GetComponent<JumpingObject> ().jump ();

		//unfreeze:
		po.immovable = false;

		//wait for betweenDelay time to enable throwing again:
		yield return new WaitForSeconds(betweenDelay);
		canThrow = true;

	}

	void throwCandy(){
		//only throw if it has been long enough
		if (canThrow) {

			StartCoroutine (Throw ());
		}
	}

	void Transform (Collider other){

		//get features of the animal
		JumpingObject animalJO = other.gameObject.GetComponent<JumpingObject> ();
		FallingObject animalFO = other.gameObject.GetComponent<FallingObject> ();
		Vector3 animalPOS = other.gameObject.transform.position;
		Material animalMAT = other.gameObject.renderer.material;
		Destroy (other.gameObject);
		//transfer it to Nemo
		type = other.gameObject.tag;
		typeGT.text = "Type: " + type;
		transform.position = animalPOS;
		GetComponent<JumpingObject> ().jumpHeight = animalJO.jumpHeight;
		GetComponent<JumpingObject> ().jumpVelocity = animalJO.jumpVelocity;
		GetComponent<FallingObject> ().fallSpeed = animalFO.fallSpeed;
		GetComponent<FallingObject> ().transitionEasing = animalFO.transitionEasing;
		renderer.material.color = animalMAT.color;
		//update special ability
		if (type == "Frog") {
			killOnJump = true;
		} else if (type == "Lizard") {
			stickToWalls = true;
		} else if (type == "Mole") {
			dig = true;
		}
		//set the new original jump:
		originalJumpHeight = GetComponent<JumpingObject>().jumpHeight;
		originalJumpVelocity = GetComponent<JumpingObject>().jumpVelocity;
		//can't throw while not nemo
		canThrow = false;
	}

	void changeBack (){
		GetComponent<JumpingObject> ().jumpHeight = 1.2f;
		GetComponent<JumpingObject> ().jumpVelocity = 8.0f;
		GetComponent<FallingObject> ().fallSpeed = 24.0f;
		GetComponent<FallingObject> ().transitionEasing = 0.05f;
		type = "Nemo";
		typeGT.text = "Type: " + type;

		renderer.material.color = Color.white;

		killOnJump = false;
		stickToWalls = false;
		dig = false;
		canThrow = true;
	}

	void OnTriggerEnter(Collider other){
		//first, check if its an Animal - only animals have this component
		if (other.gameObject.GetComponent<AnimalBehavior> () != null) {
			//check if its friendly and transform if so:
			if (other.gameObject.GetComponent<AnimalBehavior> ().friendly) {
				Transform (other);
				return;
			}
			//If not, check if you are hitting it from above by checking if hero's bottom is above the animal's top
			float heroBottom = transform.position.y - (transform.lossyScale.y / 2);
			float animalTop = other.gameObject.transform.position.y + (other.gameObject.transform.lossyScale.y / 2);
			if (heroBottom >= animalTop) {
				kill (other);		
			} else {
				health--;
				healthGT.text = "Health: " + health;
				miniJump();
				if (health <= 0) {
					die ();
				}
			}
		}
		//if its not an animal, check if its a gui object and interact accordingly
		else if (other.gameObject.tag == "Life") {
			livesCount++;
			livesGT.text = "Lives: " + livesCount.ToString ();
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Key") {
			keysCount++;
			keysGT.text = "Keys: " + keysCount.ToString ();
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Health") {
			if (health < 4) {
				health++;
			}
			healthGT.text = "Health: " + health;
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Door") {
			if (keysCount >= keysMax){
				Application.LoadLevel ("_Scene_0");
			}
		}
	}

	void miniJump (){
		miniJumped = true;
		//cut jumping object by half
		GetComponent<JumpingObject> ().jumpVelocity = miniJumpVelocity;
		GetComponent<JumpingObject> ().jumpHeight = miniJumpHeight;
		//jump
		GetComponent<JumpingObject> ().jump ();
	}

	public void landMiniJump(){
		if (miniJumped == false) {
			return;
		}
		else {
			//set it back to normal
			GetComponent<JumpingObject> ().jumpVelocity = originalJumpVelocity;
			GetComponent<JumpingObject> ().jumpHeight = originalJumpHeight;
			miniJumped = false;
		}
	}



	void kill (Collider other){
		if (killOnJump == true) {
			other.gameObject.GetComponent<AnimalBehavior> ().health--;
			if (other.gameObject.GetComponent<AnimalBehavior> ().health <= 0){
				Destroy (other.gameObject);
			}
		} else {	
			health--;
			healthGT.text = "Health: " + health;
		}
		miniJump();
	}

	void die (){
		livesCount--;
		livesGT.text = "Lives: " + livesCount.ToString();
		if (livesCount <= 0) {
			//go to game over scene
			Application.LoadLevel ("_Scene_0");
		}
	}
}
