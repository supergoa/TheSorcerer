using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script attached to Projectile prefabs that slow the hit Enemy
 * 
 **/
public class ApplyFreeze : MonoBehaviour {
	public float percentSlow;

	/**
	 * A game event triggers this method.
	 * Calls method ApplyFreeze(float percentSlow) found on Enemy GameObject
	 * 
	 * @param other holds the Collider of the GameObject (Enemy) which the Projectile hit.
	 * @return
	 * 
	 **/
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Enemy")) {
			
			// Links Projectile prefabs to GameObjects with script Enemy.cs
			other.GetComponent<Enemy>().ApplyFreeze(percentSlow);
			//transform.SendMessage("ApplyFreeze", percentSlow);
		}
	}
}
