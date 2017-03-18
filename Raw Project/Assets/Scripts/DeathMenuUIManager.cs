using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * UI Script that listens for input on the Death Menu
 * 
 **/
public class DeathMenuUIManager : MonoBehaviour {
	/**
	 * A button event triggers this method.
	 * Launches the main menu
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void launchMainMenu () {
		MainMenuUIManager.launchGame(MainMenuUIManager.MainMenu);
	}

	/**
	 * A button event triggers this method.
	 * Restarts the current level
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void relauchCurrentLevel() {
		MainMenuUIManager.launchGame(MainMenuUIManager.CurrentLevel);
	}
}