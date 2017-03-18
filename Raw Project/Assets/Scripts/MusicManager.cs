using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that maintains one instance of background music between scenes
 * 
 **/
public class MusicManager : MonoBehaviour {

	//
	// Singleton pattern
	//

	private static MusicManager instance = null;

	public static MusicManager Instance {
		get { 
			return instance; 
		}
	}
		
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
