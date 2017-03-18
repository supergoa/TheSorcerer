using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that loads the Death menu should a player manage to get out of bounds
 * 
 * -- CURRENTLY NOT IN USE --
 * 
 **/
public class OutOfBounds : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Player")) {
			SceneManager.LoadScene(MainMenuUIManager.DeathMenu);
		}
	}
}
