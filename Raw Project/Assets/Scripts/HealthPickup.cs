using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that attaches to Health_Pick_Up prefabs
 * 
 **/
public class HealthPickup : MonoBehaviour {

	// The amount to heal the player by
	public int healAmount;

	/**
	 * A game event triggers this method.
	 * When the player enters the Health_Pick_Up collider, pick it up (destroy it) and heal the player
	 * 
	 * @param other holds the Collider of the GameObject (Player) which the Health_Pick_Up came in contact with
	 * @return
	 * 
	 **/
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Player")) {
			if (other.transform.parent.gameObject.GetComponent<Player>().ApplyHeal(healAmount)) {
				Destroy(transform.gameObject);
			}
		}
	}
}
