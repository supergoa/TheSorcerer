using UnityEngine;
using System.Collections;
//using UnityEditor.Animations;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script attached to the player which controlls movement capabilies through a series of collision mechanics
 * 
 **/
[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	// Determines what the player can collide with
	public LayerMask collisionMask;

	// Distance inset from the player object from which to fire raycasts
	const float skinWidth = .015f;

	// Number of horizontal raycasts to fire when detecting horizontal collision
	public int horizontalRayCount = 4;
	// Number of vertical raycasts to fire when detecting vertical collision
	public int verticalRayCount = 4;

	// Distance from floor in which to trigger a landing animation
	private const float almostLandedDistance = 5f;

	// Distance between each horizontal raycast
	float horizontalRaySpacing;
	// Distance between each vertical raycast
	float verticalRaySpacing;

	// The player's collider collides (prevents movement) with all other 2D Colliders in the scene (i.e. the ground)
	BoxCollider2D collider;

	//Struct that holds info about the corners of the player collider
	RaycastOrigins raycastOrigins;

	//Struct that holds info about the manner in which the player is colliding with another collider
	public CollisionInfo collisions;

	// Controlls the animation of the player object
	public Animator anim;

	void Start() {
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing();
	}

	/**
	 * Called by Player.cs every frame.
	 * Updates the velocity of the player based on input and collisions
	 * 
	 * @param velocity holds both the x and y velocities of the player.
	 * @return
	 */
	public void Move(Vector3 velocity) {

		InitializeRaycastOrigins();
		collisions.Reset();

		if (velocity.x != 0) {
			HorizontalCollisions(ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions(ref velocity);
		}
			
		transform.Translate (velocity);
	}

	/**
	 * Casts four horizontal rays in detection for other objects
	 * Updates collisionInfo and the x-velocity accordingly
	 * 
	 * @param velocity holds both the x and y velocities of the player.
	 * @return
	 */
	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		// Cast four rays
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

			// Space each ray evenly
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			// Fire the ray and store information about its collition in hit
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			if (hit) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}

	/**
	 * Casts four vertical rays in detection for other objects
	 * Updates collisionInfo and the y-velocity accordingly
	 * 
	 * @param velocity holds both the x and y velocities of the player.
	 * @return
	 */
	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		// Cast four rays
		for (int i = 0; i < verticalRayCount; i++ ) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

			// Space each ray evenly
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

			// Fire the ray and store information about its collition in hit
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;
				collisions.below = directionY == -1;
				collisions.above = directionY == 1;

				// Check if player is about to land
				if ((velocity.y<-.01 || anim.GetCurrentAnimatorStateInfo(0).IsName("falling")) && hit.distance <= almostLandedDistance) {
					anim.SetBool("isAlmostGrounded", true); //start landing animation
				} else {
					anim.SetBool("isAlmostGrounded", false);
				}
			}
		}
	}

	/**
	 * Sets up the bounds for raycasting depending on the player collider
	 * 
	 * @param
	 * @return
	 */
	private void InitializeRaycastOrigins() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	/**
	 * Calculates distance between each fired ray depending on the how many raycasts are being fired horizontally & vertically
	 * 
	 * @param
	 * @return
	 */
	private void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	/**
	 * Stores where the rays are fired from
	 * 
	 */
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	/**
	 * Stores whether the player is colliding with something in a certain direction (left, right, up, down)
	 * 
	 */
	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public void Reset() {
			above = below = false;
			left = right = false;
		}
	}

}
