using UnityEngine;
using System.Collections;

public class UIInitializationSequence : MonoBehaviour {

	public static UIInitializationSequence instance;

	void Awake() {
		instance = this;
	}

	//UI
	public delegate void BaseInitialization();

	//NOTE: Use Awake() to set instance static variables.  

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

	public static event BaseInitialization InitializeEnemyHealthControllers;
	public static event BaseInitialization InitializeNPCPanelControllers;

	public void Initialize() {
		Debug.Log ("Starting UI Initialization");

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

		if (InitializeHotbarManager != null) InitializeHotbarManager (); else Debug.LogError("InitializeHotbarItems was null!"); //Used for initializing the HotbarManager.  

		if (InitializeEnemyHealthControllers != null) InitializeEnemyHealthControllers (); else Debug.LogError("InitializeEnemyHealthControllers was null!"); //Used for initializing CharacterHealthController.  

		if (InitializeNPCPanelControllers != null) InitializeNPCPanelControllers(); else Debug.LogError("InitializeNPCPanelControllers was null!");

	}

}
