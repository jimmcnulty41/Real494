using UnityEngine;
using System.Collections;

public class MakeShroom : MonoBehaviour {

	public GameObject shroomCubePrefab;
	public GameObject shroomBasePrefab;
	public GameObject shroomContainer;
	public float xChange = .3f;
	public float minX = .5f;
	public float yChange = .15f;
	public float width = 4f;
	public float height = .3f;
	public bool ________________________;
	public float curYChange = 0;
	public float curXChange = 0;
	public Vector3 vLeft;
	public Vector3 vRight;
	public Vector3 vLeftNormX;
	public Vector3 vRightNormX;
	public Vector3 bottomLeftCorner;
	public Vector3 bottomRightCorner;
	public Vector3 topRightCorner;
	public Vector3 topLeftCorner;

	void Start(){
		//	Set up the prefab
		changeScale();
		calculateBoundInformation();
		createBaseCube();
		GameObject iterativeCube = null;
		do {
			GameObject downShroom = null;
			if (iterativeCube){
				downShroom = iterativeCube;
			}
			iterativeCube = getInitialCube();
			changeCubeSizeAndPos(iterativeCube);
			ShroomCube sc = iterativeCube.GetComponent<ShroomCube>();
			//	Set info common to all shroomcubes
			sc.xDiff = xChange;
			sc.yDiff = yChange;
			sc.minX = minX;
			sc.baseloc = gameObject.transform.position;
			sc.baseScale = shroomCubePrefab.transform.lossyScale;
			sc.setNum();

			//	Set up linked list of shrooms
			sc.downShroom = downShroom;
			if (sc.downShroom != null){
				sc.downShroom.GetComponent<ShroomCube>().upShroom = iterativeCube;
			}
			if (shroomContainer.transform.childCount > 1000) break;
		} while(iterativeCube.transform.localScale.x > minX);
	}

	void createBaseCube(){
		GameObject baseCube = Instantiate(shroomBasePrefab) as GameObject;
		Vector3 scale = baseCube.transform.localScale;
		scale.y = 2 * height;
		baseCube.transform.localScale = scale;

		Vector3 pos = transform.position;
		//pos.y -= height / 2;
		baseCube.transform.position = pos;
		//baseCube.transform.parent = shroomContainer.transform;
	}

	//	For creating individual cubes
	//============================================

	void changeScale(){
		Vector3 startScale = shroomCubePrefab.transform.localScale;
		startScale.x = width;
		startScale.y = height;
		shroomCubePrefab.transform.localScale = startScale;
		shroomBasePrefab.transform.localScale = startScale;
	}

	GameObject getInitialCube(){
		GameObject iterativeCube = Instantiate(shroomCubePrefab) as GameObject;
		iterativeCube.transform.position = shroomContainer.transform.position;
		iterativeCube.transform.parent = shroomContainer.transform;
		return iterativeCube;
	}

	void changeCubeSizeAndPos(GameObject iterativeCube){
		Vector3 pos = iterativeCube.transform.position;
		pos.y += getY();
		iterativeCube.transform.position = pos;
		Vector3 scale = iterativeCube.transform.localScale;
		scale.x -= getX();
		iterativeCube.transform.localScale = scale;
	}

	float getY(){
		curYChange += yChange;
		return curYChange;
	}
	
	float getX(){
		curXChange += xChange;
		return curXChange;
	}

	//	Information about the shroom as a whole
	//=================================================

	void calculateBoundInformation(){
		//	Get the bottom corners
		bottomLeftCorner = transform.position;
		bottomLeftCorner.x -= width / 2;
		bottomLeftCorner.y += height / 2;
		bottomLeftCorner.z = 0;
		bottomRightCorner = transform.position;
		bottomRightCorner.x += width / 2;
		bottomRightCorner.y += height / 2;
		bottomRightCorner.z = 0;

		//	Find the number of cubes that will be created on top of the base
		float numberOfCubes = ((width - minX) / xChange);
		//	Use that to figure out the slope. For the right side it will have a negative x
		vLeft = Vector3.zero;
		vLeft.x = (width - minX) /2;
		vLeft.y = yChange * numberOfCubes;
		vRight = vLeft;
		vRight.x *= -1;

		vLeftNormX = vLeft / vLeft.x;
		vRightNormX = vRight / vRight.x;

		//	Now we can find the topCorners
		topLeftCorner = bottomLeftCorner + vLeft;
		topRightCorner = bottomRightCorner + vRight;

//		LineRenderer lr = GetComponent<LineRenderer>();
//		lr.SetPosition(0, bottomLeftCorner);
//		lr.SetPosition(1, topLeftCorner);
//		lr.SetPosition(2, topRightCorner);
//		lr.SetPosition(3, bottomRightCorner);
	
	}

	public Vector3 getCorrectPos(GameObject mover){
		float minBound = mover.collider.bounds.min.x;
		Vector3 pos = mover.transform.position;
		if (minBound <= bottomRightCorner.x && minBound >= topRightCorner.x)
		{	//	On right slope
			float difference = minBound - bottomRightCorner.x ;
			pos = bottomRightCorner;
			pos.y +=  ((vRight.y * difference) / vRight.x);
			pos.x = mover.transform.position.x;
			pos.y += mover.collider.bounds.extents.y;
		}
		float maxBound = mover.collider.bounds.max.x;
		if (maxBound >= bottomLeftCorner.x && maxBound <= topLeftCorner.x)
		{	//	On left slope
			float difference = maxBound - bottomLeftCorner.x;
			pos =  bottomLeftCorner;
			pos.y += ((vLeft.y * difference) / vLeft.x);
			pos.x = mover.transform.position.x;
			pos.y += mover.collider.bounds.extents.y;
		}
		if (maxBound >= topLeftCorner.x && minBound <= topRightCorner.x)
		{	//	In the middle
			pos = topLeftCorner;
			pos.x = mover.transform.position.x;
			pos.y += mover.collider.bounds.extents.y;
		}
		return pos;
	}


	public bool offShroom(GameObject go){
		return (go.collider.bounds.max.x < bottomLeftCorner.x ||
		        go.collider.bounds.min.x > bottomRightCorner.x);
	}

}
