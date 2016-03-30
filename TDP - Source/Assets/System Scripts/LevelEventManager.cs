
/*
 * Author: Makiah Bennett
 * Last edited: 18 November 2015
 * 
 * This script manages the entire path the game takes.  Each event is static, so it only belongs to this class.  Each OnEnable() and OnDsiable() 
 * assigns and de-assigns events to this class.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelEventManager : MonoBehaviour {

	//The loading bar.  
	[SerializeField] private GameObject loadingBar = null;

	//Pretty much contains every event that does not require a parameter or a return type.  
	public delegate void BaseInitialization();

	public static event BaseInitialization CreateInventorySlots;
	public static event BaseInitialization CreateHotbarSlots;

	public static event BaseInitialization InitializeSlots;
	public static event BaseInitialization InitializeHotbarManager;

	public static event BaseInitialization EnableUIHideShow;

	public static event BaseInitialization InitializeUIHealthController;
	public static event BaseInitialization InitializeHealthPanels;

	public static event BaseInitialization InitializeInteractablePanelController;
	public static event BaseInitialization InitializeInteractablePanels;

	public static event BaseInitialization InitializeUISpeechControl;

	public static event BaseInitialization InitializeObjectiveManager;

	public static event BaseInitialization CreatePlayer;

	public static event BaseInitialization InitializePlayer; //Use for initializing CostumeManager and PlayerAction, as well as the PlayerHealthController.  

	public static event BaseInitialization InitializeCostume;

	public static event BaseInitialization InitializeCameraFunctions;
	public static event BaseInitialization InitializeBackgroundManager;
	public static event BaseInitialization InitializeTimeIndicator;
	
	public static event BaseInitialization InitializeSystemWideParticleEffect;

	public static event BaseInitialization InitializeEnemyHealthControllers;
	public static event BaseInitialization InitializeEnemies;
	
	public static event BaseInitialization InitializeNPCPanelControllers;
	public static event BaseInitialization InitializeNPCs;
	
	public static event BaseInitialization InitializePurchasePanels;
	public static event BaseInitialization InitializePurchasePanelManager;

	public static event BaseInitialization InitializeDoors;

	public static event BaseInitialization SetInactiveObjects;

	//Pretty much the only Start() method in the whole program.  
	void Start() {
	}

	IEnumerator LoadEverything() {
		//Could be a useful reference later on, I'll keep this in.  
		/*AsyncOperation loadingOperation = SceneManager.LoadSceneAsync ("MainGameUI", LoadSceneMode.Additive);
		while (!loadingOperation.isDone) {
			yield return null;
		}*/

		yield return null;
	}

}
