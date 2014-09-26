using UnityEngine;
using System.Collections;

public class StickyWalls : MonoBehaviour {
	public bool allowLeft;
	public bool allowRight;

	public bool allows(relationToOther relation){
		if (relation == relationToOther.TOLEFT){
			return allowLeft;
		}
		if (relation == relationToOther.TORIGHT){
			return allowRight;
		}
		return true;
	}
}
