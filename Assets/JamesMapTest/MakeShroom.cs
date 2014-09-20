using UnityEngine;
using System.Collections;

public class MakeShroom : MonoBehaviour {

	public GameObject cubePrefab;
	public GameObject shroomContainer;
	public float xChange = .3f;
	public float minX = .5f;
	public float yChange = .15f;
	public bool ________________________;
	public float curYChange = 0;
	public float curXChange = 0;

	void Start(){
		GameObject iterativeCube = null;
		do {
			GameObject downShroom = null;
			if (iterativeCube){
				downShroom = iterativeCube;
			}
			iterativeCube = Instantiate(cubePrefab) as GameObject;
			iterativeCube.transform.position = shroomContainer.transform.position;
			iterativeCube.transform.parent = shroomContainer.transform;
			Vector3 pos = iterativeCube.transform.position;
			pos.y += getY();
			iterativeCube.transform.position = pos;
			Vector3 scale = iterativeCube.transform.localScale;
			scale.x -= getX();
			iterativeCube.transform.localScale = scale;
			ShroomCube sc = iterativeCube.GetComponent<ShroomCube>();
			//	Set info common to all shroomcubes
			sc.xDiff = xChange;
			sc.yDiff = yChange;
			sc.minX = minX;
			sc.baseloc = gameObject.transform.position;
			sc.baseScale = cubePrefab.transform.lossyScale;
			sc.setNum();

			//	Set up linked list of shrooms
			sc.downShroom = downShroom;
			if (sc.downShroom != null){
				sc.downShroom.GetComponent<ShroomCube>().upShroom = iterativeCube;
			}
			if (shroomContainer.transform.childCount > 1000) break;
		} while(iterativeCube.transform.localScale.x > minX);
	}

	float getY(){
		curYChange += yChange;
		return curYChange;
	}

	float getX(){
		curXChange += xChange;
		return curXChange;
	}

}
