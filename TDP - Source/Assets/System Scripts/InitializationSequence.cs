
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

public class InitializationSequence : MonoBehaviour {

	//The loading bar.  
	//[SerializeField] private GameObject loadingBar = null;

	//UI
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

	//Notification UI
	public static event BaseInitialization InitializeNotificationUI;

	//Player
	public static event BaseInitialization CreatePlayer;
	public static event BaseInitialization InitializePlayer; //Use for initializing CostumeManager and PlayerAction, as well as the PlayerHealthController.  
	public static event BaseInitialization InitializeCostume;

	//Camera
	public static event BaseInitialization InitializeCameraFunctions;

	//Enemies
	public static event BaseInitialization InitializeEnemyHealthControllers;
	public static event BaseInitialization InitializeEnemies;

	//NPCs
	public static event BaseInitialization InitializeNPCPanelControllers;
	public static event BaseInitialization InitializeNPCs;

	//Purchase Panels
	public static event BaseInitialization InitializePurchasePanels;
	public static event BaseInitialization InitializePurchasePanelManager;

	public static event BaseInitialization SetInactiveObjects;

	public IEnumerator LoadEverything() {
		//Could be a useful reference later on, I'll keep this in.  
		AsyncOperation loadingOperation = SceneManager.LoadSceneAsync ("MainGameUI", LoadSceneMode.Additive);
		while (!loadingOperation.isDone) {
			yield return null;
		}

		Debug.Log ("Loading Main Game UI is complete.");

		//Initialize everything!!!

		//Create slots, and define 2D array values.  
		if (CreateInventorySlots != null) CreateInventorySlots (); else Debug.LogError("CreateInventorySlots was null!"); // Used with PanelLayout
		if (CreateHotbarSlots != null) CreateHotbarSlots (); else Debug.LogError("CreateHotbarSlots was null!"); //Used with HotbarPanelLayout (Otherwise createdUISlots gets the hotbarslots return).  

		//Initialize Slots
		if (InitializeSlots != null) InitializeSlots (); else Debug.LogError("InitializeSlots was null!"); //Used with SlotScript

		//Hide/Show
		if (EnableUIHideShow != null) EnableUIHideShow (); else Debug.LogError("EnableUIHideShow was null!");//Used with InventoryHideShow
		//Health Panels
		if (InitializeUIHealthController != null) InitializeUIHealthController(); else Debug.LogError("InitializeUIHealthController was null!"); //Used for UIHealthController
		if (InitializeHealthPanels != null) InitializeHealthPanels (); else Debug.LogError("InitializeHealthPanels was null!"); //Used for HealthPanelReference and PlayerHealthPanelReference.  

		//Interactable Panels
		if (InitializeInteractablePanelController != null) InitializeInteractablePanelController(); else Debug.LogError("InitializeInteractablePanelController was null!");
		if (InitializeInteractablePanels != null) InitializeInteractablePanels(); else Debug.LogError("InitializeInteractablePanels was null!");
		//Speech control
		if (InitializeUISpeechControl != null) InitializeUISpeechControl (); else Debug.LogError("InitializeUISpeechControl was null!");

		if (InitializeNotificationUI != null) InitializeNotificationUI(); else Debug.LogError("InitializeNotificationUI was null!");

		//Has to be done after the player is instantiated.  
		CurrentLevelVariableManagement.SetLevelReferences ();

		if (InitializeHotbarManager != null) InitializeHotbarManager (); else Debug.LogError("InitializeHotbarItems was null!"); //Used for initializing the HotbarManager.  

		if (InitializeCostume != null) InitializeCostume(); else Debug.LogError("InitializeCostume was null!"); //Used for PlayerCostumeManager
		if (InitializePlayer != null) InitializePlayer (); else Debug.LogError("InitializePlayer was null!"); //Used for initializing the HumanoidBaseReferenceClass.  

		if (InitializeCameraFunctions != null) InitializeCameraFunctions (); else Debug.LogError("InitializeCameraFunctions was null!"); // Used for camera controller.  

		if (InitializeEnemyHealthControllers != null) InitializeEnemyHealthControllers (); else Debug.LogError("InitializeEnemyHealthControllers was null!"); //Used for initializing CharacterHealthController.  
		if (InitializeEnemies != null) InitializeEnemies(); else Debug.LogError("InitializeEnemies was null!"); //Used for all enemies (requires player being instantiated).  

		if (InitializeNPCPanelControllers != null) InitializeNPCPanelControllers(); else Debug.LogError("InitializeNPCPanelControllers was null!");
		if (InitializeNPCs != null) InitializeNPCs(); else Debug.LogError("InitializeNPCs was null!");

		if (InitializePurchasePanels != null) InitializePurchasePanels(); else Debug.LogError("InitializePurchasePanels was null!");
		if (InitializePurchasePanelManager != null) InitializePurchasePanelManager(); else Debug.LogError("InitializePurchasePanelManager is null!");

		if (SetInactiveObjects != null) SetInactiveObjects (); else Debug.LogError("HideInventories is null!");

		yield return null;
	}

}
