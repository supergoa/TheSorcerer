using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * Script that manages the player HUD
 * 
 **/
public class HealthAndResourceManager : MonoBehaviour {
	// The Slider that represents ability power remaining for the player
	public Slider APSlider;
	// The Slider that represents health remaining for the player
	public Slider HPSlider;

	float playerHealthRatio = 1f;
	float playerAbilityPowerRatio = 1f;

	const int maxPlayerHealth = 200;
	const int maxplayerAbilityPower = 200;

	// Initialization
	void Start () {
		APSlider = APSlider.GetComponent<Slider>();
		HPSlider = HPSlider.GetComponent<Slider>();

		APSlider.maxValue = maxplayerAbilityPower;
		APSlider.minValue = 0;
		HPSlider.maxValue = maxPlayerHealth;
		HPSlider.minValue = 0;

		UpdateAbilityPowerBar(maxPlayerHealth);
		UpdateHealthBar(maxplayerAbilityPower);
	}

	/**
	 * Modifies the HUD's display of health
	 * 
	 * @param val is the amount of health to display
	 * @return
	 * 
	 **/
	public void UpdateHealthBar(int val) {
		HPSlider.value = val;
	}

	/**
	 * Modifies the HUD's display of ability power
	 * 
	 * @param val is the amount of ability power to display
	 * @return
	 * 
	 **/
	public void UpdateAbilityPowerBar(int val) {
		APSlider.value = val;
	}
}
