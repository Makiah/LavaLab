using UnityEngine;
using System.Collections;

public class HotbarPanelLayout : PanelLayout {

	protected override void OnEnable() {
		UIInitializationSequence.CreateHotbarSlots += AddSlotsToSystem;
	}

	protected override void OnDisable() {
		UIInitializationSequence.CreateHotbarSlots -= AddSlotsToSystem;
	}

	//The inventory thing has to be on the actual inventory, not the hotbar.  
	protected override void AddSlotsToSystem() {
		PlayerInventory.instance.AddSlotsToSystem (InitializeSlots ());
	}

}
