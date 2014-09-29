using UnityEngine;
using System.Collections;

public class GotoScene : MonoBehaviour {

	public string sceneName;

	void OnMouseDown () {
		Application.LoadLevel(sceneName);	
	}
}
