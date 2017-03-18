using UnityEngine;
using System.Collections;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that controlls usage of ALL abilities from ALL powers
 * 
 **/
public class PowersManager : MonoBehaviour {
	
	public GameObject fireballPrefab;
	public GameObject firewavePrefab;
	public GameObject iceballPrefab;

	// The fireball ability
	ProjectileAbility fireball;
	// The firewave ability
	ProjectileAbility firewave;
	// The iceball ability
	ProjectileAbility iceball;
	ProjectileAbility[] projectileAbilities;

	// The heal ability
	Heal heal;

	// The powerjump ability
	Powerjump powerjump;

	// The glide ability
	Glide glide;
	bool isGliding = false;

	// A reference to the player's animator
	public Animator anim;
	// A reference to the sole player
	public GameObject player;

	// Initialize all powers with their properties (i.e. speed, ability power cost, etc...)
	void Start() {
		player = transform.parent.parent.gameObject;

		fireball = new ProjectileAbility(anim, player, fireballPrefab, 13f, 2f, 20, new Vector3(.25f, 1f, 0f), "isFireball", 1400, 2.583f, this);
		firewave = new ProjectileAbility(anim, player, firewavePrefab, 13f, 5f, 50, new Vector3(1f, 1f, 0f), "isFirewave", 1110, 2.583f, this);
		iceball = new ProjectileAbility(anim, player, iceballPrefab, 13f, 2f, 20, new Vector3(.25f, 1f, 0f), "isIceball", 1400, 2.583f, this);
		projectileAbilities = new ProjectileAbility[] {fireball, firewave, iceball};

		powerjump = new Powerjump(anim, player, "isPowerjump", 25, 70f, 3f, this);
		glide = new Glide(anim, player, "isGliding", 20, 5, .5f, this);
		heal = new Heal(anim, player, "isHeal", 25, 35, 2f, this);
	}

	public ProjectileAbility getFireballRef(){
		return this.fireball;
	}
	public ProjectileAbility getFirewaveRef(){
		return this.firewave;
	}
	public Powerjump getPowerjumpRef(){
		return this.powerjump;
	}
	public Glide getGlideRef(){
		return this.glide;
	}

	// See each ability for documentation on their uses here
	void Update () {
		// All projectiles
		foreach(ProjectileAbility p in projectileAbilities) {
			if(p.toShoot()) {
				p.prepareToFire();
			}

			if (p.readyToFire()) {
				p.fire();
			}
			p.updateCooldown();
		}

		// Powerjump
		if (powerjump.toJump()) {
			powerjump.prepareToJump();
		}
		if (powerjump.readyToJump()) {
			powerjump.jump();
		}
		powerjump.updateAnimations();
		powerjump.updateCooldown();


		// Glide
		if (glide.toGlide()) {
			isGliding = true;
		}
		if (isGliding) {
			glide.drainAbilityPower();
			if (!glide.gliding()) {
				isGliding = false;
			}
		}
		glide.updateCooldown();

		// Heal
		if (heal.toHeal()) {
			heal.prepareToHeal();
		}
		if (heal.readyToHeal()) {
			heal.heal();
		}
		heal.updateCooldown();
		heal.updateParticles();
	}

}