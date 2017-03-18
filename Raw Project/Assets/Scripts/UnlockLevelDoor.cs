using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that unlocks the door at the end of the level
 * 
 **/
public class UnlockLevelDoor : MonoBehaviour {
	public GameObject finalBoos;
	public GameObject levelDoor;

	void FixedUpdate() {
		if (finalBoos == null) {
			levelDoor.transform.Translate(new Vector3(-.05f, .05f, 0f));
		}
	}
}
