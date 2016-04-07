
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

	public static InitializationSequence instance;

	void Start() {
		instance = this;
	}

	//The loading bar.  
	//[SerializeField] private GameObject loadingBar = null;

	public delegate void BaseInitialization();

	//Notification UI
	public static event BaseInitialization InitializeNotificationUI;

	//Player
	//public static event BaseInitialization CreatePlayer;
	public static event BaseInitialization InitializePlayer; //Use for initializing CostumeManager and PlayerAction, as well as the PlayerHealthController.  
	public static event BaseInitialization InitializeCostume;

	//Camera
	public static event BaseInitialization InitializeCameraFunctions;
	//public static event BaseInitialization InitializeCameras;

	//Enemies
	public static event BaseInitialization InitializeEnemies;

	//NPCs
	public static event BaseInitialization InitializeNPCs;

	//Purchase Panels
	//public static event BaseInitialization InitializePurchasePanels;
	//public static event BaseInitialization InitializePurchasePanelManager;

	public static event BaseInitialization SetInactiveObjects;

	public IEnumerator LoadEverything() {
		//Add the main game UI
		AsyncOperation loadingOperation = SceneManager.LoadSceneAsync ("MainGameUI", LoadSceneMode.Additive);
		while (!loadingOperation.isDone) {
			yield return null;
		}

		InstanceDatabase.SetLevelReferences ();

		UIInitializationSequence.instance.Initialize ();

		if (InitializeCostume != null) InitializeCostume(); else Debug.LogError("InitializeCostume was null!"); //Used for PlayerCostumeManager
		if (InitializePlayer != null) InitializePlayer (); else Debug.LogError("InitializePlayer was null!"); //Used for initializing the HumanoidBaseReferenceClass.  

		if (InitializeCameraFunctions != null) InitializeCameraFunctions (); else Debug.LogError("InitializeCameraFunctions was null!"); // Used for camera controller.  
	}

}
