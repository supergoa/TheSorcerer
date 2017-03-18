using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that abstracts all projectile mechanics (2 abilities in the power, Fire -- 1 ability in the power, Water)
 * 
 **/
public class ProjectileAbility {

	// The Prefab to fire
	GameObject projectile;

	// The string paramter for the animator controller
	string animName;

	// The speed the projectile flies at
	float speed;

	// The amount of seconds before the projectile despawns
	float shotLifespan;

	// The current time left until the player may cast another projectile
	float cooldown;
	// The total time until the player may cast another projectile
	float maxCooldown;
	bool onCooldown = false;

	// Determines when to release the actual projectile (not startup animations)
	bool releaseProjectile = false;
	// The time between the animation start and actual release of the projectile
	int delay;

	// The cost to cast this projectile
	int abilityPowerCost;

	Vector3 spawnOffset;

	// A reference to powersManagerScript.cs
	PowersManager powersManagerScript;

	// A reference to the player's animator
	Animator anim;

	// A reference to the sole player
	GameObject player;

	public ProjectileAbility(Animator anim, GameObject player, GameObject projectile, float speed, float shotLifespan, int abilityPowerCost, Vector3 spawnOffset, string animName, int delay, float cooldown, PowersManager shoot){
		this.anim = anim;
		this.player = player;
		this.projectile = projectile;
		this.speed = speed;
		this.shotLifespan = shotLifespan;
		this.abilityPowerCost = abilityPowerCost;
		this.spawnOffset = spawnOffset;
		this.animName = animName;
		this.delay = delay;
		this.cooldown = cooldown;
		this.maxCooldown = cooldown;
		this.powersManagerScript = shoot;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Checks that the conditions to shoot are met, if so, sets up to shoot
	 * 
	 * @param
	 * @return true when the player enters input to shoot with sufficient materials
	 * 		   false otherwise
	 * 
	 **/
	public bool toShoot() {
		if (anim.GetBool(animName) && !onCooldown && player.GetComponent<Player>().AbilityPower >= abilityPowerCost) {
			return true;
		} else {
			return false;
		}
	}

	/**
	 * Called once by PowersManager.cs if toHeal()
	 * Starts animation to shoot
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void prepareToFire() {
		System.Threading.Timer timer = null;
		timer = new System.Threading.Timer((obj) => {
			releaseProjectile = true;
			timer.Dispose();
		}, null, delay, System.Threading.Timeout.Infinite);

		player.GetComponent<Player>().AbilityPower -= abilityPowerCost;
		onCooldown = true;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * 
	 * @param
	 * @return releaseHeal true if @Instance_var delay time has passed since initial cast time
	 * 
	 **/
	public bool readyToFire() {
		return releaseProjectile;
	}
		
	/**
	 * Called once by PowersManager.cs if readyToFire()
	 * Fires the @Instance_var projectile prefab at @Instance_var spawnOffset and is destoryed after @Instance_var shotLifespan
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void fire() {
		Vector3 location = powersManagerScript.transform.position;
		GameObject temp = (GameObject) UnityEngine.Object.Instantiate(projectile, location+spawnOffset, Quaternion.identity);


		Transform mesh = player.transform.GetChild(0).transform;
		if (mesh.rotation == Quaternion.Euler(0, 90, 0)) { // If the player is facing right, shoot right
			temp.GetComponent<Rigidbody>().AddForce(Vector3.right * speed);
		} else if (mesh.rotation == Quaternion.Euler(0, -90, 0)) {  // If the player is facing left, shoot left
			temp.transform.rotation = Quaternion.Euler(0, 0, -180);
			temp.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
		}

		UnityEngine.Object.Destroy(temp, shotLifespan);
		releaseProjectile = false;
	}
		
	
	/**
	 * Called every frame by PowersManager.cs
	 * Updates the cooldown of projectile
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void updateCooldown () {
		if (onCooldown) {
			cooldown -= Time.deltaTime;
			if (cooldown <= 0) {
				onCooldown = false;
				cooldown = maxCooldown;
			}
		}
	}
}
