using UnityEngine;
using System.Collections;

public class KillPoint : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag != "Hero") return;
		other.GetComponent<Hero>().die();
		Transform child = transform.GetChild(0);
		other.transform.position = child.position;
	}
}
