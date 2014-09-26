using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour {
	public Vector3 savePoint;

	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != "Savepoint") {
			return;
		}
		savePoint = other.gameObject.transform.position;
	}

}
