using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that abstracts the Glide mechanic (an ability in the power, Air)
 * 
 **/
public class Glide {

	// The string paramter for the animator controller
	string animName;

	// The current time left until the player may cast another glide
	float cooldown;
	// The total time until the player may cast another glide
	float maxCooldown;
	bool onCooldown = false;

	// Determines when to release powerjump's velocity effects
	bool releaseJump = false;

	// The intitial cost of Glide
	int abilityPowerCost;
	// The cost of Glide every 1.0s of use
	int abilityPowerTickCost;

	// The current time between draining abilityPowerTickCost
	float abiliyPowerTickTime = 1f;
	// The max time between draining abilityPowerTickCost
	float tickDuration = 1;

	bool resetTimerTick = false;
	bool passedFirstTick = false;

	// A reference to powersManagerScript.cs
	PowersManager powersManagerScript;

	// The player's mesh
	Transform mesh;

	// Determines when to release powerjump's animation effects
	bool animationTransition1 = false;
	bool animationTransition2 = false;
	bool isFalling = false;

	// Stores whether the player has glided in the past .5s
	bool recentlyGlided = false;
	bool currentlyGliding = false;

	Animator anim;

	// A reference to the sole player
	GameObject player;

	public Glide(Animator anim, GameObject player, string animName, int abilityPowerCost, int abilityPowerTickCost, float cooldown, PowersManager powersManagerScript) {
		this.maxCooldown = cooldown;
		this.abilityPowerCost = abilityPowerCost;
		this.abilityPowerTickCost = abilityPowerTickCost;
		this.anim = anim;
		this.player = player;
		this.animName = animName;
		this.powersManagerScript = powersManagerScript;
		this.mesh = powersManagerScript.transform.parent.transform;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Checks that the conditions to glide are met, if so, sets up to glide
	 * 
	 * @param
	 * @return true when the player enters input to cast glide with sufficient materials,
	 * 		   false otherwise
	 * 
	 **/
	public bool toGlide() {
		if (Input.GetKey(KeyCode.R) && !onCooldown && player.GetComponent<Player>().canGlide() && player.GetComponent<Player>().AbilityPower >= abilityPowerCost && player.GetComponent<Player>().CurrentPower.Equals("Air")) {

			// Play the glide animation
			anim.SetBool(animName, true);

			// Glide costs 20 AP on initial cast and 5 AP for every 1.0s after that
			// If the player recently glided, do not charge them the initial cast
			if (!recentlyGlided) {
				player.GetComponent<Player>().AbilityPower -= abilityPowerCost;
			}
			recentlyGlided = true;

			onCooldown = true;
			cooldown = maxCooldown;

			return true;
		} else {
			return false;
		}
	}

	/**
	 * Called every frame by PowersManager.cs if the player has indicated toGlide()
	 * Perfroms the glide mechanic
	 * 
	 * @param
	 * @return true if the player has inputed to continue gliding,
	 * 		   false otherwise
	 * 
	 **/
	public bool gliding() {
		if (Input.GetKey(KeyCode.R)) {
			recentlyGlided = true;

			player.GetComponent<Player>().gravityEnabled(false);

			Vector3 tempVelocity = player.GetComponent<Player>().Velocity;
			tempVelocity.y = 0;
			player.GetComponent<Player>().Velocity = tempVelocity;

			return true;
		} else {

			// Player has no longer glided recently after 2s, so initial ability power cost should apply next cast
			System.Threading.Timer t = null;
			t = new System.Threading.Timer((obj) => {
				recentlyGlided = false;
				t.Dispose();
			}, null, 2000, System.Threading.Timeout.Infinite);

			player.GetComponent<Player>().gravityEnabled(true);

			// Stop gliding animation
			anim.SetBool(animName, false);
			onCooldown = true;
			passedFirstTick = false;

			return false;
		}
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Updates the cooldown of Glide
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

	/**
	 * Called every frame by PowersManager.cs if the player indicated toGlide()
	 * Drains ability power every second of glide usage
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void drainAbilityPower() {
		if (!recentlyGlided && !passedFirstTick) {
			tickDuration = 1f;
		}

		tickDuration -= Time.deltaTime;

		if (tickDuration <= 0) {
			tickDuration = 1;
			player.GetComponent<Player>().AbilityPower -= abilityPowerTickCost;
		}

		passedFirstTick = true;
	}

	/**
	 * @param
	 * @return whether the player is currently Gliding
	 * 
	 **/
	public bool isGliding() {
		return currentlyGliding;
	}
}
