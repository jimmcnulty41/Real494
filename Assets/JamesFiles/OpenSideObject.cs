using UnityEngine;
using System.Collections;

public class OpenSideObject : MonoBehaviour {
	public bool openSides;
	static bool openAllSidesWasCalled;

	void Awake(){
		openSides = true;
		openAllSidesWasCalled = false;
	}

	public void closeSides(){
		openAllSidesWasCalled = false;
		openSides = false;
	}

	static public void openAllSides(){
		openAllSidesWasCalled = true;
	}

	void Update(){
		if (openAllSidesWasCalled){
			print(gameObject.name);
			openSides = true;
		}
	}
}
