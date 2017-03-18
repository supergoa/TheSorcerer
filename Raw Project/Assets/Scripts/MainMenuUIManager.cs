using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * Created by Nicholas Ostaffe on 3/12/2017.
 * UI Script that manages the main menu and current scene
 * 
 **/
public class MainMenuUIManager : MonoBehaviour 
{
	public Canvas quitMenu;
	public Canvas levelMenu;
	public Canvas instructionsMenu;

	public Text startGame;
	public Text levelSelect;
	public Text instructions;
	public Text quit;

	public Font defaultFont;

	public Dropdown levelSelectDropdown;
	public static int currentLevel;

	private Button startGameButton;
	private Button quitButton;
	private Button levelMenuButton;
	private Button instructionsMenuButton;

	// In the build, scenes indexed 0, 1, and 2 are UI scenes.
	// Level 1 is indexed at 2, level 2 at 3, and so on.
	private const int sceneLevelOffset = 2;

	// See UIHoverManager.cs documentation
	private UIHoverManager[] texts;

	// Initialize
	void Start () {
		startGameButton = startGame.GetComponent<Button> ();
		levelMenuButton = levelSelect.GetComponent<Button>();
		quitButton = quit.GetComponent<Button> ();
		instructionsMenuButton = instructions.GetComponent<Button> ();

		quitMenu.enabled = false;
		instructionsMenu.enabled = false;
		levelMenu.enabled = false;
	}

	// Abstraction of the scene value for the main menu
	public static int MainMenu {
		get {
			return -2;
		}
	}

	// Abstraction of the scene value for the death menu
	public static int DeathMenu {
		get {
			return -1;
		}
	}

	// Abstraction of the scene value for the win menu
	public static int WinMenu {
		get {
			return 0;
		}
	}

	// Abstraction of the scene value for the current level
	public static int CurrentLevel {
		get {
			return currentLevel;
		}
		set {
			currentLevel = value;
		}
	}

	// Abstraction of the scene value for the next level
	public static int NextLevel {
		get {
			return currentLevel+1;
		}
	}

	/**
	 * Loads level 1
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public static void launchGame () {
		currentLevel = 1;
		SceneManager.LoadSceneAsync (sceneLevelOffset + currentLevel);
	}

	/**
	 * Called when "Start Game" is selected from the main menu via the inspector
	 *  
	 * @param
	 * @return
	 * 
	 **/
	public void launchGameWrapper () {
		launchGame();
	}

	/**
	 * Loads the appropriate UI Screen or level
	 *  
	 * @param level is the level to load
	 * @return
	 * 
	 **/
	public static void launchGame (int level) {
		
		// Is @param level a UI scene?
		if (sceneLevelOffset + level != 0 && sceneLevelOffset + level != 1 && sceneLevelOffset + level != 2) { // No
			currentLevel = level;
			SceneManager.LoadSceneAsync(currentLevel + sceneLevelOffset);
		} else { // Yes
			SceneManager.LoadSceneAsync(sceneLevelOffset + level);
		}
	}

	/**
	 * Wrapper method so {static void launchGame (int level)} can be used from the inspector tab
	 *  
	 * @param level is the level to load
	 * @return
	 * 
	 **/
	public void launchGameWrapper (int level) {
		launchGame(level);
	}

	/**
	 * Called by the 'Go' button on the levelSelect sub-menu
	 * Parses the Dropdownbox text and loads the respective level 
	 * 
	 * @param
	 * @return
	 * 
	 **/
	public void loadLevel() {
		int level = int.Parse(levelSelectDropdown.captionText.text);
		currentLevel = level;
		launchGame(currentLevel);
	}

	/**
	 * Utility method which disables all the buttons on the main menu
	 * Called when sub-menus are opened
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void disableMainButtons() {
		startGameButton.enabled = false;
		instructionsMenuButton.enabled = false;
		levelMenuButton.enabled = false;
		quitButton.enabled = false;
	}

	/**
	 * Utility method which disables all the hover animations on the main menu
	 * Called when sub-menus are opened
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void disableMainAnimations() {
		startGame.GetComponent<UIHoverManager>().enabled = false;
		quit.GetComponent<UIHoverManager>().enabled = false;
		instructions.GetComponent<UIHoverManager>().enabled = false;
		resetFonts();
	}

	/**
	 * Utility method which enables all the buttons on the main menu
	 * Called when sub-menus are closed
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void enableMainButtons() {
		startGameButton.enabled = true; 
		//optionsButton.enabled = true;
		levelMenuButton.enabled = true;
		instructionsMenuButton.enabled = true;
		quitButton.enabled = true;
	}

	/**
	 * Utility method which enables all the hover animations on the main menu
	 * Called when sub-menus are closed
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void enableMainAnimations() {
		startGame.GetComponent<UIHoverManager>().enabled = true;
		quit.GetComponent<UIHoverManager>().enabled = true;
		instructions.GetComponent<UIHoverManager>().enabled = true;
		resetFonts();
	}

	/**
	 * Utility method which resets all the hovered fonts on the main menu to their default faunts
	 * Called when sub-menus are closed
	 * 
	 * @param
	 * @return
	 * 
	 **/
	private void resetFonts() {
		texts = FindObjectsOfType(typeof(UIHoverManager)) as UIHoverManager[];
		for (int i = 0; i < texts.Length; i++) {
			texts[i].GetComponentInParent<Text>().font = defaultFont;
		}
	}

	public void lauchQuitMenu() {
		// enable sub-menu
		quitMenu.enabled = true; 

		//disable main buttons & animations
		disableMainButtons();
		disableMainAnimations();
	}

	public void closeQuitMenu() {
		// disable sub-menu
		quitMenu.enabled = false; 

		// re-enable main buttons & animations
		enableMainButtons();
		enableMainAnimations();
	}

	public void launchLevelMenu () {
		// enable sub-menu
		levelMenu.enabled = true; 

		// disable main buttons & animations
		disableMainButtons();
		disableMainAnimations();
	}

	public void closeLevelMenu () {
		// enable sub-menu
		levelMenu.enabled = false; 

		// disable main buttons & animations
		enableMainButtons();
		enableMainAnimations();
	}
	public void launchInstructionsMenu () {
		// enable sub-menu
		instructionsMenu.enabled = true; 

		// disable main buttons & animations
		disableMainButtons();
		disableMainAnimations();
	}

	public void closeInstructionsMenu () {
		// enable sub-menu
		instructionsMenu.enabled = false; 

		// disable main buttons & animations
		enableMainButtons();
		enableMainAnimations();
	}

	public void ExitGame () {
		Application.Quit();
	}

}