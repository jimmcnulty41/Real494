using UnityEngine;
using System.Collections;

public enum characterType {
	nemo,
	frog,
	mole,
	lizard
};

public class Hero : MonoBehaviour {

	public GameObject Candy; //the game object to create

	public bool killOnJump;
	public bool stickToWalls;

	public float throwingDelay; //time it takes to initiate throwing
	public float betweenDelay; //time it takes between throws

	static public bool ______________________;

	//GUI text:
	public GUIText livesGT;
	public GUIText typeGT;
	public GUIText lifeGT;
	public GUIText keysGT;

	int livesCount;
	int keysCount;
	characterType type;

	bool canThrow = true; //flag for enable/disable throw

	// Use this for initialization
	void Start () {
		//set the variables and GUI text:
		killOnJump = false;
		stickToWalls = false;
		livesCount = 3;
		keysCount = 0;
		type = characterType.nemo;

		GameObject livesGO = GameObject.Find ("lives");
		GameObject keysGO = GameObject.Find ("keys");
		GameObject typeGO = GameObject.Find ("type");

		livesGT = livesGO.GetComponent<GUIText> ();
		livesGT.text = "Lives: 3";

		keysGT = keysGO.GetComponent<GUIText> ();
		keysGT.text = "Keys: 0";

		typeGT = typeGO.GetComponent<GUIText> ();
		typeGT.text = "Type: Nemo";
	}
	
	// Update is called once per frame
	void Update () {
		manageInputs();
	}

	void manageInputs(){

		if (Input.GetKeyDown(KeyCode.Q)) die();
		if (Input.GetKeyDown(KeyCode.W)) changeBack();
		if (Input.GetKeyDown(KeyCode.C)) throwCandy();
//		if (Input.GetKeyDown(KeyCode.R)) jump();
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
		print (transform.lossyScale.y / 2);
		if (canThrow) {

			StartCoroutine (Throw ());
		}

//		GameObject candy = Instantiate (Candy) as GameObject;
//		candy.transform.position = transform.position;
//		/*float startingPoint = transform.position.y + GetComponent<CollisionDetector>().vertHalfWidth(this);
//		Vector3 destination = transform.position;
//		destination.y = startingPoint;
//		transform.position = destination;*/
//		candy.GetComponent<JumpingObject>().jump();
//
//		//delay the time between throws
//		StartCoroutine(Wait(betweenDelay));


	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Frog") {
			changeType (other);
			kill (other);
		}
		else if ((other.gameObject.tag == "Key") || (other.gameObject.tag == "Life")) pickup (other);
	}

	void kill (Collider other){
		if (killOnJump == true) {
			Destroy (other.gameObject);
		}
	}

	void pickup (Collider other){
		if (other.gameObject.tag == "Life") {
			livesCount++;
			livesGT.text = "Lives: " + livesCount.ToString();
		} else if (other.gameObject.tag == "Key") {
			keysCount++;
			keysGT.text = "Keys: " + keysCount.ToString();
		} else if (other.gameObject.tag == "Health") {
			//increment the health to <= 4
		}
		kill (other);
	}

	void changeType (Collider other){
		if (other.gameObject.tag == "Frog") {
			type = characterType.frog;
			//set other attributes here
			typeGT.text = "Type: Frog";
		} else if (other.gameObject.tag == "Mole") {
			type = characterType.mole;
			//set other attributes here
			typeGT.text = "Type: Mole";
		} else if (other.gameObject.tag == "Lizard") {
			type = characterType.lizard;
			//set other attributes here
			typeGT.text = "Type: Lizard";
		}
	}

	void changeBack (){
		if (type == characterType.nemo) {
			return;
		} 
		else {
			type = characterType.nemo;
			typeGT.text = "Type: Nemo";
		}
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
