using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that attaches to Abilitypower_Pick_Up prefabs
 * 
 **/
public class AbilitypowerPickup : MonoBehaviour {

	public int abilitypowerAmount;

	/**
	 * A game event triggers this method.
	 * When the player enters the Health_Pick_Up collider, pick it up (destroy it) and give AP to the player
	 * 
	 * @param other holds the Collider of the GameObject (Player) which the Abilitypower_Pick_Up came in contact with
	 * @return
	 * 
	 **/
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Player")) {
			if (other.transform.parent.gameObject.GetComponent<Player>().ApplyAbilitypowerpack(abilitypowerAmount)) {
				Destroy(transform.gameObject);
			}
		}
	}
}
