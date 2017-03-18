using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that abstracts the powerjump mechanic (an ability in the power, Air)
 * 
 **/
public class Powerjump {

	// The string paramter for the animator controller
	string animName;

	// The force with which to powerjump
	float powerJumpForce;

	// The current time left until the player may cast another powerjump
	float cooldown; 
	// The total time until the player may cast another powerjump
	float maxCooldown;
	bool onCooldown = false;

	// Determines when to release powerjumps's effects (not startup animations)
	bool releaseJump = false;

	// The cost of powerjump
	int abilityPowerCost;

	// A reference to powersManagerScript.cs
	PowersManager powersManagerScript;

	// A reference to the sole player
	GameObject player;
	// The player's mesh
	Transform mesh;
	// A reference to the player's animator
	Animator anim;

	//
	// A powerjump requires a series of animations
	// @Instance_var animationTransition1 marks the transition between the first and second animation
	// @Instance_var animationTransition2 marks the transition between the second and third animation
	//
	bool animationTransition1 = false;
	bool animationTransition2 = false;
	int delayTransition1;
	int delayTransition2;

	bool isFalling = false;

	public Powerjump(Animator anim, GameObject player, string animName, int abilityPowerCost, float powerJumpForce, float cooldown, PowersManager powersManagerScript) {
		this.powerJumpForce = powerJumpForce;
		this.maxCooldown = cooldown;
		this.abilityPowerCost = abilityPowerCost;
		this.anim = anim;
		this.player = player;
		this.animName = animName;
		this.powersManagerScript = powersManagerScript;
		this.mesh = powersManagerScript.transform.parent.transform;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Checks that the conditions to powerjump are met, if so, sets up to powerjump
	 * 
	 * @param
	 * @return true when the player enters input to cast a powerjump with sufficient materials
	 * 		   false otherwise
	 * 
	 **/
	public bool toJump() {
		if (Input.GetKey(KeyCode.E) && !onCooldown && player.GetComponent<Player>().AbilityPower >= abilityPowerCost && !player.GetComponent<Player>().performingAbility() && player.GetComponent<Player>().CurrentPower.Equals("Air")) {

			// Play the first powerjump animation
			anim.SetBool("isPowerjump_1", true);
			player.GetComponent<Player>().AbilityPower -= abilityPowerCost;
			onCooldown = true;
			cooldown = maxCooldown;

			return true;
		} else {
			return false;
		}
	}

	/**
	 * Called once by PowersManager.cs if toJump()
	 * Starts the second and third powerjump animations
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void prepareToJump() {
		
		//release jump after 1.1s
		System.Threading.Timer timer2 = null;
		timer2 = new System.Threading.Timer((obj) => {

			releaseJump = true;
			timer2.Dispose();

		}, null, 1100, System.Threading.Timeout.Infinite);

		//switch animations from crouch to start jumping at 0.5s
		System.Threading.Timer timer = null;
		timer = new System.Threading.Timer((obj) => {
			
			//releaseJump = true;
			animationTransition1 = true;

			timer.Dispose();
		}, null, 500, System.Threading.Timeout.Infinite);
			

		//start fall animation 1.5s
		System.Threading.Timer timer1 = null;
		timer1 = new System.Threading.Timer((obj) => {

			animationTransition2 = true;
			timer1.Dispose();

		}, null, 1500, System.Threading.Timeout.Infinite);
	}

	/**
	 * Called every frame by PowersManager.cs
	 * 
	 * @param
	 * @return releaseHeal true if animations one and two have completed
	 * 
	 **/
	public bool readyToJump() {
		return releaseJump;
	}

	/**
	 * Called once by PowersManager.cs if readyToJump()
	 * Performs a powerjump
	 * @param
	 * @return
	 * 
	 **/
	public void jump() {
		Vector3 tempVelocity = player.GetComponent<Player>().Velocity;
		tempVelocity.y += powerJumpForce;
		player.GetComponent<Player>().Velocity = tempVelocity;

		releaseJump = false;
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Resets animations for next use
	 * @param
	 * @return
	 *
	 **/
	public void updateAnimations(){
		if (animationTransition1) {
			anim.SetBool("isPowerjump_2", true);
			anim.SetBool("isPowerjump_1", false);
			animationTransition1 = false;
		}
		if (animationTransition2) {
			anim.SetBool("isFalling", true);
			anim.SetBool("isPowerjump_2", false);
			animationTransition2 = false;
			isFalling = true;
		}
	}

	/**
	 * Called every frame by PowersManager.cs
	 * Updates the cooldown of powerjump
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
