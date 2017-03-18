using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that enables the pause menu during play
 * Script attaches to a GameObject that should never be destoryed such as the 'Main Camera' or a 'Game Manager'
 * 
 **/
public class PauseManager : MonoBehaviour {

	// The entire pause menu object
	public GameObject pauseMenu;
	// The player HUD needs to be disabled when paused
	public GameObject HUD;

	bool paused;

	/**
	 * Called every frame by update in this class
	 * 
	 * @param
	 * @return true if the player hits the ESC key during player to indicate a pause
	 *
	 **/
	bool toPause() {
		if (!paused) {
			return Input.GetKey(UnityEngine.KeyCode.Escape);
		}
		return false;
	}

	/**
	 * Called once if the player indicated toPause()
	 * 
	 * @param
	 * @return
	 *
	 **/
	void pause() {
		paused = true;

		// Freeze the game
		Time.timeScale = 0f;

		pauseMenu.SetActive(true);
		HUD.SetActive(false);
	}

	/**
	 * Called when the player selects 'close' on the pause menu
	 * 
	 * @param
	 * @return
	 *
	 **/
	public void close() {
		paused = false;

		// Unfreeze the game
		Time.timeScale = 1f;

		pauseMenu.SetActive(false);
		HUD.SetActive(true);
	}

	/**
	 * Called when the player selects 'restart' on the pause menu
	 * 
	 * @param
	 * @return
	 *
	 **/
	public void restart() {
		// Unfreeze the game
		Time.timeScale = 1f;
		MainMenuUIManager.launchGame(MainMenuUIManager.CurrentLevel);
	}

	/**
	 * Called when the player selects 'main menu' on the pause menu
	 * 
	 * @param
	 * @return
	 *
	 **/
	public void launchMainMenu () {
		// Unfreeze the game
		Time.timeScale = 1f;
		MainMenuUIManager.launchGame(MainMenuUIManager.MainMenu);
	}

	// Called every frame
	void Update() {
		if (toPause()) {
			pause();
		}
	}
}
