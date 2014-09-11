using UnityEngine;
using System.Collections;

public enum characterType {
	nemo,
	frog,
	mole,
	lizard
};

public class Hero : MonoBehaviour {

	public bool killOnJump;
	public bool stickToWalls;

	public int livesCount;
	public int keysCount;
	public characterType type;

	static public bool ______________________;

	public GUIText livesGT;
	public GUIText typeGT;
	public GUIText lifeGT;
	public GUIText keysGT;

	// Use this for initialization
	void Start () {
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
//		if (Input.GetKeyDown(KeyCode.E)) jump();
//		if (Input.GetKeyDown(KeyCode.R)) jump();
	}

	void OnTriggerEnter(Collider other){
		//checkSideMovement(other);
		print ("enter");
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
			Application.LoadLevel ("_Scene_5");
		}
	}
}
