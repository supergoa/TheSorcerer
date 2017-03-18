using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script attached to Coin prefabs
 * 
 **/
public class CollectCoins : MonoBehaviour {

	/**
	 * A game event triggers this method.
	 * When the player enters the coin collider, pick it up (destroy it)
	 * 
	 * @param other holds the Collider of the GameObject (Player) which the coin came in contact with
	 * @return
	 * 
	 **/
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Player")) {
			Destroy(transform.gameObject, 0f);
		}
	}

	/**
	 * A game event triggers this method.
	 * Upon destruction, update the amount of coins left to collect
	 * 
	 * @param
	 * @return
	 * 
	 **/
	void OnDestroy() {
		WinningManager.updateCoinCount();
	}
}
