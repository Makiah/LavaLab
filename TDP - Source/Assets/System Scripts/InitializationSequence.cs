
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
	public static event BaseInitialization InitializeCameras;

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
		if (InitializeCostume != null) InitializeCostume(); else Debug.LogError("InitializeCostume was null!"); //Used for PlayerCostumeManager
		if (InitializePlayer != null) InitializePlayer (); else Debug.LogError("InitializePlayer was null!"); //Used for initializing the HumanoidBaseReferenceClass.  

		if (InitializeCameraFunctions != null) InitializeCameraFunctions (); else Debug.LogError("InitializeCameraFunctions was null!"); // Used for camera controller.  

		yield return null;
	}

}
