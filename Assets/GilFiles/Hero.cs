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
	string type;

	public float throwingDelay; //time it takes to initiate throwing
	public float betweenDelay; //time it takes between throws
	public float miniJumpHeight;
	public float miniJumpVelocity;	
	//GUI text:
	public GUIText livesGT;
	public GUIText typeGT;
	public GUIText healthGT;
	public GUIText keysGT;
	//	Sprites
	public Sprite nemoStd;
	public Sprite nemoFrog;
	public Sprite nemoLizard;
	public Sprite nemoLizardOnWall;
	public Sprite nemoSquirrel;
	public string nextLevel;

	static public bool ______________________;

	
	public int livesCount;
	int keysCount;
	public int keysMax;

	bool canThrow = true; //flag for enable/disable throw
	public bool immune = false;
	bool miniJumped;
	Vector3 nemoScale = new Vector3(.75f, 1f, 1f);
	Vector3 nemoSpriteScale = new Vector3(1.46f, 1f, 1f);

	
	float originalJumpHeight;
	float originalJumpVelocity;

	void Awake(){
		changeBack();
	}

	// Use this for initialization
	void Start () {

		//set the variables and GUI text:
		killOnJump = false;
		stickToWalls = false;
		//livesCount = 3;
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
		keysGT.text = "Keys: " + keysCount + "/" + keysMax;

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
		Vector3 pos = transform.position;
		pos.z = 0;
		transform.position = pos;
	}

	void manageInputs(){

		if (Input.GetKeyDown(KeyCode.RightShift)) changeBack();
		if (Input.GetKeyDown(KeyCode.Z)) throwCandy();
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
		//Vector3 animalPOS = other.gameObject.transform.position;
		//Material animalMAT = other.gameObject.renderer.material;
		Destroy (other.gameObject);
		//transfer it to Nemo
		type = other.gameObject.tag;
		typeGT.text = "Type: " + type;
		//transform.position = animalPOS;
		//	Set jumping params
		JumpingObject jo = GetComponent<JumpingObject>();
		jo.jumpHeight = animalJO.jumpHeight;
		jo.jumpVelocity = animalJO.jumpVelocity;
		FallingObject fo = GetComponent<FallingObject>();
		fo.fallSpeed = animalFO.fallSpeed;
		fo.transitionEasing = animalFO.transitionEasing;

		//	Change scale to match and make sure you're actually on the ground
		transform.localScale = other.transform.localScale;
		fo.fall();
		//renderer.material.color = animalMAT.color;
		//update special ability
		if (type == "Frog") {
			changeSprite("nemoFrog", other);
			killOnJump = true;
		} else if (type == "Lizard") {
			changeSprite("nemoLizard", other);
			GetComponent<HeroMovement>().runSpeed = 3;
			gameObject.AddComponent<SticksToWalls>();
		} else if (type == "Squirrel") {
			changeSprite("nemoSquirrel", other);
			gameObject.AddComponent<FlyingSquirrel>();
		}
		//set the new original jump:
		originalJumpHeight = GetComponent<JumpingObject>().jumpHeight;
		originalJumpVelocity = GetComponent<JumpingObject>().jumpVelocity;
		//can't throw while not nemo
		canThrow = false;
	}

	void changeBack (){
		SticksToWalls stw = GetComponent<SticksToWalls>();
		if (stw){
			stw.remove();
			Destroy(stw);
		}
		FlyingSquirrel fs = GetComponent<FlyingSquirrel>();
		if (fs) {
			Destroy(fs);
		}
		if (type == "Nemo")
			return;

		GetComponent<JumpingObject> ().jumpHeight = 1.2f;
		GetComponent<JumpingObject> ().jumpVelocity = 8.0f;
		GetComponent<FallingObject> ().fallSpeed = 24.0f;
		GetComponent<FallingObject> ().transitionEasing = 0.05f;
		type = "Nemo";
		typeGT.text = "Type: " + type;
		transform.localScale = nemoScale;
		changeSprite("nemoStd", collider);

		originalJumpHeight = GetComponent<JumpingObject>().jumpHeight;
		originalJumpVelocity = GetComponent<JumpingObject>().jumpVelocity;

		
		killOnJump = false;
		stickToWalls = false;
		dig = false;
		canThrow = true;
	}

	public void changeSprite(string spriteName, Collider other){
		Transform child = transform.GetChild(0);
		SpriteRenderer rend = child.GetComponent<SpriteRenderer>();
		if (spriteName == "nemoStd") rend.sprite = nemoStd;
		if (spriteName == "nemoFrog") rend.sprite = nemoFrog;
		if (spriteName == "nemoLizard") rend.sprite = nemoLizard;
		if (spriteName == "nemoLizardOnWall") rend.sprite = nemoLizardOnWall;
		if (spriteName == "nemoSquirrel") rend.sprite = nemoSquirrel;
		//	Adjust scale

		Vector3 scale = other.transform.GetChild(0).localScale;
		if (spriteName == "nemoStd") scale = nemoSpriteScale;
		if (GetComponent<HeroMovement>().facingLeft) scale.x = Mathf.Abs(scale.x) * -1;
		child.localScale = scale;

	}

	void OnTriggerStay(Collider other){
		OnTriggerEnter(other);
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
			if (heroBottom >= animalTop - GetComponent<CollisionDetector>().forgiveness) {
				kill (other);		
			} else {
				miniJump();
				animalFall(other);
				takeDamage();

			}
		}
		//if its not an animal, check if its a gui object and interact accordingly
		else if (other.gameObject.tag == "Life") {
			livesCount++;
			livesGT.text = "Lives: " + livesCount.ToString ();
			Destroy (other.gameObject);

		} else if (other.gameObject.tag == "Key") {
			keysCount++;
			keysGT.text = "Keys: " + keysCount + "/" + keysMax;
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Health") {
			print (health);
			if (health < 4) {
				health = 4;
				print (health);
			}
			print (health);
			healthGT.text = "Health: " + health;
			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Door") {
			if (keysCount >= keysMax){
				Application.LoadLevel (nextLevel);
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

	void animalFall(Collider other){
		if (other.tag == "Bee") return;
		other.GetComponent<FallingObject> ().falling = true;
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
			takeDamage ();
		}
		miniJump();
	}
	

	int HeroLayer = 11;
	int ImmuneLayer = 13;

	IEnumerator Immune() {
		immune = true;
		gameObject.layer = ImmuneLayer;
		//gameObject.AddComponent<OpenSideObject> ();
		for (int i = 0; i < 7; ++i){
			SpriteRenderer rend = GetComponentInChildren<SpriteRenderer>();
			Color c = rend.color;
			c.a = .4f;
			rend.color = c;
			yield return new WaitForSeconds(0.25f);
			gameObject.layer = HeroLayer;
			c = rend.color;
			c.a = 1f;
			rend.color = c;
		}
		//OpenSideObject oso = GetComponent<OpenSideObject> ();
		//Destroy (oso);
		immune = false;

	}

	void takeDamage (){
		if (immune == true) {
			return;
		}

		health--;
		healthGT.text = "Health: " + health;
		if (health <= 0) {
			die ();
		}

		StartCoroutine (Immune ());
	}

	public void die (){
		livesCount--;

		if (livesCount <= 0) {
			//go to game over scene
			Application.LoadLevel ("_Scene_0");
		}

		livesGT.text = "Lives: " + livesCount.ToString();
		health = 3;
		healthGT.text = "Health: " + health;
		Vector3 savePoint = GetComponent<SavePoint> ().savePoint;
		savePoint.z = 0;
		transform.position = savePoint;

		changeBack ();
		GetComponent<FallingObject>().fall();
	}
}
