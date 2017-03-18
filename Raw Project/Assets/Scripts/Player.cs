using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float jumpHeight;
	public float timeToJumpApex;
	float accelerationTimeAirborne = .3f;
	float accelerationTimeGrounded = .25f;

	// The velocity of the player
	Vector3 velocity;

	bool jumpPermitted = true;
	bool willJump = false;

	float walkSpeed = 3f;
	float runMultiplier = 2f;

	bool useGravity = true;

	bool deathTransition = false;
	bool hasDied = false;

	// Dictates which abilities can be used based on the current power
	string currentPower = "Fire";

	//
	// Unimplemented mechanic
	//

	//float fallingTime = 0;
	//bool isFalling = false;

	float gravity;
	float jumpVelocity;
	float velocityZSmoothing;

	// The maximum health the player can have
	public const int maxHealth = 200;
	// The current health the player has
	int health = maxHealth;

	// The maximum ability power the player can have
	public const int maxAbilitypower = 200;
	// The current ability power the player has
	public int abilitypower = 200;

	// Wrapper for @instance_var healthAndResourceManager
	public GameObject HARM;

	// Referece to the HUD
	public static GameObject healthAndResourceManager;

	// Controlls and provides information about player collisions
	Controller2D controller;

	// Controlls animation for the player
	Animator anim;

	/** 
	 * Getter/Setter for Current Power
	 * Accessed by AbilityChooser.cs
	 * 
	 **/
	public string CurrentPower{
		get {
			return this.currentPower;
		}
		set {
			this.currentPower = value;
		}
	}

	/**
	 * Called when the an Enemy damages the player
	 * 
	 * @param dmg the amount of health to lose
	 * @return
	 *
	 **/
	public void ApplyDamage(int dmg) {
		health -= dmg;
		healthAndResourceManager.GetComponent<HealthAndResourceManager>().UpdateHealthBar(health);
	}


	/**
	 * Called when the player casts heal or picks up a health_pick_up
	 * 
	 * @param healthGained the amount of health to gain
	 * @return true if the player was healed,
	 * 		   false otherwise
	 *
	 **/
	public bool ApplyHeal(int healthGained) {
		if (health == maxHealth) {
			return false;
		} else {
			if (healthGained > maxHealth - health) {
				health = maxHealth;
			} else {
				health += healthGained;
			}
			healthAndResourceManager.GetComponent<HealthAndResourceManager>().UpdateHealthBar(health);
			return true;
		}
	}

	/**
	 * Called when the player picks up an abilityPower_pick_up
	 * 
	 * @param abilitypowerGained the amount of ability power to gain
	 * @return true if the player gained AP,
	 * 		   false otherwise
	 *
	 **/
	public bool ApplyAbilitypowerpack(int abilitypowerGained) {
		if (abilitypower == maxAbilitypower) {
			return false;
		} else {
			if (abilitypowerGained > maxAbilitypower - abilitypower) {
				abilitypower = maxAbilitypower;
			} else {
				abilitypower += abilitypowerGained;
			}
			healthAndResourceManager.GetComponent<HealthAndResourceManager>().UpdateAbilityPowerBar(abilitypower);
			return true;
		}
	}

	/** 
	 * Getter/Setter for ability power
	 * 
	 **/
	public int AbilityPower {
		get {
			return abilitypower;
		}
		set { 
			abilitypower = value;
			healthAndResourceManager.GetComponent<HealthAndResourceManager>().UpdateAbilityPowerBar(abilitypower);
		}
	}

	/** 
	 * Getter/Setter for velocity
	 * 
	 **/
	public Vector3 Velocity {
		get {
			return this.velocity;
		}
		set { 
			this.velocity = value;
		}
	}

	/** 
	 * Enables/Disables gravity on the player
	 * 
	 * @param val is true if gravity is to be enabled
	 * 
	 **/
	public void gravityEnabled(bool val) {
		this.useGravity = val;
	}

	// Initialize
	void Start() {
		controller = GetComponent<Controller2D> ();
		anim = GetComponent<Animator>();
		healthAndResourceManager = HARM;

		// Kinematic equations to find 'gravity' and 'Jump Velocity' from 'Jumpheight' and 'time to MaxY'
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

	}
	 // Caleld every frame
	void Update() {
		
		if (this.health <= 0) {
			die();

			velocity.y = -9; // fall to ground if died in mid-air
			velocity.x = 0;
			willJump = false; // Cannot jump while dead!

			if (deathTransition) {
				
				// Death Screen Scene
				MainMenuUIManager.launchGame(MainMenuUIManager.DeathMenu);
			}
		}

		updateIsGrounded();

		// Get input for this frame
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		float targetVelocityX = input.x * walkSpeed;

		// Use of controller collisions to restrict player velocity
		if (controller.collisions.above || (controller.collisions.below && !anim.GetCurrentAnimatorStateInfo(0).IsName("Standing_Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch_Idle"))) {
			velocity.y = 0;
		}

		// Jump if the player indicated so on the last frame
		if (willJump && isGrounded) {
			velocity.y = jumpVelocity;
			willJump = false;
		}

		// Update player animations
		else if (controller.collisions.below) {
			anim.SetBool("isJump", false);
			anim.SetBool("isGrounded", true);
			anim.SetBool("isFalling", false);
		} else {
			anim.SetBool("isGrounded", false);
		}

		// Restrict movement if casting an ability
		if (performingAbility()) {
			if (!anim.GetBool("isJump") && !anim.GetBool("isPowerjump")) {
				targetVelocityX = 0;
			}
			jumpPermitted = false;
		} else {
			jumpPermitted = true;
		}

		// jump animation is over due to blend of 99%
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("jump") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .99f) {
			anim.SetBool("isJump", false);
		} else if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpPermitted) {
			anim.SetBool("isJump", true);
			willJump = true;
		}

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || anim.GetBool("isJump")) {
			targetVelocityX *= runMultiplier;
		}

		//
		// Fire Abilities
		//

		// fireball
		if (!anim.GetBool("isFireball") && currentPower.Equals("Fire") && Input.GetKey(KeyCode.E) && controller.collisions.below && !performingAbility() && !anim.IsInTransition(0) && abilitypower >= 20) {
			anim.SetBool("isJump", false);
			anim.SetBool("isFireball", true);
			targetVelocityX = 0;
		}

		// fireball cast animation is over due to blend of 98%
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("fireball") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .98f) {
			anim.SetBool("isFireball", false);
		}

		// firewave
		if (!anim.GetBool("isFirewave") &&  currentPower.Equals("Fire") && Input.GetKey(KeyCode.R) && controller.collisions.below && !performingAbility() && !anim.IsInTransition(0) && abilitypower >= 50) {
			anim.SetBool("isJump", false);
			anim.SetBool("isFirewave", true);
			targetVelocityX = 0;
		}

		// firewave cast animation is over due to blend of 98%
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("firewave") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .98f) {
			anim.SetBool("isFirewave", false);
		}

		//
		// Air Abilities
		//

		// powerjump
		if (!anim.GetBool("isPowerjump_1") && currentPower.Equals("Air") && Input.GetKey(KeyCode.E) && controller.collisions.below && !performingAbility() && !anim.IsInTransition(0)) {
			anim.SetBool("isJump", false);
			//anim.SetBool("isPowerjump_1", true);
			targetVelocityX = 0;
		}

		//
		// Water Abilities
		//

		// iceball
		if (!anim.GetBool("isIceball") && currentPower.Equals("Water") && Input.GetKey(KeyCode.E) && controller.collisions.below && !performingAbility() && !anim.IsInTransition(0) && abilitypower >= 20) {
			anim.SetBool("isJump", false);
			anim.SetBool("isIceball", true);
			targetVelocityX = 0;
		}

		// iceball cast animation is over due to blend of 98%
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("iceball") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .98f) {
			anim.SetBool("isIceball", false);
		}

		// Provides a smooth transition from walking to running and vice versa
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityZSmoothing, (controller.collisions.below) ? accelerationTimeGrounded+.001f : accelerationTimeAirborne+.001f);

		// Move the player downward if there is no floor beneath the player and gravity is in use
		if (!controller.collisions.below && useGravity) {
			velocity.y += gravity * Time.deltaTime;
		}

		// Update movement animation
		anim.SetFloat("speed", Mathf.Abs(velocity.x) / (walkSpeed * runMultiplier));
		// Move the player this frame
		controller.Move(velocity * Time.deltaTime);

		// Controlls which direction the player is facing
		if (controller.collisions.below) {
			Transform Mesh = transform.GetChild(0).transform;
			if (targetVelocityX < 0) {
				Mesh.rotation = Quaternion.Euler(0, -90, 0);
			} else if (targetVelocityX > 0) {
				Mesh.rotation = Quaternion.Euler(0, 90, 0);
			}
		}

	}

	/**
	 * Called when the player's health <= 0
	 * 
	 * @param
	 * @return
	 *
	 **/
	private void die() {
		if (!hasDied) {
			hasDied = true; // only die once or the animation jitters
			anim.SetTrigger("isDead");

			// Allow 3.0s to pass for the death animation to player before loading the death scene
			System.Threading.Timer timer = null;
			timer = new System.Threading.Timer((obj) => {
				deathTransition = true;
				timer.Dispose();

			}, null, 3000, System.Threading.Timeout.Infinite);
		}

	}

	/**
	 * If the player is in any animation states related to performing an ability or jumping
	 * 
	 * @param 
	 * @return true if the player is,
	 * 		   false otherwise
	 *
	 **/
	public bool performingAbility() {
		return anim.GetBool("isFirewave") || anim.GetBool("isFireball") || anim.GetBool("isIceball") || anim.GetBool("isPowerjump_1") || anim.GetBool("isPowerjump_2") || anim.GetBool("isJump");
	}
		
	/**
	 * If the player meets the physcial requirements to perform glide
	 * 
	 * @param 
	 * @return true if the player can,
	 * 		   false otherwise
	 *
	 **/
	public bool canGlide() {
		// Must not be grounded and must be falling
		if (!controller.collisions.below && velocity.y < -.01) {
			return true;
		}
		return false;
	}

	//
	// Controller2D.cs is grounded every other frame or so due to a constant slight shift of the player upward when it collides with the ground and slight shift down again until the player again contacts the ground
	// This loops continues indefinitely
	// The following code checks if the player has been grounded sometime the last couple frames instead of 'controller.collisions' single frame detection
	// This useful when jumping as it prevents a missed jump in between the small frames when a player is NOT --technically-- grounded
	//

	private const float isGroundedTickTime = .1f;
	private float currentGroundedTickTime = .1f;
	private float totalUngrounedTime = 0f;
	private const float totalUngrounedTimeRequiredForFall = .35f;
	private bool isGrounded = false;

	void updateIsGrounded() {
		if (isGrounded && currentGroundedTickTime >= 0) {
			currentGroundedTickTime -= Time.deltaTime;
		} else if (anim.GetBool("isGrounded")) {
			isGrounded = true;
			currentGroundedTickTime = isGroundedTickTime;
			totalUngrounedTime = 0f;
		} else {
			isGrounded = false;
			totalUngrounedTime += Time.deltaTime;
		}
		if (totalUngrounedTime >= totalUngrounedTimeRequiredForFall) {
			anim.SetBool("isFalling", true);
		}
	}

	//
	// Unimplemented mechanic
	//
	/*void updateFallingTime(){
		Debug.Log(this.fallingTime);
		if (isFalling) {
			this.fallingTime += Time.deltaTime;
		}
		if(velocity.y >= -.001) {
			isFalling = false;
			this.fallingTime = 0f;
		}
		else if(velocity.y <= -.001) {
			isFalling = true;
		}
	}
	public float getFallingTime() {
		return this.fallingTime;
	}*/
}