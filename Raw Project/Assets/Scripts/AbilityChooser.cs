using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * HUD Script in charge of selecting current power
 * 
 **/
public class AbilityChooser : MonoBehaviour {


	// The ability wheel to hide when the power button is pushed
	public Canvas tohide;

	// The four buttons which enable the current power in use
	public Button earth;
	public Button air;
	public Button water;
	public Button fire;

	// Two icons are always present to the left and right of the power button resembling the abilities of the current power
	// For each power, the icon on the left represents the ability toggled by KeyCode 'E' and the right, KeyCode 'R'
	// These icons are enabled/disabled depending on the current power
	public GameObject earthIcons;
	public GameObject airIcons;
	public GameObject waterIcons;
	public GameObject fireIcons;

	// Required to play audio clips
	SoundManager sm;

	// Audio clips that are played when a power is selected
	public AudioClip earthSound;
	public AudioClip airSound;
	public AudioClip waterhSound;
	public AudioClip fireSound;

	// A reference to the sole player
	public GameObject player;

	// Initialize variables and set default power and respective icons
	void Start () {
		sm = new SoundManager();
		deactivateIcons();
		fireIcons.SetActive(true);
	}

	/**
	 * A button event triggers this method and hides the ability wheel
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void hide() {
		tohide.GetComponent<Canvas>().enabled = !tohide.GetComponent<Canvas>().enabled;
		if (tohide.GetComponent<Canvas>().enabled) {
			earth.enabled = true;
			air.enabled = true;
			water.enabled = true;
			fire.enabled = true;
		} else {
			earth.enabled = false;
			air.enabled = false;
			water.enabled = false;
			fire.enabled = false;
		}
	}

	/**
	 * Deactiveates all four power's icons
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void deactivateIcons() {
		earthIcons.SetActive(false);
		airIcons.SetActive(false);
		waterIcons.SetActive(false);
		fireIcons.SetActive(false);
	}

	/**
	 * A button event triggers this method
	 * Selects and configures the Earth power
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void selectEarth() {
		sm.PlaySound(earthSound, player.transform.position);

		deactivateIcons();
		earthIcons.SetActive(true);

		// Link from UI to Player.cs via a string which dictates the power in use
		player.GetComponent<Player>().CurrentPower = "Earth";

		hide();
	}

	/**
	 * A button event triggers this method
	 * Selects and configures the Air power
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void selectAir() {
		sm.PlaySound(airSound, player.transform.position);

		deactivateIcons();
		airIcons.SetActive(true);

		// Link from UI to Player.cs via a string which dictates the power in use
		player.GetComponent<Player>().CurrentPower = "Air";

		hide();
	}

	/**
	 * A button event triggers this method
	 * Selects and configures the Water power
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void selectWater() {
		sm.PlaySound(waterhSound, player.transform.position);

		deactivateIcons();
		waterIcons.SetActive(true);

		// Link from UI to Player.cs via a string which dictates the power in use
		player.GetComponent<Player>().CurrentPower = "Water";

		hide();
	}

	/**
	 * A button event triggers this method
	 * Selects and configures the Fire power
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void selectFire() {
		sm.PlaySound(fireSound, player.transform.position);

		deactivateIcons();
		fireIcons.SetActive(true);

		// Link from UI to Player.cs via a string which dictates the power in use
		player.GetComponent<Player>().CurrentPower = "Fire";

		hide();
	}
}