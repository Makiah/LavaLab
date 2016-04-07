using UnityEngine;
using System.Collections;

public class InteractablePanelController : MonoBehaviour {

	void OnEnable() {
		UIInitializationSequence.InitializeInteractablePanelController += InitializeInteractablePanelController;
	}

	void OnDisable() {
		UIInitializationSequence.InitializeInteractablePanelController -= InitializeInteractablePanelController;
	}

	InteractablePanelReference interactablePanel1;

	//Reference the Interactable Panel References.  
	void InitializeInteractablePanelController() {
		interactablePanel1 = transform.FindChild ("InteractablePanel1").GetComponent <InteractablePanelReference> ();
	}

	public InteractablePanelReference GetAvailableInteractablePanel() {
		if (interactablePanel1.IsEmpty ()) {
			return interactablePanel1;
		} else {
			return null;
		}
	}

}
