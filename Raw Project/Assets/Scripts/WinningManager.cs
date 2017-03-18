using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that checks if a player has won the level (collected all the coins)
 * 
 **/
public class WinningManager : MonoBehaviour {

	public Text coinDisplay;
	public static int coinCount;
	private static bool won = false;
	private float loadTime = 5f;

	void Start () {
		loadTime = 5f;
		won = false;

		// Find count of all coins in a scene
		coinCount = GameObject.FindGameObjectsWithTag("Coin").Length;
	}
	 
	/**
	 * A game event triggers this method.
	 * Called when a coin is collected (destroyed)
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public static void updateCoinCount () {
		// Find count of all coins in a scene
		coinCount = GameObject.FindGameObjectsWithTag("Coin").Length;
		if (coinCount <= 0) {
			won = true;
		} else {
			won = false;
		}
	}

	/**
	 * Called every frame.
	 * Updates the HUD display of coins left to collect
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void updateDisplay(int coinCount) {
		coinDisplay.text = "" + coinCount;
	}

	/**
	 * Checks, every frame, if the player has met the conditions to win
	 * 
	 * @param
	 * @return
	 * 
	 **/
	void Update() {
		updateDisplay(coinCount);
		if (won) {
			MainMenuUIManager.launchGame(MainMenuUIManager.WinMenu);
			won = false;
		}
		loadTime -= Time.deltaTime;
	}
}
