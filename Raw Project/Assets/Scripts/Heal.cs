using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that abstracts the Heal mechanic (an ability in the power, Water)
 * 
 **/
public class Heal {

	// The string paramter for the animator controller
	string animName;

	// The current time left until the player may cast another heal
	float cooldown;
	// The total time until the player may cast another heal
	float maxCooldown;
	bool onCooldown = false;

	// The amount to heal the player by
	int healAmount;

	// Determines when to release heal's effects (not startup animations)
	bool releaseHeal = false;
	// Determines when to release heal's animation effects
	bool releaseParticles = false;

	// The cost of heal
	int abilityPowerCost;

	// A reference to powersManagerScript.cs
	PowersManager powersManagerScript;

	// A reference to the player's animator
	Animator anim;

	// A reference to the sole player
	GameObject player;

	public Heal(Animator anim, GameObject player, string animName, int abilityPowerCost, int healAmount, float cooldown, PowersManager powersManagerScript) {
		this.maxCooldown = cooldown;
		this.abilityPowerCost = abilityPowerCost;
		this.healAmount = healAmount;
		this.anim = anim;
		this.player = player;
		this.animName = animName;
		this.powersManagerScript = powersManagerScript;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Checks that the conditions to heal are met, if so, sets up to heal
	 * 
	 * @param
	 * @return true when the player enters input to cast a heal with sufficient materials
	 * 		   false otherwise
	 * 
	 **/
	public bool toHeal() {
		if (Input.GetKey(KeyCode.R) && !onCooldown && player.GetComponent<Player>().AbilityPower >= abilityPowerCost && player.GetComponent<Player>().CurrentPower.Equals("Water")) {

			// Play the heal animation
			anim.SetBool(animName, true);
			cooldown = maxCooldown;
			return true;
		} else {
			return false;
		}
	}

	/**
	 * Called once by PowersManager.cs if toHeal()
	 * Starts animation and particle effects for heal
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void prepareToHeal() {
		
		// Releases actual heal after 1.8s
		System.Threading.Timer timer = null;
		timer = new System.Threading.Timer((obj) => {
			releaseHeal = true;
			timer.Dispose();
		}, null, 1800, System.Threading.Timeout.Infinite);

		// Releases heal particles after 1.0s
		System.Threading.Timer timer2 = null;
		timer2 = new System.Threading.Timer((obj) => {
			releaseParticles = true;
			timer2.Dispose();
		}, null, 1000, System.Threading.Timeout.Infinite);

		player.GetComponent<Player>().AbilityPower -= abilityPowerCost;
		onCooldown = true;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Plays heal particles
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void updateParticles() {
		if (releaseParticles) {
			player.transform.GetChild(3).GetChild(0).GetComponent<ParticleSystem>().Play();
			player.transform.GetChild(3).GetChild(1).GetComponent<ParticleSystem>().Play();
			releaseParticles = false;
		}
	}

	/**
	 * Called every frame by PowersManager.cs
	 * 
	 * @param
	 * @return releaseHeal true if 1.0s has passed since initial cast time
	 * 
	 **/
	public bool readyToHeal() {
		return releaseHeal;
	}

	/**
	 * Called once by PowersManager.cs if readyToHeal()
	 * Heals the player by healAmount
	 * @param
	 * @return
	 * 
	 **/
	public void heal(){
		// No longer healing, disable animation
		anim.SetBool(animName, false);
		player.GetComponent<Player>().ApplyHeal(healAmount);
		releaseHeal = false;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Updates the cooldown of Heal
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
