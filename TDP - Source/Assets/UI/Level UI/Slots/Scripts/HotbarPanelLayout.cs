using UnityEngine;
using System.Collections;

public class HotbarPanelLayout : PanelLayout {

	protected override void OnEnable() {
		InitializationSequence.CreateHotbarSlots += AddSlotsToSystem;
	}

	protected override void OnDisable() {
		InitializationSequence.CreateHotbarSlots -= AddSlotsToSystem;
	}

	//The inventory thing has to be on the actual inventory, not the hotbar.  
	protected override void AddSlotsToSystem() {
		transform.parent.FindChild("Inventory").GetComponent <InventoryFunctions> ().AddSlotsToSystem (InitializeSlots ());
	}

}
