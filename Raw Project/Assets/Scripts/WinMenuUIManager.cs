using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * UI Script that listens for input on the Win Menu
 * 
 **/
public class WinMenuUIManager : MonoBehaviour {

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
	public void lauchNextLevel() {
		MainMenuUIManager.launchGame(MainMenuUIManager.NextLevel);
	}
}