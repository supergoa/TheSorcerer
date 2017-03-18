using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script attached to Projectile prefabs that damage the hit Enemy
 * 
 **/
public class DealDamage : MonoBehaviour {

	// How much damage the Projectile will deal
	public int projectileDamage;

	// If the Projectile will be destroyed on impact
	public bool destroyedOnImpact;

	/**
	 * A game event triggers this method.
	 * Calls method ApplyDamage(int damage) found on Enemy GameObject
	 * 
	 * @param other holds the Collider of the GameObject (Enemy) which the Projectile hit.
	 * @return
	 * 
	 **/
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Enemy")) {
			
			// Links Projectile prefabs to GameObjects with script Enemy.cs
			other.GetComponent<Enemy>().ApplyDamage(projectileDamage);

			if (destroyedOnImpact) {
				Destroy(transform.gameObject, 0f);
			}
		}
	}
}
